// WaterCausticsModules
// Copyright (c) 2021 Masataka Hakozaki

Shader "Hidden/WaterCausticsModules/UIRawImagePreview" {
    Properties {
        _MainTex ("Texture", 2D) = "Black" { }
    }
    SubShader {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always

        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 col;
                col.rgb = tex2D(_MainTex, i.uv).rgb;
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
