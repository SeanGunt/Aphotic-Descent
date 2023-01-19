// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

Shader "Hidden/WaterCausticsModules/DustParticle" {
    Properties {
        [NoScaleOffset]_ParticleTex ("ParticleTex", 2D) = "white" { }
        _Intensity ("Caustics Intensity", Range(0, 5)) = 1
        _BaseColor ("BaseColor", Color) = (0, 0, 0, 1)
        [ToggleOff(_RECEIVE_SHADOWS_OFF)] _RECEIVE_SHADOWS_OFF ("Receive Shadow", Float) = 1
    }
    SubShader {
        LOD 0
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" "PreviewType" = "Plane" "IgnoreProjector" = "True" }

        Pass {
            Name "WCE_DEMO_DustParticle"
            Tags { "LightMode" = "WCE_DEMO_UnderWaterEffect" }

            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend One One
            
            HLSLPROGRAM
            #define _SURFACE_TYPE_TRANSPARENT 1

            #include "../../../../ShaderFunctions/ForHLSL/WaterCausticsEmissionFunc_HLSL.hlsl"
            #pragma multi_compile_fragment _ _WCE_DISABLED
            

            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #pragma exclude_renderers d3d11_9x

            #if VERSION_GREATER_EQUAL(11, 0)
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #else
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS_CASCADE
            #endif
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #if UNITY_VERSION >= 202120 // Unity2021.2.0 URP12.0
                #pragma multi_compile_fragment _ _LIGHT_LAYERS
                #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #endif
            #if UNITY_VERSION >= 202220 // Unity2022.2.0 URP14.0
                #pragma multi_compile_fragment _ _FORWARD_PLUS
            #endif

            #pragma multi_compile_fog
            #if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
                #define _USE_FOG
            #endif
            #pragma multi_compile_instancing
            #pragma multi_compile_local_fragment _ _RECEIVE_SHADOWS_OFF

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f {
                float4 posCS : SV_POSITION;
                float3 posWS : TEXCOORD0;
                half2 uv : TEXCOORD1;
                float2 screenUV : TEXCOORD2;
                half4 color : COLOR;
                #if defined(_USE_FOG)
                    float viewDepth : TEXCOORD3;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half _Intensity;
            CBUFFER_END
            
            // --------- Vertex Shader
            v2f vert(appdata v) {
                v2f o = (v2f)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float4 posCS = TransformObjectToHClip(v.vertex.xyz);
                float3 posWS = TransformObjectToWorld(v.vertex.xyz);
                float4 posSS = ComputeScreenPos(posCS);
                o.posCS = posCS;
                o.posWS = posWS;
                o.screenUV = posSS.xy / posSS.w;
                o.uv = v.uv;
                o.color = v.color;
                #if defined(_USE_FOG)
                    o.viewDepth = -TransformWorldToView(TransformObjectToWorld(v.vertex.xyz)).z;
                #endif
                return o;
            }

            // --------- Fog (Oblique Projection Supported)
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

                half calcFog(float viewDepth) {
                    float nearToFarZ = max(viewDepth - _ProjectionParams.y, 0);
                    return ComputeFogIntensity(WCE_computeFogFactorZ0ToFar(nearToFarZ));
                }
            #endif
            
            // --------- Fragment Shader
            sampler2D _ParticleTex;
            half4 frag(v2f i) : COLOR {
                half texR = tex2D(_ParticleTex, i.uv).r;
                texR *= step(i.posWS.y, _WCECF_SurfaceY);
                half atten = texR * i.screenUV.y * _Intensity;
                #if defined(_USE_FOG)
                    atten *= calcFog(i.viewDepth);
                #endif
                half3 caustics = WCE_EffectCore(i.posWS, half3(0, 1, 0), i.screenUV, WCE_TEX_PARAMS_RAW(_WCECF_CausticsTex), _WCECF_TexRotateSinCos, _WCECF_TexChannels, _WCECF_TilingSeed, _WCECF_TilingRot, _WCECF_TilingHard, _WCECF_Density, _WCECF_SurfaceY, _WCECF_SurfFadeStart, _WCECF_SurfFadeCoef, _WCECF_DepthFadeStart, _WCECF_DepthFadeCoef, _WCECF_IntensityMainLit * atten, _WCECF_IntensityAddLit * atten, _WCECF_ShadowIntensity, _WCECF_ColorShift, _WCECF_LitSaturation, 0, 1, 0, 1);
                half4 c;
                c.rgb = (caustics + _BaseColor.rgb * texR) * i.color.rgb;
                c.a = 1;
                return c;
            }

            ENDHLSL
        }
    }

    Fallback Off
}
