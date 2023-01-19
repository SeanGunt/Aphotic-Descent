// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#ifndef WCE_COMMON_INCLUDED
#define WCE_COMMON_INCLUDED

#if defined(_LIGHT_COOKIES)
    #pragma warning(disable : 3595) // ループ内勾配命令warning
#endif

#if !defined(SHADERGRAPH_PREVIEW)
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "hextiling.hlsl"
#endif

#define WCE_LIT_DIR_MIN_Y 0.1

#if defined(WCE_USE_SAMPLER2D_INSTEAD_TEXTURE2D)
    #define WCE_TEX_ARGS(texName) sampler2D texName
    #define WCE_TEX_SAMPLE_GRAD(st, texName, uv, dx, dy) tex2Dgrad(st.texName, uv, dx, dy)
    #define WCE_TEX_STRUCT(texName) sampler2D texName
    #define WCE_TEX_STORE(st, texName) st.texName = texName
#else
    #define WCE_TEX_ARGS(texName) TEXTURE2D(texName), SAMPLER(texName##SS)
    #define WCE_TEX_SAMPLE_GRAD(st, texName, uv, dx, dy) SAMPLE_TEXTURE2D_GRAD(st.texName, st.texName##SS, uv, dx, dy)
    #define WCE_TEX_STRUCT(texName) TEXTURE2D(texName); SAMPLER(texName##SS)
    #define WCE_TEX_STORE(st, texName) st.texName = texName; st.texName##SS = texName##SS
#endif


struct WCE_SamplingData {
    WCE_TEX_STRUCT(CausticsTex);
    float3 pos;
    float3 posDDX;
    float3 posDDY;
    int3 channel;
    float2 colShift;
    float2x2 rotMatrix;
    bool useTiling;
    uint tilingSeed;
    float tilingRot;
    float tilingHard;
    float2 dx;
    float2 dy;
};

struct WCE_Lit {
    half3 dir;
    half3 dirDDX;
    half3 dirDDY;
    half3 color;
    half atten;
    half shadow;
};

struct WCE_CommonData {
    float3 posW;
    float3 posWdX;
    float3 posWdY;
    half3 normWS;
    float2 scrnUV;
    half shadow;
    half saturation;
    half normAtten;
    half normRate;
    half transparent;
    half backShadow;
    float depthSign;
    half depthFade;
    uint layer;
    float3 posOnSurf;
};


// Mainライト事前チェック  水面通過時、光線が上向き(1)か下向き(-1)か / RenderingLayerMaskチェック
void WCE_mainLitPreCheck(WCE_CommonData dt, out float signLitY, out bool result) {
    #if defined(SHADERGRAPH_PREVIEW)
        signLitY = -1;
        result = true;
    #else
        Light l = GetMainLight();
        signLitY = sign(l.direction.y);
        #ifdef _LIGHT_LAYERS
            result = (dt.depthSign == signLitY) && IsMatchingLightLayer(l.layerMask, dt.layer);
        #else
            result = (dt.depthSign == signLitY);
        #endif
    #endif
}

// Addライト事前チェック  水面通過時、光線が上向き(1)か下向き(-1)か / RenderingLayerMaskチェック
void WCE_addLitPreCheck(uint idx, WCE_CommonData dt, out float signLitY, out bool result) {
    #if defined(SHADERGRAPH_PREVIEW)
        signLitY = -1;
        result = false;
    #else
        Light l = GetAdditionalLight(idx, dt.posOnSurf);
        signLitY = sign(l.direction.y);
        #ifdef _LIGHT_LAYERS
            result = (dt.depthSign == signLitY) && IsMatchingLightLayer(l.layerMask, dt.layer);
        #else
            result = (dt.depthSign == signLitY);
        #endif
    #endif
}


// Main Light Data
void WCE_getMainLitData(WCE_CommonData dt, out WCE_Lit lit) {
    lit = (WCE_Lit)0;
    #if defined(SHADERGRAPH_PREVIEW)
        lit.dir = normalize(half3(0, 1, -0.4));
        lit.color = half3(1, 1, 1);
        lit.atten = lit.shadow = 1;
    #else
        #if (defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE) || defined(_MAIN_LIGHT_SHADOWS_SCREEN)) && !defined(_RECEIVE_SHADOWS_OFF)
            #if defined(_MAIN_LIGHT_SHADOWS_SCREEN) && !defined(_SURFACE_TYPE_TRANSPARENT)
                float4 shadowCoord = float4(dt.scrnUV.xy, 0, 1);
            #else
                float4 shadowCoord = TransformWorldToShadowCoord(dt.posW);
            #endif
            Light l = GetMainLight(shadowCoord, dt.posW, half4(1, 1, 1, 1));
        #else
            Light l = GetMainLight();
        #endif
        lit.dir = lit.dirDDX = lit.dirDDY = l.direction;
        lit.color = l.color;
        lit.atten = l.distanceAttenuation;
        lit.shadow = l.shadowAttenuation;
    #endif
}

// Additional Light Data
void WCE_getAddLitData(uint idx, WCE_CommonData dt, out WCE_Lit lit) {
    lit = (WCE_Lit)0;
    #if defined(SHADERGRAPH_PREVIEW)
        lit.dir = lit.color = lit.dirDDX = lit.dirDDY = half3(0, -1, 0);
        lit.atten = lit.shadow = 0;
    #else
        #if defined(_ADDITIONAL_LIGHT_SHADOWS) && !defined(_RECEIVE_SHADOWS_OFF)
            Light l = GetAdditionalLight(idx, dt.posW, half4(1, 1, 1, 1));
        #else
            Light l = GetAdditionalLight(idx, dt.posW);
        #endif
        lit.dir = l.direction;
        lit.color = l.color;
        lit.atten = l.distanceAttenuation;
        lit.shadow = l.shadowAttenuation;
        lit.dirDDX = GetAdditionalLight(idx, dt.posWdX).direction;
        lit.dirDDY = GetAdditionalLight(idx, dt.posWdY).direction;
    #endif
}


// 水面付近減衰
half WCE_SurfFade(float depthAbs, float SurfFadeStart, float SurfFadeCoef) {
    return saturate((depthAbs - SurfFadeStart) * SurfFadeCoef);
}

// 深さによる減衰
half WCE_depthFade(float depthAbs, float DepthFadeStart, float DepthFadeCoef) {
    return saturate(1 - (depthAbs - DepthFadeStart) * DepthFadeCoef);
}

// ライト角度での減衰
half WCE_litDirAtten(half3 litDir) {
    return saturate((abs(litDir.y) - WCE_LIT_DIR_MIN_Y) / (0.4 - WCE_LIT_DIR_MIN_Y));
}


// 減衰をまとめる
half WCE_calcAtten(WCE_CommonData dt, WCE_Lit lit, half dotLN) {
    half dot = saturate(dotLN) + saturate(-dotLN) * dt.transparent;
    dot = pow(abs(dot), dt.normRate);
    dot = 1 - (1 - dot) * dt.normAtten;
    half litDirAtten = WCE_litDirAtten(lit.dir);
    return lit.atten * lit.shadow * dot * litDirAtten * dt.depthFade;
}


// 彩度調整
half3 WCE_adjustSaturation(half3 litColor, WCE_CommonData dt) {
    half gray = dot(litColor, half3(0.299, 0.587, 0.114));
    return lerp(gray.xxx, litColor, dt.saturation.xxx);
}




half3 WCE_sampleColGrad(WCE_SamplingData smpDt, float2 uvG) {
    float2 uvR = uvG - smpDt.colShift;
    float2 uvB = uvG + smpDt.colShift;
    half3 c;
    c.r = WCE_TEX_SAMPLE_GRAD(smpDt, CausticsTex, uvR, smpDt.dx, smpDt.dy)[smpDt.channel[0]];
    c.g = WCE_TEX_SAMPLE_GRAD(smpDt, CausticsTex, uvG, smpDt.dx, smpDt.dy)[smpDt.channel[1]];
    c.b = WCE_TEX_SAMPLE_GRAD(smpDt, CausticsTex, uvB, smpDt.dx, smpDt.dy)[smpDt.channel[2]];
    return c;
}

half3 WCE_sampleRandomTiling(WCE_SamplingData smpDt, float2 uvG) {
    #if defined(SHADERGRAPH_PREVIEW)
        return WCE_sampleColGrad(smpDt, uvG);
    #else
        float2 uv1, uv2, uv3;
        float3 w;
        HEX_TILING_GetUV(uvG, smpDt.tilingRot, smpDt.tilingSeed, uv1, uv2, uv3, w);
        half3 c1 = WCE_sampleColGrad(smpDt, uv1);
        half3 c2 = WCE_sampleColGrad(smpDt, uv2);
        half3 c3 = WCE_sampleColGrad(smpDt, uv3);
        return HEX_TILING_MixColor(smpDt.tilingHard, c1, c2, c3, w);
    #endif
}

// テクスチャサンプリング
half3 WCE_sampleCausticsTex(WCE_SamplingData smpDt, WCE_Lit lit, float signLitY) {
    // ※litDir.yの絶対値がWCE_LIT_DIR_MIN_Y以下の場合はattenが0なので0除算は無視
    // smpDt.posは水面基準の位置(Density乗算済み) yは深さ
    float2 uvG = (lit.dir.xz / lit.dir.y) * smpDt.pos.y + smpDt.pos.xz;
    float2 uvDDX = (lit.dirDDX.xz / lit.dirDDX.y) * smpDt.posDDX.y + smpDt.posDDX.xz;
    float2 uvDDY = (lit.dirDDY.xz / lit.dirDDY.y) * smpDt.posDDY.y + smpDt.posDDY.xz;
    uvG = mul(smpDt.rotMatrix, uvG);
    smpDt.dx = mul(smpDt.rotMatrix, uvDDX) - uvG;
    smpDt.dy = mul(smpDt.rotMatrix, uvDDY) - uvG;

    uvG += signLitY * 0.05; // ←鏡面すぎると不自然なのでずらす
    half3 c;
    [branch] if (smpDt.useTiling) {
        c = WCE_sampleRandomTiling(smpDt, uvG);
    } else {
        c = WCE_sampleColGrad(smpDt, uvG);
    }
    return c;
}

half WCE_calcShadowIntensity(WCE_Lit lit, half dotLN, WCE_CommonData dt) {
    #if defined(_RECEIVE_SHADOWS_OFF)
        return lit.shadow;
    #else
        return 1 - (1 - lit.shadow) * lerp(dt.backShadow * (1 - step(dt.transparent, 0) * step(1, dt.normAtten)), dt.shadow, step(0, dotLN));
    #endif
}

uint WCE_getMeshRendLayer() {
    #if defined(SHADERGRAPH_PREVIEW) || !defined(_LIGHT_LAYERS)
        return 255;
    #else
        return GetMeshRenderingLightLayer();
    #endif
}

half3 WCE_commonLitProcess(WCE_CommonData dt, WCE_SamplingData smpDt, half Intensity, WCE_Lit lit, float signLitY, half3 debugCol) {
    half dotLN = dot(lit.dir, dt.normWS);
    lit.shadow = WCE_calcShadowIntensity(lit, dotLN, dt);
    half3 color = half3(0, 0, 0);
    [branch] if (lit.atten * lit.shadow > 0) {
        half atten = WCE_calcAtten(dt, lit, dotLN);
        [branch] if (atten > 0) {
            #if defined(WCE_DEBUG_AREA)
                color = debugCol;
            #else
                half3 texColor = WCE_sampleCausticsTex(smpDt, lit, signLitY);
                lit.color = WCE_adjustSaturation(lit.color, dt);
                color = texColor * lit.color * atten * Intensity;
            #endif
        }
    }
    return color;
}


half3 WCE_addLitProcess(uint i, WCE_CommonData dt, WCE_SamplingData smpDt, half Intensity) {
    WCE_Lit lit;
    float signLitY;
    bool checkResult;
    half3 debugCol = half3(0, 0.1, 0);
    WCE_addLitPreCheck(i, dt, /*out*/ signLitY, checkResult);
    half3 color = half3(0, 0, 0);
    [branch] if (checkResult) {
        WCE_getAddLitData(i, dt, /*out*/ lit);
        color = WCE_commonLitProcess(dt, smpDt, Intensity, lit, signLitY, debugCol);
    }
    return color;
}


// Main
half3 WCE_EffectCore(float3 WorldPos, half3 NormalWS, float2 ScreenUV, WCE_TEX_ARGS(CausticsTex), float2 TexRotSinCos, int3 TexChannels, int TilingSeed, float TilingRot, float TilingHard, float Density, float SurfY, float SurfFadeStart, float SurfFadeCoef, float DepthFadeStart, float DepthFadeCoef, half MainLitIntensity, half AddLitIntensity, half ShadowIntensity, float2 ColorShift, half LitSaturation, half NormalAtten, half NormalAttenRate, half TransparentBack, half BacksideShadow) {
    #if defined(SHADERGRAPH_PREVIEW)
        SurfY = 10;
        SurfFadeStart = DepthFadeStart = DepthFadeCoef = AddLitIntensity = 0;
        SurfFadeCoef = 1;
    #endif

    #if defined(_WCE_DISABLED)
        return half3(0, 0, 0);
    #else
        WCE_CommonData dt;
        dt.posW = WorldPos;
        dt.posWdX = WorldPos + ddx(WorldPos);
        dt.posWdY = WorldPos + ddy(WorldPos);
        dt.normWS = NormalWS;
        dt.scrnUV = ScreenUV;
        dt.shadow = ShadowIntensity;
        dt.saturation = LitSaturation;
        dt.normAtten = NormalAtten;
        dt.normRate = max(NormalAttenRate, 1);
        dt.transparent = TransparentBack;
        dt.backShadow = BacksideShadow;
        float depth = SurfY - WorldPos.y;
        float depthAbs = abs(depth);
        dt.depthSign = sign(depth);
        dt.depthFade = WCE_SurfFade(depthAbs, SurfFadeStart, SurfFadeCoef) * WCE_depthFade(depthAbs, DepthFadeStart, DepthFadeCoef);
        dt.layer = WCE_getMeshRendLayer();
        dt.posOnSurf = float3(WorldPos.x, SurfY, WorldPos.z);

        WCE_SamplingData smpDt;
        smpDt.useTiling = (TilingSeed >= 0);
        smpDt.tilingSeed = asuint(TilingSeed);
        smpDt.tilingRot = TilingRot;
        smpDt.tilingHard = TilingHard;
        smpDt.rotMatrix = float2x2(TexRotSinCos.y, -TexRotSinCos.x, TexRotSinCos.x, TexRotSinCos.y);
        smpDt.colShift = mul(smpDt.rotMatrix, ColorShift);
        smpDt.channel = TexChannels;
        smpDt.pos = float3(dt.posW.x, depth, dt.posW.z) * Density;
        smpDt.posDDX = float3(dt.posWdX.x, depth, dt.posWdX.z) * Density;
        smpDt.posDDY = float3(dt.posWdY.x, depth, dt.posWdY.z) * Density;
        WCE_TEX_STORE(smpDt, CausticsTex);

        half3 color = half3(0, 0, 0);

        [branch] if (MainLitIntensity > 0) {
            float signLitY;
            bool checkResult;
            WCE_mainLitPreCheck(dt, /*out*/ signLitY, checkResult);
            [branch] if (checkResult) {
                WCE_Lit lit;
                half3 debugCol = half3(0.2, 0, 0);
                WCE_getMainLitData(dt, /*out*/ lit);
                color = WCE_commonLitProcess(dt, smpDt, MainLitIntensity, lit, signLitY, debugCol);
            }
        }

        #if !defined(SHADERGRAPH_PREVIEW)
            #if defined(_ADDITIONAL_LIGHTS) || (defined(SHADERPASS) && (SHADERPASS == SHADERPASS_GBUFFER)) // Deferredは素通り
                [branch] if (AddLitIntensity > 0) {
                    #if defined(_FORWARD_PLUS)
                        // [Forward+]
                        uint dirLitCnt = URP_FP_DIRECTIONAL_LIGHTS_COUNT;
                        [loop] for (uint i = 0; i < min(dirLitCnt, MAX_VISIBLE_LIGHTS); i++) {
                            color += WCE_addLitProcess(i, dt, smpDt, AddLitIntensity);
                        }
                        uint idx;
                        ClusterIterator iterator = ClusterInit(ScreenUV, WorldPos, 0);
                        [loop] while (ClusterNext(iterator, /*out*/ idx)) {
                            color += WCE_addLitProcess(idx + dirLitCnt, dt, smpDt, AddLitIntensity);
                        }
                    #else
                        // [Forward] & [Deferred]
                        uint pixelLitCnt = GetAdditionalLightsCount();
                        [loop] for (uint i = 0u; i < pixelLitCnt; i++) {
                            color += WCE_addLitProcess(i, dt, smpDt, AddLitIntensity);
                        }
                    #endif
                }
            #endif
        #endif

        return color;
    #endif
}

#if defined(_LIGHT_COOKIES)
    #pragma warning(enable : 3595) // ループ内勾配命令warning
#endif

#endif
