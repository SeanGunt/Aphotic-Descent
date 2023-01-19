// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

Shader "Hidden/WaterCausticsModules/TexFont" {
    Properties {
        [PerRendererData] _MainTex ("Font Texture", 2D) = "white" { }
    }
    SubShader {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }

        Pass {
            Name "WCE_DEMO_TexFont"
            Tags { "LightMode" = "WCE_DEMO_TexFont" }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #pragma target 3.5

            #pragma multi_compile_fog
            #if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
                #define _USE_FOG
            #endif

            struct vIn {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0; // ※Float精度必須
                half4 color : COLOR;
                #if defined(_USE_FOG)
                    float viewDepth : TEXCOORD4;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            // --------- Vertex Shader
            v2f vert(vIn v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = TransformObjectToHClip(v.vertex.xyz);
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
            sampler2D _MainTex;
            half4 frag(v2f i) : COLOR {
                half4 color = i.color;
                half alpha = tex2Dbias(_MainTex, float4(i.uv, 0, -1)).r;
                #if defined(_USE_FOG)
                    alpha *= calcFog(i.viewDepth);
                #endif
                color.a *= alpha;
                return color;
            }

            ENDHLSL
        }
    }
    
    Fallback Off
}
