// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

#ifndef WCECF_SYNC_COMMON_INCLUDED
#define WCECF_SYNC_COMMON_INCLUDED

#include "../../Effect/Shaders/WaterCausticsEffectCommon.hlsl"

CBUFFER_START(_WCECF_)
    float _WCECF_Density;
    int3 _WCECF_TexChannels;
    float2 _WCECF_TexRotateSinCos;
    int _WCECF_TilingSeed;
    float _WCECF_TilingRot;
    float _WCECF_TilingHard;
    float _WCECF_SurfaceY;
    float _WCECF_SurfFadeStart;
    float _WCECF_SurfFadeCoef;
    float _WCECF_DepthFadeStart;
    float _WCECF_DepthFadeCoef;
    float _WCECF_DistanceFadeStart;
    float _WCECF_DistanceFadeCoef;
    half _WCECF_IntensityMainLit;
    half _WCECF_IntensityAddLit;
    float2 _WCECF_ColorShift;
    half _WCECF_LitSaturation;
    half _WCECF_MultiplyIntensity;
    half _WCECF_NormalAtten;
    half _WCECF_NormalAttenRate;
    half _WCECF_TransparentBack;
    half _WCECF_BacksideShadow;
    half _WCECF_ShadowIntensity;
    int _WCECF_ClipOutside;
    int _WCECF_UseImageMask;
    float4x4 _WCECF_WorldToObjMatrix;
CBUFFER_END

#if defined(WCE_USE_SAMPLER2D_INSTEAD_TEXTURE2D)
    sampler2D _WCECF_CausticsTex;
    sampler2D _WCECF_ImageMaskTex;
    #define WCE_TEX_PARAMS_RAW(texName) texName
    #define WCE_TEX_SAMPLE_RAW(texName, uv) tex2D(texName, uv)
#else
    TEXTURE2D(_WCECF_CausticsTex);
    TEXTURE2D(_WCECF_ImageMaskTex);
    SAMPLER(sampler_WCECF_CausticsTex);
    SAMPLER(sampler_WCECF_ImageMaskTex);
    #define WCE_TEX_PARAMS_RAW(texName) texName, sampler##texName
    #define WCE_TEX_SAMPLE_RAW(texName, uv) texName.Sample(sampler##texName, (uv))
#endif


half3 WCECF_syncCore(float3 WorldPos, half3 NormalWS, float2 ScreenUV, half3 BaseColor, half aten) {
    half3 c = WCE_EffectCore(WorldPos, NormalWS, ScreenUV, WCE_TEX_PARAMS_RAW(_WCECF_CausticsTex), _WCECF_TexRotateSinCos, _WCECF_TexChannels, _WCECF_TilingSeed, _WCECF_TilingRot, _WCECF_TilingHard, _WCECF_Density, _WCECF_SurfaceY, _WCECF_SurfFadeStart, _WCECF_SurfFadeCoef, _WCECF_DepthFadeStart, _WCECF_DepthFadeCoef, _WCECF_IntensityMainLit * aten, _WCECF_IntensityAddLit * aten, _WCECF_ShadowIntensity, _WCECF_ColorShift, _WCECF_LitSaturation, _WCECF_NormalAtten, _WCECF_NormalAttenRate, _WCECF_TransparentBack, _WCECF_BacksideShadow);

    c *= 1 - (1 - BaseColor) * _WCECF_MultiplyIntensity;
    return c;
}


half3 WCECF_SyncCommon(float3 WorldPos, half3 NormalWS, float2 ScreenUV, half3 BaseColor) {
    #if defined(_WCE_DISABLED)
        return half3(0, 0, 0);
    #elif defined(SHADERGRAPH_PREVIEW)
        return WCECF_syncCore(WorldPos, NormalWS, ScreenUV, BaseColor, 1);
    #else
        float atten = 1;
        const float ATTEN_TH = 0.001;
        // Distance Fade
        float3 viewDir = WorldPos - _WorldSpaceCameraPos; // ←ifに入れるとVR SinglePassでエラー
        [branch] if (_WCECF_DistanceFadeCoef > 0.0001) {
            atten *= smoothstep(0, 1, 1 - (length(viewDir) - _WCECF_DistanceFadeStart) * _WCECF_DistanceFadeCoef);
        }
        // Volume & Image Mask
        [branch] if (atten > ATTEN_TH && (_WCECF_ClipOutside != 0 || _WCECF_UseImageMask != 0)) {
            float3 posES = mul(_WCECF_WorldToObjMatrix, float4(WorldPos, 1)).xyz;
            [branch] if (atten > ATTEN_TH && _WCECF_ClipOutside != 0 && (abs(posES.x) > 0.5 || abs(posES.y) > 0.5 || abs(posES.z) > 0.5)) {
                atten = 0;
            }
            [branch] if (atten > ATTEN_TH && _WCECF_UseImageMask != 0) {
                atten *= WCE_TEX_SAMPLE_RAW(_WCECF_ImageMaskTex, posES.xz + 0.5).r;
            }
        }
        [branch] if (atten > ATTEN_TH) {
            return WCECF_syncCore(WorldPos, NormalWS, ScreenUV, BaseColor, atten);
        } else {
            return half3(0, 0, 0);
        }
    #endif
}

// for ShaderGraph
void WCECF_SyncForShaderGraph_float(float3 WorldPos, half3 NormalWS, float2 ScreenUV, half3 BaseColor, out half3 EmissionColor) {
    EmissionColor = WCECF_SyncCommon(WorldPos, NormalWS, ScreenUV, BaseColor);
}

#endif

