// Made with Amplify Shader Editor v1.9.0.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/WaterCausticsModules/DEMO_GodRayEffect"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector]_Intensity("Intensity", Range( 0 , 3)) = 0.09
		[ASEEnd]_SurfaceAttenCoef("_SurfaceAttenCoef", Float) = 0
		[HideInInspector]_DepthCoef("DepthCoef", Float) = -2
		[HideInInspector]_DepthFade("DepthFade", Range( 0.001 , 5)) = 0.5
		[HideInInspector]_SightDepthFadeStart("SightDepthFadeStart", Float) = 1
		[HideInInspector]_SightDepthFadeRange("SightDepthFadeRange", Float) = 2.5
		[HideInInspector][ToggleOff(_RECEIVE_SHADOWS_OFF)] _RECEIVE_SHADOWS_OFF("RECEIVE_SHADOWS", Float) = 1


		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Overlay" }

		Cull Off
		AlphaToMask Off

		

		HLSLINCLUDE

		#pragma target 4.5

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS

		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="WCE_DEMO_UnderWaterEffect" }
			
			Blend One One, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define ASE_SRP_VERSION 100900
			#define REQUIRE_DEPTH_TEXTURE 1


			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma shader_feature _RECEIVE_SHADOWS_OFF
			#pragma multi_compile_fragment __ _WCE_DISABLED


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
					float fogFactor : TEXCOORD2;
				#endif
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float _Intensity;
			float _DepthFade;
			float _DepthCoef;
			float _SightDepthFadeRange;
			float _SightDepthFadeStart;
			float _SurfaceAttenCoef;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			sampler2D _WCECF_CausticsTex;
			float _WCECF_SurfaceY;
			float _WCECF_Density;
			float2 _WCECF_TexRotateSinCos;
			float2 _WCECF_ColorShift;
			float3 _WCECF_TexChannels;
			float _WCECF_IntensityMainLit;
			uniform float4 _CameraDepthTexture_TexelSize;
			float _WCECF_SurfFadeStart;


			void PragmaMultiCompile( float In0 )
			{
				#if VERSION_GREATER_EQUAL(11, 0)
					#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
				#else
					#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS
					#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS_CASCADE
				#endif
				#pragma multi_compile_fog
				#pragma multi_compile_fragment _ _LIGHT_LAYERS
				#pragma multi_compile_fragment _ _LIGHT_COOKIES
				#pragma multi_compile_fragment _ _FORWARD_PLUS
			}
			
			float2 RotateV2182( float2 v2, float2 RotSinCos )
			{
				float2x2 rot = float2x2(RotSinCos.y, -RotSinCos.x, RotSinCos.x, RotSinCos.y);
				return mul(rot, v2);
			}
			
			float2 RotateV2183( float2 v2, float2 RotSinCos )
			{
				float2x2 rot = float2x2(RotSinCos.y, -RotSinCos.x, RotSinCos.x, RotSinCos.y);
				return mul(rot, v2);
			}
			
			float3 TexSamplingWithShift204( sampler2D Tex, float2 uvR, float2 uvG, float2 uvB, float3 Channels )
			{
				return float3(tex2D(Tex,uvR)[Channels[0]], tex2D(Tex,uvG)[Channels[1]], tex2D(Tex,uvB)[Channels[2]]);
			}
			
			float FogFactor200( float positionCS_Z )
			{
				#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
					return ComputeFogFactor(positionCS_Z);
				#else
					return 1;
				#endif
			}
			
			float ComputeFogIntensity202( float FogFactor )
			{
				#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
					return ComputeFogIntensity(FogFactor);
				#else
					return 1;
				#endif
			}
			

			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float4 ase_clipPos = TransformObjectToHClip((v.vertex).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord3 = screenPos;
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				float3 objectToViewPos = TransformWorldToView(TransformObjectToWorld(v.vertex.xyz));
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord4.w = eyeDepth;
				float positionCS_Z200 = screenPos.z;
				float localFogFactor200 = FogFactor200( positionCS_Z200 );
				float vertexToFrag196 = localFogFactor200;
				o.ase_texcoord5.x = vertexToFrag196;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord5.yzw = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif

				float3 vertexValue = defaultVertexValue;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				#ifdef ASE_FOG
					o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif

				o.clipPos = positionCS;

				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					float3 WorldPosition = IN.worldPos;
				#endif

				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				sampler2D Tex204 = _WCECF_CausticsTex;
				float SurfaceY137 = _WCECF_SurfaceY;
				float dotResult13 = dot( _MainLightPosition.xyz , float3(0,1,0) );
				float2 appendResult33 = (float2(WorldPosition.x , WorldPosition.z));
				float2 v2182 = ( ( ( ( SurfaceY137 - WorldPosition.y ) * ( (_MainLightPosition.xyz).xz / max( dotResult13 , 0.001 ) ) ) + appendResult33 ) * _WCECF_Density );
				float2 RotSinCos184 = _WCECF_TexRotateSinCos;
				float2 RotSinCos182 = RotSinCos184;
				float2 localRotateV2182 = RotateV2182( v2182 , RotSinCos182 );
				float2 v2183 = _WCECF_ColorShift;
				float2 RotSinCos183 = RotSinCos184;
				float2 localRotateV2183 = RotateV2183( v2183 , RotSinCos183 );
				float2 uvR204 = ( localRotateV2182 - localRotateV2183 );
				float2 uvG204 = localRotateV2182;
				float2 uvB204 = ( localRotateV2182 + localRotateV2183 );
				float3 Channels204 = _WCECF_TexChannels;
				float3 localTexSamplingWithShift204 = TexSamplingWithShift204( Tex204 , uvR204 , uvG204 , uvB204 , Channels204 );
				float localLightData211 = ( 0.0 );
				float3 WorldPos211 = WorldPosition;
				float3 LitColor211 = float3( 0,0,0 );
				float ShadowAtten211 = 0;
				{
				#if (defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE) || defined(_MAIN_LIGHT_SHADOWS_SCREEN)) && !defined(_RECEIVE_SHADOWS_OFF)
				    #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)
				        float4 clipPos = TransformWorldToHClip(WorldPos211);
				        float4 shadowCoord = ComputeScreenPos(clipPos);
				    #else
				        float4 shadowCoord = TransformWorldToShadowCoord(WorldPos211);
				    #endif
				    Light mainLight = GetMainLight (shadowCoord);
				#else
				    Light mainLight = GetMainLight ();
				#endif
				LitColor211 = mainLight.color;
				ShadowAtten211 = mainLight.shadowAttenuation;
				}
				float3 LitColor216 = LitColor211;
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth67 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth67 = abs( ( screenDepth67 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _DepthFade ) );
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 switchResult108 = (((ase_vface>0)?(ase_worldNormal):(-ase_worldNormal)));
				float3 NormalWS110 = switchResult108;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float dotResult189 = dot( NormalWS110 , ase_worldViewDir );
				float eyeDepth = IN.ase_texcoord4.w;
				float cameraDepthFade95 = (( eyeDepth -_ProjectionParams.y - _SightDepthFadeStart ) / _SightDepthFadeRange);
				float ShadowAtten213 = ShadowAtten211;
				float vertexToFrag196 = IN.ase_texcoord5.x;
				float FogFactor202 = vertexToFrag196;
				float localComputeFogIntensity202 = ComputeFogIntensity202( FogFactor202 );
				#ifdef _WCE_DISABLED
				float3 staticSwitch209 = float3(0,0,0);
				#else
				float3 staticSwitch209 = ( localTexSamplingWithShift204 * LitColor216 * ( _Intensity * _WCECF_IntensityMainLit * saturate( distanceDepth67 ) * saturate( (0.0 + (dotResult189 - 0.3) * (1.0 - 0.0) / (0.6 - 0.3)) ) * saturate( ( 1.0 - ( abs( ( SurfaceY137 - WorldPosition.y ) ) * _DepthCoef ) ) ) * saturate( cameraDepthFade95 ) * ShadowAtten213 * saturate( ( ( ( SurfaceY137 + _WCECF_SurfFadeStart ) - WorldPosition.y ) * _SurfaceAttenCoef ) ) * localComputeFogIntensity202 ) );
				#endif
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = staticSwitch209;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}
			ENDHLSL
		}

	
	}
	
	
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19002
-1530.4;388;1536;785.4;4802.276;1622.263;4.160055;True;False
Node;AmplifyShaderEditor.CommentaryNode;6;-3338.429,-1134.245;Inherit;False;1899.009;855.7812;CausticsUV;18;23;182;26;186;13;137;11;22;131;33;17;14;12;16;10;15;31;9;CausticsUV;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;114;-3089.798,491.6486;Inherit;False;903.0642;231.3819;Comment;4;107;109;108;110;Normal World Space;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;12;-3273.956,-828.8858;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;11;-3229.768,-674.6952;Inherit;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;13;-2970.021,-693.8109;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;107;-3039.798,542.4305;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;131;-3042.271,-1083.832;Inherit;False;Global;_WCECF_SurfaceY;_WCECF_SurfaceY;0;0;Create;True;0;0;0;False;0;False;0;3.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;137;-2773.253,-1084.196;Inherit;False;SurfaceY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;9;-2736.874,-992.359;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMaxOpNode;14;-2741.625,-695.5835;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.001;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;15;-2753.921,-836.5353;Inherit;False;FLOAT2;0;2;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;146;-1632.051,1149.758;Inherit;False;1076.708;335.3246;Comment;8;89;121;188;125;176;172;173;175;DepthFade;1,1,1,1;0;0
Node;AmplifyShaderEditor.NegateNode;109;-2805.746,608.1073;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;22;-2482.604,-1017.031;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;187;-2606.991,-153.4301;Inherit;False;594.4221;211;Comment;2;180;184;TextureRotation;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;89;-1574.923,1311.72;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;16;-2476.669,-835.4747;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;17;-2509.849,-689.7358;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;172;-1600.657,1222.461;Inherit;False;137;SurfaceY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;108;-2645.794,541.6486;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;144;-1696.879,2343.867;Inherit;False;1153.567;376.2704;Comment;8;168;166;142;133;169;170;171;143;WaterSurfaceAtten;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;168;-1632.436,2398.828;Inherit;False;137;SurfaceY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;197;-1519.195,2808.646;Inherit;False;971.5968;229.5724;Comment;4;200;196;202;199;Fog;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-2411.533,545.8616;Inherit;False;NormalWS;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;166;-1668.952,2488.014;Inherit;False;Global;_WCECF_SurfFadeStart;_WCECF_SurfFadeStart;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;173;-1377.985,1238.799;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2295.787,-653.0907;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;215;-3011.894,931.0656;Inherit;False;761.4219;229;Comment;4;212;211;213;216;LightData;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;147;-1633.118,714.5161;Inherit;False;1091.899;319.8731;Comment;7;189;111;190;127;194;126;94;DirectionAtten;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;180;-2556.991,-99.58142;Inherit;False;Global;_WCECF_TexRotateSinCos;_WCECF_TexRotateSinCos;8;0;Create;True;0;0;0;False;0;False;0,0;0.258819,0.9659258;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-2302.189,-941.9452;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;199;-1476.006,2858.208;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;190;-1580.764,858.5984;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;121;-1362.446,1382.267;Inherit;False;Property;_DepthCoef;DepthCoef;2;1;[HideInInspector];Create;True;0;0;0;False;0;False;-2;0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;175;-1213.071,1237.684;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;159;-1950.061,-149.9527;Inherit;False;532.4854;294.1536;Comment;3;179;185;183;ColorShift;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-2140.58,-783.4621;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;145;-1313.113,1575.62;Inherit;False;748.8712;278.0231;Comment;4;96;97;95;98;SightDepthFade;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-1578.042,761.121;Inherit;False;110;NormalWS;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;142;-1394.186,2563.343;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;23;-2118.605,-477.3661;Inherit;False;Global;_WCECF_Density;_WCECF_Density;0;0;Create;True;0;0;0;False;0;False;4;0.2222222;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;170;-1353.997,2467.279;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;212;-2961.894,981.0656;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-2240.569,-100.225;Inherit;False;RotSinCos;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;148;-1499.653,406.6987;Inherit;False;945.2531;234.8125;Comment;3;81;67;73;SoftParticle;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-1871.095,37.97807;Inherit;False;184;RotSinCos;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;169;-1133.762,2470.949;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;179;-1911.317,-88.03909;Inherit;False;Global;_WCECF_ColorShift;_WCECF_ColorShift;10;0;Create;True;0;0;0;False;0;False;0,0;0.005362311,0.004499513;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;188;-1054.674,1231.199;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-1200.685,848.9449;Inherit;False;Constant;_AngleRangeMin;AngleRangeMin;2;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1258.314,1738.243;Inherit;False;Property;_SightDepthFadeStart;SightDepthFadeStart;4;1;[HideInInspector];Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;189;-1350.861,765.3046;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;186;-1879.366,-400.3867;Inherit;False;184;RotSinCos;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1263.113,1648.043;Inherit;False;Property;_SightDepthFadeRange;SightDepthFadeRange;5;1;[HideInInspector];Create;True;0;0;0;False;0;False;2.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;211;-2774.506,982.0336;Inherit;False;#if (defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE) || defined(_MAIN_LIGHT_SHADOWS_SCREEN)) && !defined(_RECEIVE_SHADOWS_OFF)$    #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)$        float4 clipPos = TransformWorldToHClip(WorldPos)@$        float4 shadowCoord = ComputeScreenPos(clipPos)@$    #else$        float4 shadowCoord = TransformWorldToShadowCoord(WorldPos)@$    #endif$    Light mainLight = GetMainLight (shadowCoord)@$#else$    Light mainLight = GetMainLight ()@$#endif$LitColor = mainLight.color@$ShadowAtten = mainLight.shadowAttenuation@$;7;Create;3;True;WorldPos;FLOAT3;0,0,0;In;;Inherit;False;True;LitColor;FLOAT3;0,0,0;Out;;Inherit;False;True;ShadowAtten;FLOAT;0;Out;;Inherit;False;LightData;True;False;0;;False;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT3;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;127;-1198.465,936.9351;Inherit;False;Constant;_AngleRangeMax;AngleRangeMax;3;0;Create;True;0;0;0;False;0;False;0.6;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1852.281,-512.2893;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CustomExpressionNode;200;-1274.772,2934.329;Inherit;False;#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)$	return ComputeFogFactor(positionCS_Z)@$#else$	return 1@$#endif;1;Create;1;True;positionCS_Z;FLOAT;0;In;;Inherit;False;FogFactor;True;False;0;;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;133;-1191.276,2626.206;Inherit;False;Property;_SurfaceAttenCoef;_SurfaceAttenCoef;1;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1381.092,529.2355;Inherit;False;Property;_DepthFade;DepthFade;3;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.5;1;0.001;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexToFragmentNode;196;-1036.301,2934.017;Inherit;False;False;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;67;-1063.746,509.8884;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;99;-888.8705,1971.281;Inherit;False;300.779;220.5209;Shadow;1;214;Shadow;1,1,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;194;-941.4257,761.0665;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;95;-992.1929,1625.62;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;183;-1634.437,-81.31442;Inherit;False;float2x2 rot = float2x2(RotSinCos.y, -RotSinCos.x, RotSinCos.x, RotSinCos.y)@$return mul(rot, v2)@;2;Create;2;True;v2;FLOAT2;0,0;In;;Inherit;False;True;RotSinCos;FLOAT2;0,0;In;;Inherit;False;RotateV2;True;False;0;;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-874.1378,2470.956;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;213;-2464.595,1062.672;Inherit;False;ShadowAtten;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;182;-1677.761,-511.0421;Inherit;False;float2x2 rot = float2x2(RotSinCos.y, -RotSinCos.x, RotSinCos.x, RotSinCos.y)@$return mul(rot, v2)@;2;Create;2;True;v2;FLOAT2;0,0;In;;Inherit;False;True;RotSinCos;FLOAT2;0,0;In;;Inherit;False;RotateV2;True;False;0;;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;176;-890.3177,1225.054;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-770.7977,290.787;Inherit;False;Global;_WCECF_IntensityMainLit;_WCECF_IntensityMainLit;7;0;Create;True;0;0;0;False;0;False;1;1.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;202;-765.7265,2933.639;Inherit;False;#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)$	return ComputeFogIntensity(FogFactor)@$#else$	return 1@$#endif;1;Create;1;True;FogFactor;FLOAT;0;In;;Inherit;False;ComputeFogIntensity;True;False;0;;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;73;-719.1998,509.4;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;216;-2462.639,984.6966;Inherit;False;LitColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;143;-699.9424,2473.761;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;206;-1185.87,-724.4023;Inherit;True;Global;_WCECF_CausticsTex;_WCECF_CausticsTex;7;0;Create;True;0;0;0;False;0;False;None;748dc78fc131a8d43884fc0521487795;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;98;-725.5027,1626.578;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;94;-706.0175,764.5161;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;125;-736.0682,1225.251;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;157;-1142.676,-513.9413;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;203;-1131.996,22.10153;Inherit;False;Global;_WCECF_TexChannels;_WCECF_TexChannels;7;0;Create;True;0;0;0;False;0;False;0,1,2;0,1,2;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;214;-801.6852,2067.06;Inherit;False;213;ShadowAtten;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-1101.141,-108.0777;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-794.1233,184.3506;Inherit;False;Property;_Intensity;Intensity;0;1;[HideInInspector];Create;True;0;0;0;False;0;False;0.09;0.06;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;217;-185.0503,285.1038;Inherit;False;216;LitColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;204;-792.6343,-337.3347;Inherit;False;return float3(tex2D(Tex,uvR)[Channels[0]], tex2D(Tex,uvG)[Channels[1]], tex2D(Tex,uvB)[Channels[2]])@;3;Create;5;True;Tex;SAMPLER2D;;In;;Inherit;False;True;uvR;FLOAT2;0,0;In;;Inherit;False;True;uvG;FLOAT2;0,0;In;;Inherit;False;True;uvB;FLOAT2;0,0;In;;Inherit;False;True;Channels;FLOAT3;0,1,2;In;;Inherit;False;TexSamplingWithShift;True;False;0;;False;5;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT3;0,1,2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-135.5519,385.7728;Inherit;False;9;9;0;FLOAT;1;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;95.90932,270.4717;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;1,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;219;414.6158,-95.36697;Inherit;False;474.4234;252.5571;Comment;1;218;Pragma Multi Compile;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;210;108.4609,463.1104;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;113;414.6869,-422.0385;Inherit;False;466.1409;246.73;Comment;1;106;Shadow Switch;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;209;319.1416,267.4238;Inherit;False;Property;_WCE_DISABLED;_WCE_DISABLED;7;0;Create;True;0;0;0;True;0;False;1;0;0;False;;Toggle;2;Key0;Key1;Create;False;True;Fragment;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;106;535.6977,-355.22;Inherit;False;Property;_RECEIVE_SHADOWS_OFF;RECEIVE_SHADOWS;6;0;Create;False;0;0;0;True;1;HideInInspector;False;0;1;1;True;_RECEIVE_SHADOWS_OFF;ToggleOff;2;Key0;Key1;Create;False;False;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;218;574.2249,-11.11419;Inherit;False;#if VERSION_GREATER_EQUAL(11, 0)$	#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN$#else$	#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS$	#pragma multi_compile_fragment _ _MAIN_LIGHT_SHADOWS_CASCADE$#endif$#pragma multi_compile_fog$#pragma multi_compile_fragment _ _LIGHT_LAYERS$#pragma multi_compile_fragment _ _LIGHT_COOKIES$#pragma multi_compile_fragment _ _FORWARD_PLUS;7;Create;1;True;In0;FLOAT;0;In;;Inherit;False;Pragma MultiCompile;False;True;0;;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;77;280.0168,452.6936;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;74;400.4763,452.6936;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;76;280.0168,452.6936;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;75;592.5094,275.8414;Half;False;True;-1;2;;0;5;Hidden/WaterCausticsModules/DEMO_GodRayEffect;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Overlay=Queue=0;True;5;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;;1;False;;1;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=WCE_DEMO_UnderWaterEffect;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;0;  Blend;2;0;Two Sided;0;0;Cast Shadows;0;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;0;0;LOD CrossFade;0;0;Built-in Fog;0;637981606238589453;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;False;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;78;280.0168,452.6936;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;137;0;131;0
WireConnection;14;0;13;0
WireConnection;15;0;12;0
WireConnection;109;0;107;0
WireConnection;22;0;137;0
WireConnection;22;1;9;2
WireConnection;16;0;15;0
WireConnection;16;1;14;0
WireConnection;108;0;107;0
WireConnection;108;1;109;0
WireConnection;110;0;108;0
WireConnection;173;0;172;0
WireConnection;173;1;89;2
WireConnection;33;0;17;1
WireConnection;33;1;17;3
WireConnection;10;0;22;0
WireConnection;10;1;16;0
WireConnection;175;0;173;0
WireConnection;31;0;10;0
WireConnection;31;1;33;0
WireConnection;170;0;168;0
WireConnection;170;1;166;0
WireConnection;184;0;180;0
WireConnection;169;0;170;0
WireConnection;169;1;142;2
WireConnection;188;0;175;0
WireConnection;188;1;121;0
WireConnection;189;0;111;0
WireConnection;189;1;190;0
WireConnection;211;1;212;0
WireConnection;26;0;31;0
WireConnection;26;1;23;0
WireConnection;200;0;199;3
WireConnection;196;0;200;0
WireConnection;67;0;81;0
WireConnection;194;0;189;0
WireConnection;194;1;126;0
WireConnection;194;2;127;0
WireConnection;95;0;96;0
WireConnection;95;1;97;0
WireConnection;183;0;179;0
WireConnection;183;1;185;0
WireConnection;171;0;169;0
WireConnection;171;1;133;0
WireConnection;213;0;211;4
WireConnection;182;0;26;0
WireConnection;182;1;186;0
WireConnection;176;0;188;0
WireConnection;202;0;196;0
WireConnection;73;0;67;0
WireConnection;216;0;211;3
WireConnection;143;0;171;0
WireConnection;98;0;95;0
WireConnection;94;0;194;0
WireConnection;125;0;176;0
WireConnection;157;0;182;0
WireConnection;157;1;183;0
WireConnection;158;0;182;0
WireConnection;158;1;183;0
WireConnection;204;0;206;0
WireConnection;204;1;157;0
WireConnection;204;2;182;0
WireConnection;204;3;158;0
WireConnection;204;4;203;0
WireConnection;208;0;62;0
WireConnection;208;1;207;0
WireConnection;208;2;73;0
WireConnection;208;3;94;0
WireConnection;208;4;125;0
WireConnection;208;5;98;0
WireConnection;208;6;214;0
WireConnection;208;7;143;0
WireConnection;208;8;202;0
WireConnection;72;0;204;0
WireConnection;72;1;217;0
WireConnection;72;2;208;0
WireConnection;209;1;72;0
WireConnection;209;0;210;0
WireConnection;75;2;209;0
ASEEND*/
//CHKSM=EF19CDD41BAA33349EC026EBB7105D10406802D2