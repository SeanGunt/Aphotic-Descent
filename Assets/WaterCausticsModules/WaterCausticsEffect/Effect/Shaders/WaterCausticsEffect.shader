// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

Shader "Hidden/WaterCausticsModules/Effect" {
    Properties {
        // --- Scope
        _WCE_ClipOutside ("Clip Outside Volume", Int) = 1
        _WCE_UseImageMask ("Use Image Mask", Int) = 0
        [NoScaleOffset]_WCE_ImageMaskTex ("Texture", 2D) = "white" { }
        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Int) = 2
        // --- Texture
        [NoScaleOffset]_WCE_CausticsTex ("Caustics Texture", 2D) = "black" { }
        _WCE_TexChannels ("Channel", Vector) = (0, 1, 2, 0)
        _WCE_TexRotateSinCos ("Rotation Sin and Cos", Vector) = (0, 1, 0, 0)
        _WCE_TilingSeed ("Tiling Seed", Int) = -1
        _WCE_TilingRot ("Tiling Rot", Float) = 0.02
        _WCE_TilingHard ("Tiling Hard", Float) = 0.85
        // --- Dimension
        _WCE_Density ("Density", Float) = 0.2
        _WCE_SurfaceY ("Water Surface Y", Float) = 2
        _WCE_SurfFadeCoef ("Surface Fade Coef", Float) = 2
        _WCE_SurfFadeStart ("Surface Fade Start", Float) = 0
        _WCE_DepthFadeStart ("Depth Fade Start", Float) = 0
        _WCE_DepthFadeCoef ("Depth Fade Coef", Float) = 0.01
        _WCE_DistanceFadeStart ("Distance Fade Start", Float) = 0
        _WCE_DistanceFadeCoef ("Distance Fade Coef", Float) = 0.01
        // --- Effect
        _WCE_IntensityMainLit ("Main Light Intensity", Range(0, 50)) = 1
        _WCE_IntensityAddLit ("Additional Lights Intensity", Range(0, 50)) = 1
        [ToggleOff(_RECEIVE_SHADOWS_OFF)] _RECEIVE_SHADOWS_OFF ("Receive Shadow", Float) = 1
        _WCE_ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 1
        _WCE_ColorShift ("ColorShift", Vector) = (0.004, -0.001, 0, 0)
        _WCE_LitSaturation ("Light Saturation", Range(0, 2)) = 0.2
        _WCE_MultiplyByTex ("Multiply Color", Range(0, 1)) = 1
        // --- Normal Atten
        _WCE_NormalAtten ("Normal Atten Intensity", Range(0, 1)) = 1
        _WCE_NormalAttenRate ("Normal Atten Rate", Range(1, 8)) = 2
        _WCE_TransparentBack ("Transparent Backside", Range(0, 1)) = 0
        _WCE_BacksideShadow ("Backside Shadow", Range(0, 1)) = 0
        // --- Depth Buffer
        _ZWrite ("ZWrite", Int) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Int) = 4
        _OffsetFactor ("Offset Factor", float) = 0
        _OffsetUnits ("Offset Units", float) = 0
        // --- Stencil Buffer
        _StencilRef ("Ref [0-255]", Range(0, 255)) = 0
        _StencilReadMask ("Read Mask [0-255]", Range(0, 255)) = 255
        _StencilWriteMask ("Write Mask [0-255]", Range(0, 255)) = 255
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Comp", Int) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilPass ("Pass", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("ZFail", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("ZFail", Int) = 0
        // --- Blend
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("SrcFactor", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("DstFactor", Int) = 1
        // ※ Properties は SRPBatcher に必須

    }

    SubShader {
        LOD 0
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" "DisableBatching" = "True" "IgnoreProjector" = "True" }

        Pass {
            Name "WCE_EffectPass"
            Tags { "LightMode" = "WCE_EffectPass" }

            Blend [_BlendSrcFactor] [_BlendDstFactor]
            ZWrite [_ZWrite]
            ZTest [_ZTest]
            Offset [_OffsetFactor], [_OffsetUnits]
            Cull [_CullMode]
            Stencil {
                Ref [_StencilRef]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilPass]
                Fail [_StencilFail]
                ZFail [_StencilZFail]
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma exclude_renderers d3d11_9x
            #pragma target 3.5

            #define WCE_EFFECT_SHADER
            #define REQUIRE_OPAQUE_TEXTURE

            #pragma multi_compile_local _ _WCE_ONE_PASS_NORMAL _WCE_ONE_PASS_DEPTH
            #pragma multi_compile_local_fragment _ WCE_DEBUG_NORMAL WCE_DEBUG_DEPTH WCE_DEBUG_FACING WCE_DEBUG_CAUSTICS WCE_DEBUG_AREA
            #if defined(_WCE_ONE_PASS_NORMAL) || defined(_WCE_ONE_PASS_DEPTH)
                #define REQUIRE_DEPTH_TEXTURE
                #if defined(_WCE_ONE_PASS_NORMAL)
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
                #endif
            #else
                #define WCE_EACH_MESH
            #endif

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #if VERSION_GREATER_EQUAL(11, 0)
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #else
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS_CASCADE
            #endif
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHTS

            // ---- ※ URP14のDeferredで影が現れない問題応急処置
            // ※ TODO URP14のバージョンが上がったら修正されたか要確認
            #if UNITY_VERSION >= 202220 // Unity2022.2.0 URP14.0 以上
                #if !defined(_RECEIVE_SHADOWS_OFF)
                    #define _ADDITIONAL_LIGHT_SHADOWS
                #endif
            #else
                #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #endif
            // ----
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            
            // ---- 使用しない
            // #pragma multi_compile_fragment _ LIGHTMAP_ON
            // #pragma multi_compile_fragment _ LIGHTMAP_SHADOW_MIXING
            // #pragma multi_compile_fragment _ SHADOWS_SHADOWMASK
            // #pragma multi_compile_fragment _ _MIXED_LIGHTING_SUBTRACTIVE
            // ----

            #if UNITY_VERSION >= 202120 // Unity2021.2.0 URP12.0 以上
                #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
                #pragma multi_compile_fragment _ _LIGHT_LAYERS
                #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #endif
            #if UNITY_VERSION >= 202220 // Unity2022.2.0 URP14.0 以上
                #pragma multi_compile_fragment _ _FORWARD_PLUS
            #endif

            #pragma multi_compile_fog
            #if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
                #define _USE_FOG
            #endif

            #include "WaterCausticsEffectCommon.hlsl"
            #pragma multi_compile_local_fragment _ _RECEIVE_SHADOWS_OFF

            struct appdata {
                float4 vertex : POSITION;
                #if defined(WCE_EACH_MESH)
                    float3 normal : NORMAL;
                #else
                    uint vID : SV_VertexID;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 posClip : SV_POSITION;
                float4 posScrn : TEXCOORD0;
                #if defined(WCE_EACH_MESH)
                    float3 posWld : TEXCOORD1;
                    float3 posEffect : TEXCOORD2;
                    float3 normalWS : TEXCOORD3;
                    #if defined(_USE_FOG)
                        float viewDepth : TEXCOORD4;
                    #endif
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                float _WCE_Density;
                int3 _WCE_TexChannels;
                float2 _WCE_TexRotateSinCos;
                int _WCE_TilingSeed;
                float _WCE_TilingRot;
                float _WCE_TilingHard;
                float _WCE_SurfaceY;
                float _WCE_SurfFadeStart;
                float _WCE_SurfFadeCoef;
                float _WCE_DepthFadeStart;
                float _WCE_DepthFadeCoef;
                float _WCE_DistanceFadeStart;
                float _WCE_DistanceFadeCoef;
                half _WCE_IntensityMainLit;
                half _WCE_IntensityAddLit;
                float2 _WCE_ColorShift;
                half _WCE_LitSaturation;
                half _WCE_MultiplyByTex;
                half _WCE_NormalAtten;
                half _WCE_NormalAttenRate;
                half _WCE_TransparentBack;
                half _WCE_BacksideShadow;
                half _WCE_ShadowIntensity;
                int _WCE_ClipOutside;
                int _WCE_UseImageMask;
            CBUFFER_END

            #if defined(WCE_EACH_MESH)
                CBUFFER_START(FrequentlyUpdateVariables)
                    float4x4 _WCE_WorldToObjMatrix;
                CBUFFER_END
            #endif


            // ------------------------------------------------------------------------ Fill Clipped Hole
            bool WCE_intersectPlaneAndLine(float3 ptA, float3 ptB, float3 planeP, float3 planeN, bool isAllowEndOnPlane, out float3 PT) {
                float dotPA = dot(ptA - planeP, planeN);
                float dotPB = dot(ptB - planeP, planeN);
                bool isCross = (sign(dotPA) != sign(dotPB));
                bool isPtOnPlane = (dotPA == 0 || dotPB == 0);
                [branch] if (isCross && (isAllowEndOnPlane || !isPtOnPlane)) {
                    float3 AB = ptB - ptA;
                    float dif = abs(dotPA) + abs(dotPB);
                    float rate = abs(dotPA) / dif;
                    PT = ptA + AB * rate;
                    return true;
                } else {
                    PT = float3(0, 0, 0);
                    return false;
                }
            }

            float3 WCE_camDirWS() {
                return -UNITY_MATRIX_V[2].xyz;
            }

            float3 WCE_viewDirWS(float3 posWS) {
                return (unity_OrthoParams.w == 0) ? normalize(posWS - _WorldSpaceCameraPos) : WCE_camDirWS();
            }
            float3 WCE_viewDirRawWS(float3 posWS) {
                return (unity_OrthoParams.w == 0) ? posWS - _WorldSpaceCameraPos : WCE_camDirWS();
            }

            float4 WCE_fillClippedHole(uint vID) {
                const float3 pts[8] = {
                    float3(-0.5, -0.5, -0.5), float3(0.5, -0.5, -0.5), float3(-0.5, 0.5, -0.5), float3(0.5, 0.5, -0.5),
                    float3(-0.5, -0.5, 0.5), float3(0.5, -0.5, 0.5), float3(-0.5, 0.5, 0.5), float3(0.5, 0.5, 0.5),
                };
                const uint2 idxs [12] = {
                    uint2(0, 4), uint2(1, 5), uint2(2, 6), uint2(3, 7), uint2(0, 2), uint2(1, 3), uint2(4, 6), uint2(5, 7), uint2(0, 1), uint2(4, 5), uint2(2, 3), uint2(6, 7),
                };
                // ObliqueMatrix対応のためClipSpaceの端点からNearClipPlaneを得る
                float near = UNITY_NEAR_CLIP_VALUE; // ← D3D11/Metal/Vulkan/Switch:1, GLCore/GLES:-1
                float3 p0 = ComputeWorldSpacePosition(float4(0, 0, near, 1), UNITY_MATRIX_I_VP);
                float3 p1 = ComputeWorldSpacePosition(float4(1, 0, near, 1), UNITY_MATRIX_I_VP);
                float3 p2 = ComputeWorldSpacePosition(float4(0, 1, near, 1), UNITY_MATRIX_I_VP);
                float3 planeN_WS = cross(p1 - p0, p2 - p0);
                planeN_WS *= sign(dot(planeN_WS, -WCE_camDirWS()));
                
                float3 planeN = TransformWorldToObjectNormal(planeN_WS, true);
                float3 planeP = TransformWorldToObject(p0);
                float3 rightV = TransformWorldToObjectDir(p1 - p0, true);
                
                uint cnt = 0u;
                float3 center = float3(0, 0, 0);
                float4 intersectPts[6];
                [unroll(12)] for (uint i = 0u; i < 12u; i++) {
                    float3 ptA = pts [idxs [i].x];
                    float3 ptB = pts [idxs [i].y];
                    float3 PT;
                    bool isIntersect = WCE_intersectPlaneAndLine(ptA, ptB, planeP, planeN, (i < 4), PT);
                    [branch] if (isIntersect) {
                        intersectPts [cnt].xyz = PT;
                        center += PT;
                        cnt++;
                    }
                }
                center /= cnt;

                float3 outputPt = center;
                [branch] if (cnt >= 3u && vID <= cnt) {
                    [unroll(6)] for (uint k = 0u; k < cnt; k++) {
                        // 時計回りにwを設定 -2～+2
                        float3 v = normalize(intersectPts [k].xyz - center);
                        intersectPts [k].w = - (dot(v, rightV) - 1.0) * sign(dot(cross(v, rightV), planeN));
                    }
                    // ソート
                    [unroll(5)] for (uint m = 0u; m < cnt - 1u; m++) {
                        [unroll(5)] for (uint o = m + 1u; o < cnt; o++) {
                            [branch] if (intersectPts[m].w < intersectPts[o].w) {
                                float4 swap = intersectPts[m];
                                intersectPts[m] = intersectPts[o];
                                intersectPts[o] = swap;
                            }
                        }
                    }
                    outputPt = intersectPts[vID % cnt].xyz;
                }
                float4 posClip = TransformObjectToHClip(outputPt);
                posClip.z = UNITY_NEAR_CLIP_VALUE * posClip.w;
                return posClip;
            }

            // ------------------------------------------------------------------------ Vertex Shader
            v2f vert(appdata v) {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                #if defined(WCE_EACH_MESH)
                    // [Each Mesh]
                    float4 posClip = TransformObjectToHClip(v.vertex.xyz);
                    float3 posWld = TransformObjectToWorld(v.vertex.xyz);
                    float4 posScrn = ComputeScreenPos(posClip);
                    o.posClip = posClip;
                    o.posWld = posWld;
                    o.posScrn = posScrn;
                    o.posEffect = mul(_WCE_WorldToObjMatrix, float4(posWld, 1)).xyz;
                    o.normalWS = TransformObjectToWorldNormal(v.normal, true);
                    #if defined(_USE_FOG)
                        o.viewDepth = -TransformWorldToView(posWld).z;
                    #endif
                #else
                    // [At Once]
                    float4 posClip;
                    [branch]if (v.vID < 8u) {
                        posClip = TransformObjectToHClip(v.vertex.xyz);
                    } else {
                        posClip = WCE_fillClippedHole(v.vID - 8u);
                    }
                    float4 posScrn = ComputeScreenPos(posClip);
                    o.posClip = posClip;
                    o.posScrn = posScrn;
                #endif
                return o;
            }

            

            // ------------------------------------------------------------------------ Reconstruct World Pos and Normal
            float WCE_fixReversedZ(float Depth) {
                #if UNITY_REVERSED_Z
                    return 1 - Depth;
                #else
                    return Depth;
                #endif
            }

            float3 WCE_reconstructPosWS(float2 screenUV, float rawDepth) {
                #if !UNITY_REVERSED_Z
                    rawDepth = lerp(UNITY_NEAR_CLIP_VALUE, 1, rawDepth);
                #endif
                return ComputeWorldSpacePosition(screenUV, rawDepth, UNITY_MATRIX_I_VP);
            }
            
            bool WCE_checkOutside(float3 posES) {
                return (abs(posES.x) > 0.5 || abs(posES.y) > 0.5 || abs(posES.z) > 0.5);
            }

            #if !defined(WCE_EACH_MESH)
                #define RECONSTRUCT_NORMAL_HQ 1
                half3 WCE_reconstructNormalWS(float2 screenUV, float3 posWS, float rdC) {
                    #if (!RECONSTRUCT_NORMAL_HQ)
                        return normalize(cross(ddy(posWS), ddx(posWS)));
                    #else
                        float2 offsetU = float2(_ScreenParams.z - 1, 0);
                        float2 offsetV = float2(0, _ScreenParams.w - 1);
                        float2 uvN = screenUV + offsetV;
                        float2 uvS = screenUV - offsetV;
                        float2 uvE = screenUV + offsetU;
                        float2 uvW = screenUV - offsetU;
                        float rdN = SampleSceneDepth(uvN);
                        float rdS = SampleSceneDepth(uvS);
                        float rdE = SampleSceneDepth(uvE);
                        float rdW = SampleSceneDepth(uvW);
                        float3 vV = abs(rdN - rdC) < abs(rdS - rdC) ? WCE_reconstructPosWS(uvN, rdN) - posWS : posWS - WCE_reconstructPosWS(uvS, rdS);
                        float3 vU = abs(rdE - rdC) < abs(rdW - rdC) ? WCE_reconstructPosWS(uvE, rdE) - posWS : posWS - WCE_reconstructPosWS(uvW, rdW);
                        return normalize(cross(vV, vU));
                    #endif
                }
            #endif

            half3 WCE_getNormal(v2f IN, float2 screenUV, float3 posWS, float rawDepth) {
                #if defined(WCE_EACH_MESH)
                    float3 normal = IN.normalWS; // VFACEセマンティクスでの向きの取得はメッシュのデータに不具合があると正しくないことがあるので廃止
                    normal *= -sign(dot(normal, WCE_viewDirRawWS(posWS)));
                    return normal;
                #elif defined(_WCE_ONE_PASS_NORMAL)
                    float3 normal = SampleSceneNormals(screenUV);
                    #if VERSION_LOWER(10, 9) || (VERSION_GREATER_EQUAL(11, 0) && VERSION_LOWER(12, 0))
                        // ViewSpaceで保存されているURPのバージョン対応
                        normal.z *= -1;
                        normal = mul((float3x3)UNITY_MATRIX_I_V, normal).xyz;
                        normal = normalize(normal);
                    #endif
                    // normal *= -sign(dot(normal, WCE_viewDirWS(posWS))+0.07 );
                    normal *= -sign(dot(normal, WCE_viewDirRawWS(posWS)));
                    return normal;
                #else
                    return WCE_reconstructNormalWS(screenUV, posWS, rawDepth);
                #endif
            }

            // ------------------------------------------------------------------------ Fog (Oblique Projection Supported)
            #if defined(_USE_FOG)
                float WCE_computeFogFactorZ0ToFar(float z) {
                    #if defined(FOG_LINEAR)
                        return saturate(z * unity_FogParams.z + unity_FogParams.w);
                    #elif defined(FOG_EXP) || defined(FOG_EXP2)
                        return unity_FogParams.x * z;
                    #else
                        return 0;
                    #endif
                }

                float WCE_calcFog(float viewDepth) {
                    float nearToFarZ = max(viewDepth - _ProjectionParams.y, 0);
                    return ComputeFogIntensity(WCE_computeFogFactorZ0ToFar(nearToFarZ));
                }
            #endif

            // ------------------------------------------------------------------------
            #if defined(WCE_EACH_MESH)
                // [Each Mesh]
                #define WCE_clipOutside (_WCE_ClipOutside == 1)
            #else
                // [At Once]
                #define WCE_clipOutside true
            #endif


            // ------------------------------------------------------------------------ Fragment
            #define CLR_COL half4(0, 0, 0, 0)

            TEXTURE2D(_WCE_CausticsTex);
            TEXTURE2D(_WCE_ImageMaskTex);
            SAMPLER(sampler_WCE_CausticsTex);
            SAMPLER(sampler_WCE_ImageMaskTex);

            half4 frag(v2f IN) : SV_Target {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                // ---------- WorldPos
                float2 screenUV = IN.posScrn.xy / IN.posScrn.w;
                #if defined(WCE_EACH_MESH)
                    // [Each Mesh]
                    float3 posWS = IN.posWld;
                    float3 posES = IN.posEffect;
                    float rawDepth = 0.5;
                #else
                    // [At Once]
                    float rawDepth = SampleSceneDepth(screenUV);
                    float3 posWS = WCE_reconstructPosWS(screenUV, rawDepth);
                    float3 posES = TransformWorldToObject(posWS);
                #endif

                // ---------- Debug Info
                #if defined(WCE_DEBUG_NORMAL) || defined(WCE_DEBUG_DEPTH) || defined(WCE_DEBUG_FACING) || defined(WCE_DEBUG_CAUSTICS) || defined(WCE_DEBUG_AREA)
                    if ((WCE_clipOutside && WCE_checkOutside(posES)) || rawDepth == UNITY_RAW_FAR_CLIP_VALUE) discard;
                #endif
                #if defined(WCE_DEBUG_NORMAL)
                    return half4(pow(saturate(WCE_getNormal(IN, screenUV, posWS, rawDepth) * 0.5 + 0.5), 4) * 0.9, 1);
                #elif defined(WCE_DEBUG_DEPTH)
                    float debugDepth = -TransformWorldToView(posWS).z * _ProjectionParams.w;
                    return half4(pow(abs(debugDepth), 0.7).xxx, 1);
                #elif defined(WCE_DEBUG_FACING)
                    float3 n = WCE_getNormal(IN, screenUV, posWS, rawDepth);
                    float b = dot(-n, WCE_viewDirWS(posWS));
                    return half4((saturate(pow(abs(b), 2) * 0.5)).xxx, 1);
                #elif defined(WCE_DEBUG_CAUSTICS)
                    _WCE_MultiplyByTex = 0;
                #elif defined(WCE_DEBUG_AREA)
                    _WCE_MultiplyByTex = 0;
                #endif

                // ---------- Clip Outside
                [branch] if ((WCE_clipOutside && WCE_checkOutside(posES)) || rawDepth == UNITY_RAW_FAR_CLIP_VALUE) return CLR_COL;

                // ---------- Atten Start
                float atten = 1; // halfだとVRシングルパスで不安定?
                const float ATTEN_TH = 0.001;

                // ---------- Fog
                #if defined(_USE_FOG)
                    #if defined(WCE_EACH_MESH)
                        float viewDepth = IN.viewDepth;
                    #else
                        float viewDepth = -TransformWorldToView(posWS).z;
                    #endif
                    atten *= WCE_calcFog(viewDepth);
                #endif

                // ---------- Distance Fade
                float3 viewDir = posWS - _WorldSpaceCameraPos; // ←ifに入れるとVR SinglePassでエラー
                [branch] if (atten > ATTEN_TH && _WCE_DistanceFadeCoef > 0.0001f) {
                    atten *= smoothstep(0, 1, 1 - (length(viewDir) - _WCE_DistanceFadeStart) * _WCE_DistanceFadeCoef);
                }

                // ---------- Image Mask
                [branch] if (atten > ATTEN_TH && _WCE_UseImageMask != 0) {
                    atten *= _WCE_ImageMaskTex.Sample(sampler_WCE_ImageMaskTex, posES.xz + 0.5).r;
                }

                // ---------- Atten End
                [branch] if (atten <= ATTEN_TH) return CLR_COL;
                _WCE_IntensityMainLit *= atten;
                _WCE_IntensityAddLit *= atten;

                // ---------- Normal
                half3 normalWS = WCE_getNormal(IN, screenUV, posWS, rawDepth);

                // ---------- Caustics
                half3 c = WCE_EffectCore(posWS, normalWS, screenUV, _WCE_CausticsTex, sampler_WCE_CausticsTex, _WCE_TexRotateSinCos, _WCE_TexChannels, _WCE_TilingSeed, _WCE_TilingRot, _WCE_TilingHard, _WCE_Density, _WCE_SurfaceY, _WCE_SurfFadeStart, _WCE_SurfFadeCoef, _WCE_DepthFadeStart, _WCE_DepthFadeCoef, _WCE_IntensityMainLit, _WCE_IntensityAddLit, _WCE_ShadowIntensity, _WCE_ColorShift, _WCE_LitSaturation, _WCE_NormalAtten, _WCE_NormalAttenRate, _WCE_TransparentBack, _WCE_BacksideShadow);
                
                // ---------- Multiply Opaque Tex
                [branch] if (_WCE_MultiplyByTex > 0) {
                    c *= 1 - (1 - SHADERGRAPH_SAMPLE_SCENE_COLOR(screenUV)) * _WCE_MultiplyByTex;
                }
                return half4(c, 1);
            }

            ENDHLSL
        }
    }

    Fallback "Hidden/InternalErrorShader"
}
