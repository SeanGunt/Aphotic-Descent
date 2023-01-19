// Made with Amplify Shader Editor v1.9.0.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/WaterCausticsModules/DEMO_WaterSurface"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_WaveDensity("WaveDensity", Range( 0 , 5)) = 2
		_WaveHeight("WaveHeight", Range( 0 , 0.05)) = 0.015
		_WaveFluctuation("WaveFluctuation", Range( 0 , 5)) = 1
		_WaveMoveX("WaveMoveX", Range( -3 , 3)) = 0
		_WaveMoveZ("WaveMoveZ", Range( -3 , 3)) = 0
		[Header(Reflection)]_ReflectionIOR("ReflectionIOR", Range( 0 , 1)) = 0.75
		_ReflectionPow("ReflectionPow", Range( 1 , 5)) = 1
		_ReflectionIntensity("ReflectionIntensity", Range( 0 , 2)) = 1
		[Header(Refraction)]_RefractEta("RefractEta", Range( 0.5 , 1)) = 0.9
		_RefractIntensity("RefractIntensity", Range( 0 , 50)) = 1
		_RefractionDepth("RefractionDepth", Range( 0 , 100)) = 0
		[Header (WaterColor)][Toggle(_USE_WATER_COLOR_ON)] _USE_WATER_COLOR("USE_WATER_COLOR", Float) = 0
		_WaterColorSurface("WaterColorSurface", Color) = (1,1,1,1)
		_WaterColorMid("WaterColorMid", Color) = (1,1,1,1)
		_WaterColorDeep("WaterColorDeep", Color) = (0,0.4181452,0.425,1)
		_WaterColorDepthA("WaterColorDepthA", Range( 0 , 1)) = 0
		_WaterColorDepthB("WaterColorDepthB", Range( 0 , 20)) = 0
		[Header(Specular)]_Specular("Specular", Range( 0.8 , 1)) = 0.989
		_SpecularPow("SpecularPow", Range( 1 , 500)) = 0.989
		_SpecularIntensity("SpecularIntensity", Range( 0 , 1000)) = 1
		[Header(UnderWater)]_RefractionRate("RefractionRate", Range( -0.1 , 1)) = 0.5
		_RefractIntensityBackside("RefractIntensity", Range( 0 , 50)) = 1
		[ASEEnd][Header(FadeEdge)]_FadeEdgeDepth("FadeEdgeDepth", Range( 0 , 2)) = 0


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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Transparent" }

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
			Tags { "LightMode"="UniversalForwardOnly" }
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _RECEIVE_SHADOWS_OFF 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_SRP_VERSION 100900
			#define REQUIRE_OPAQUE_TEXTURE 1
			#define REQUIRE_DEPTH_TEXTURE 1
			#pragma multi_compile_fragment _ _FORWARD_PLUS
			#define ASE_USING_SAMPLING_MACROS 1


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
			#pragma shader_feature_local _USE_WATER_COLOR_ON


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
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _WaterColorDeep;
			float4 _WaterColorMid;
			float4 _WaterColorSurface;
			float _WaveMoveX;
			float _ReflectionIntensity;
			float _ReflectionPow;
			float _WaterColorDepthA;
			float _WaterColorDepthB;
			float _ReflectionIOR;
			float _SpecularIntensity;
			float _SpecularPow;
			float _Specular;
			float _RefractionDepth;
			float _RefractIntensityBackside;
			float _RefractIntensity;
			float _RefractEta;
			float _WaveHeight;
			float _WaveDensity;
			float _WaveFluctuation;
			float _WaveMoveZ;
			float _RefractionRate;
			float _FadeEdgeDepth;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			float _DemoTime;
			uniform float4 _CameraDepthTexture_TexelSize;


			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			
			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
			float3 ReflectionProbeCompatibleForwardPlus377( float3 ViewDirWS, float3 NormalWS, float3 WorldPos, float2 ScreenUV )
			{
				half3 reflectV = reflect(-ViewDirWS, NormalWS);
				#if UNITY_VERSION >= 202220
				    return GlossyEnvironmentReflection(reflectV, WorldPos, 0, 1, ScreenUV);
				#else 
				    return GlossyEnvironmentReflection(reflectV, 0, 1);
				#endif
			}
			
			float3 ReflectionProbeCompatibleForwardPlus382( float3 ViewDirWS, float3 NormalWS, float3 WorldPos, float2 ScreenUV )
			{
				half3 reflectV = reflect(-ViewDirWS, NormalWS);
				#if UNITY_VERSION >= 202220
				    return GlossyEnvironmentReflection(reflectV, WorldPos, 0, 1, ScreenUV);
				#else 
				    return GlossyEnvironmentReflection(reflectV, 0, 1);
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

				float4 screenPos = IN.ase_texcoord3;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float2 appendResult103 = (float2(ase_grabScreenPosNorm.xy));
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 break18 = WorldPosition;
				float2 appendResult181 = (float2(_WaveMoveX , _WaveMoveZ));
				float2 break178 = ( appendResult181 * ( _DemoTime * -1.0 ) );
				float3 appendResult245 = (float3(( break18.x + break178.x ) , ( break18.y + ( _DemoTime * _WaveFluctuation ) ) , ( break18.z + break178.y )));
				float3 break250 = appendResult245;
				float3 appendResult252 = (float3(break250.x , break250.y , ( break250.z + 0.01 )));
				float simplePerlin3D254 = snoise( appendResult252*_WaveDensity );
				simplePerlin3D254 = simplePerlin3D254*0.5 + 0.5;
				float3 appendResult249 = (float3(break250.x , break250.y , break250.z));
				float simplePerlin3D244 = snoise( appendResult249*_WaveDensity );
				simplePerlin3D244 = simplePerlin3D244*0.5 + 0.5;
				float3 appendResult258 = (float3(0.0 , ( ( simplePerlin3D254 - simplePerlin3D244 ) * _WaveHeight ) , 0.01));
				float3 appendResult251 = (float3(( break250.x + 0.01 ) , break250.y , break250.z));
				float simplePerlin3D253 = snoise( appendResult251*_WaveDensity );
				simplePerlin3D253 = simplePerlin3D253*0.5 + 0.5;
				float3 appendResult256 = (float3(0.01 , ( ( simplePerlin3D253 - simplePerlin3D244 ) * _WaveHeight ) , 0.0));
				float3 normalizeResult261 = normalize( cross( appendResult258 , appendResult256 ) );
				float3 switchResult362 = (((ase_vface>0)?(normalizeResult261):(-normalizeResult261)));
				float3 WorldNormal92 = switchResult362;
				float3 worldToClip357 = TransformWorldToHClip(( WorldPosition + refract( -ase_worldViewDir , WorldNormal92 , _RefractEta ) )).xyz;
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 switchResult364 = (((ase_vface>0)?(ase_worldNormal):(-ase_worldNormal)));
				float3 worldToClip358 = TransformWorldToHClip(( WorldPosition + refract( -ase_worldViewDir , switchResult364 , _RefractEta ) )).xyz;
				float switchResult309 = (((ase_vface>0)?(_RefractIntensity):(_RefractIntensityBackside)));
				float dotResult134 = dot( ( WorldPosition - _WorldSpaceCameraPos ) , ase_worldViewDir );
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float eyeDepth152 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float eyeDepth = IN.ase_texcoord4.w;
				float WaterDepth331 = ( eyeDepth152 - eyeDepth );
				float2 temp_output_124_0 = ( appendResult103 + ( ( ( ( (worldToClip357).xy - (worldToClip358).xy ) * switchResult309 ) / dotResult134 ) * saturate( ( WaterDepth331 / _RefractionDepth ) ) ) );
				float4 fetchOpaqueVal338 = float4( SHADERGRAPH_SAMPLE_SCENE_COLOR( temp_output_124_0 ), 1.0 );
				float3 appendResult340 = (float3(fetchOpaqueVal338.rgb));
				float3 DeformedScreenColor326 = appendResult340;
				float localLightInfo186 = ( 0.0 );
				float3 LightDir186 = float3( 0,1,-0.4 );
				float3 LightColor186 = float3( 1,1,1 );
				{
				#if defined (SHADERGRAPH_PREVIEW)
				LightDir186 = normalize (half3 (0, 1, -0.4));
				LightColor186 = half3 (1, 1, 1);
				#else
				Light lit = GetMainLight ();
				LightDir186 = lit.direction;
				LightColor186 = lit.color *  lit.distanceAttenuation;
				#endif
				}
				float dotResult187 = dot( LightDir186 , reflect( -ase_worldViewDir , WorldNormal92 ) );
				float saferPower202 = abs( saturate( (0.0 + (dotResult187 - _Specular) * (1.0 - 0.0) / (1.0 - _Specular)) ) );
				float fresnelNdotV27 = dot( WorldNormal92, ase_worldViewDir );
				float ior27 = _ReflectionIOR;
				ior27 = pow( max( ( 1 - ior27 ) / ( 1 + ior27 ) , 0.0001 ), 2 );
				float fresnelNode27 = ( ior27 + ( 1.0 - ior27 ) * pow( max( 1.0 - fresnelNdotV27 , 0.0001 ), 5 ) );
				float Fresnel206 = fresnelNode27;
				float3 appendResult347 = (float3(_WaterColorSurface.rgb));
				float3 appendResult345 = (float3(_WaterColorMid.rgb));
				float3 appendResult349 = (float3(_WaterColorDeep.rgb));
				float eyeDepth305 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( float4( temp_output_124_0, 0.0 , 0.0 ).xy ),_ZBufferParams);
				float temp_output_307_0 = ( eyeDepth305 - eyeDepth );
				float3 lerpResult86 = lerp( appendResult345 , appendResult349 , (saturate( ( temp_output_307_0 / _WaterColorDepthB ) )).xxx);
				float3 lerpResult165 = lerp( appendResult347 , lerpResult86 , (saturate( ( temp_output_307_0 / _WaterColorDepthA ) )).xxx);
				float3 appendResult278 = (float3(lerpResult165));
				#ifdef _USE_WATER_COLOR_ON
				float3 staticSwitch308 = ( DeformedScreenColor326 * appendResult278 );
				#else
				float3 staticSwitch308 = DeformedScreenColor326;
				#endif
				float3 ViewDirWS377 = ase_worldViewDir;
				float3 NormalWS377 = WorldNormal92;
				float3 WorldPos377 = WorldPosition;
				float2 ScreenUV377 = ase_screenPosNorm.xy;
				float3 localReflectionProbeCompatibleForwardPlus377 = ReflectionProbeCompatibleForwardPlus377( ViewDirWS377 , NormalWS377 , WorldPos377 , ScreenUV377 );
				float3 saferPower80 = abs( localReflectionProbeCompatibleForwardPlus377 );
				float3 temp_cast_7 = (_ReflectionPow).xxx;
				float3 lerpResult34 = lerp( staticSwitch308 , pow( saferPower80 , temp_cast_7 ) , (saturate( ( Fresnel206 * _ReflectionIntensity ) )).xxxx.xyz);
				float3 ViewDirWS382 = ase_worldViewDir;
				float3 NormalWS382 = WorldNormal92;
				float3 WorldPos382 = WorldPosition;
				float2 ScreenUV382 = ase_screenPosNorm.xy;
				float3 localReflectionProbeCompatibleForwardPlus382 = ReflectionProbeCompatibleForwardPlus382( ViewDirWS382 , NormalWS382 , WorldPos382 , ScreenUV382 );
				float dotResult284 = dot( ase_worldViewDir , WorldNormal92 );
				float3 lerpResult294 = lerp( DeformedScreenColor326 , localReflectionProbeCompatibleForwardPlus382 , (step( dotResult284 , _RefractionRate )).xxx);
				float3 switchResult274 = (((ase_vface>0)?(( ( ( pow( saferPower202 , _SpecularPow ) * _SpecularIntensity * Fresnel206 ) * LightColor186 ) + lerpResult34 )):(lerpResult294)));
				float3 lerpResult335 = lerp( DeformedScreenColor326 , switchResult274 , (saturate( ( WaterDepth331 / _FadeEdgeDepth ) )).xxx);
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = lerpResult335;
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

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _RECEIVE_SHADOWS_OFF 1
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define ASE_SRP_VERSION 100900
			#pragma multi_compile_fragment _ _FORWARD_PLUS
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			

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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _WaterColorDeep;
			float4 _WaterColorMid;
			float4 _WaterColorSurface;
			float _WaveMoveX;
			float _ReflectionIntensity;
			float _ReflectionPow;
			float _WaterColorDepthA;
			float _WaterColorDepthB;
			float _ReflectionIOR;
			float _SpecularIntensity;
			float _SpecularPow;
			float _Specular;
			float _RefractionDepth;
			float _RefractIntensityBackside;
			float _RefractIntensity;
			float _RefractEta;
			float _WaveHeight;
			float _WaveDensity;
			float _WaveFluctuation;
			float _WaveMoveZ;
			float _RefractionRate;
			float _FadeEdgeDepth;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END

			

			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				

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

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
					o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

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

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
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

				

				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	
	
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19002
-1530.4;349.6;1536;823.8;5228.781;2309.041;7.098215;True;False
Node;AmplifyShaderEditor.CommentaryNode;352;-5873.45,-494.7699;Inherit;False;689.1172;424.952;Comment;5;135;132;134;133;131;Distance;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;351;2847.47,-394.4361;Inherit;False;1061.384;415.5675;Comment;7;342;334;337;343;344;341;335;EdgeFade;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;208;-363.7049,-2219.987;Inherit;False;1945.245;667.5901;Comment;15;205;192;198;195;191;203;188;207;199;190;186;202;204;200;187;Specular;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;96;-3797.694,-1536;Inherit;False;3320.565;1470.37;Comment;28;278;308;87;88;89;103;124;12;305;166;102;307;306;169;155;161;170;168;146;165;167;86;326;338;340;345;347;349;WaterColor;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;140;-6176.928,-1380.063;Inherit;False;2245.969;748.1693;Comment;23;366;185;364;310;359;309;127;360;354;120;122;355;121;357;356;125;129;123;126;136;358;137;119;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;336;2311.877,-337.338;Inherit;False;260;183;Comment;1;274;SwitchUnderOrAboveWater;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;141;-5044.963,-476.6736;Inherit;False;683.1204;377.7405;Comment;4;332;163;164;162;Depth;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;302;777.8079,403.0861;Inherit;False;1158.353;1099.195;Comment;13;283;281;369;284;295;350;380;378;381;379;382;294;327;UnderWater;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;33;-4860.043,159.9137;Inherit;False;3796.66;840.7024;Comment;40;292;92;249;261;23;269;260;254;175;253;174;17;32;246;263;251;264;180;181;20;257;256;18;247;250;173;248;245;259;178;252;179;258;244;311;313;314;315;362;367;Wave;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;94;-336.3441,-1182.849;Inherit;False;1511.497;1090.268;Comment;17;377;376;375;374;373;268;147;206;304;27;303;77;142;80;79;61;34;Reflection;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;333;-5920.136,-1883.066;Inherit;False;687.5215;262.8624;Comment;4;157;152;154;331;WaterDepth;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;32;-4664.73,271.8584;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;307;-2741.694,-384;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CrossProductOpNode;260;-1905.824,497.34;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;181;-4423.26,649.3738;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;146;-2031.147,-310.1565;Inherit;False;FLOAT3;0;0;0;3;1;0;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SurfaceDepthNode;154;-5870.136,-1735.203;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;358;-4975.434,-1030.919;Inherit;False;World;Clip;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceCameraPos;132;-5827.525,-276.5855;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;-4287.192,649.4547;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;334;2989.743,-192.4527;Inherit;False;331;WaterDepth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;359;-4771.563,-1180.369;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;251;-3021.348,506.7631;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-963.4219,-981.2767;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScreenDepthNode;305;-2997.694,-448;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;175;-3954.282,474.9276;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;292;-1350.129,700.7944;Inherit;False;WorldNormalUnderWater;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;103;-3381.694,-1248;Inherit;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;169;-2165.694,-208;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-4418.623,-1102.927;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScreenColorNode;338;-2096.364,-1120.432;Inherit;False;Global;_GrabScreen0;Grab Screen 0;22;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;259;-2459.817,582.1339;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;313;-4295.578,451.8706;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-4694.058,472.4114;Inherit;False;Property;_WaveFluctuation;WaveFluctuation;2;0;Create;True;0;0;0;False;0;False;1;1.14;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;136;-4247.577,-1103.245;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;269;-3155.307,809.4902;Inherit;False;Property;_WaveDensity;WaveDensity;0;0;Create;True;0;0;0;False;0;False;2;1.76;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;674.0435,-1873.447;Inherit;False;Property;_SpecularIntensity;SpecularIntensity;19;0;Create;True;0;0;0;False;0;False;1;560;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;191;-313.7049,-1836.285;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;197;1844.692,-1125.04;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;-2302.99,429.0391;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-4100.752,-1106.435;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-267.7356,-1676.733;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;253;-2720.879,503.421;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;199;1349.746,-1949.549;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;247;-3701.322,718.7671;Inherit;False;Constant;_span;span;20;0;Create;True;0;0;0;False;0;False;0.01;0.01;0.001;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;245;-3701.911,352.6401;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;34;981.7499,-1110.796;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-4736.681,640.7473;Inherit;False;Property;_WaveMoveX;WaveMoveX;3;0;Create;True;0;0;0;False;0;False;0;-0.44;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RefractOpVec;127;-5506.94,-999.848;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;133;-5548.617,-270.4866;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;23;-2688.891,860.7823;Inherit;False;Property;_WaveHeight;WaveHeight;1;0;Create;True;0;0;0;False;0;False;0.015;0.0323;0;0.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;315;-4736.576,909.7705;Inherit;False;Constant;_Float1;Float 1;23;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;308;-741.694,-1120;Inherit;False;Property;_USE_WATER_COLOR;USE_WATER_COLOR;11;0;Create;True;0;0;0;False;1;Header (WaterColor);False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;331;-5460.614,-1833.066;Inherit;False;WaterDepth;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;168;-2016.426,-211.8429;Inherit;False;FLOAT3;0;0;0;3;1;0;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;163;-4644.029,-347.6164;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;362;-1466.716,503.4081;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;187;430.3903,-2169.987;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;341;3431.306,-341.6179;Inherit;False;326;DeformedScreenColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;200;214.0608,-2054.223;Inherit;False;Property;_Specular;Specular;17;1;[Header];Create;True;1;Specular;0;0;False;0;False;0.989;0.975;0.8;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;134;-5347.436,-374.5626;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;374;-235.7817,-988.7198;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;373;-239.5305,-840.7207;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;375;-236.9004,-760.8093;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;342;3346.909,-165.8845;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;135;-5541.642,-376.7393;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;283;900.351,1156.25;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;337;2897.658,-94.45045;Inherit;False;Property;_FadeEdgeDepth;FadeEdgeDepth;22;1;[Header];Create;True;1;FadeEdge;0;0;False;0;False;0;0;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;18;-4281.365,278.4439;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.NegateNode;192;-134.5859,-1830.27;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;349;-2019.608,-685.1003;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;254;-2715.291,651.025;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;248;-3201.952,689.5505;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;161;-2325.694,-304;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;249;-3032.252,353.1651;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;310;-5161.139,-754.8861;Inherit;False;Property;_RefractIntensityBackside;RefractIntensity;21;0;Create;False;0;0;0;False;0;False;1;10;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-5014.879,-244.5833;Inherit;False;Property;_RefractionDepth;RefractionDepth;10;0;Create;True;0;0;0;False;0;False;0;0.5;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;89;-2251.558,-902.465;Inherit;False;Property;_WaterColorMid;WaterColorMid;13;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;164;-4500.532,-346.6764;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;155;-2167.196,-305.5012;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;261;-1730.395,498.3175;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;327;891.5731,493.7731;Inherit;False;326;DeformedScreenColor;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TransformPositionNode;357;-4973.801,-1179.28;Inherit;False;World;Clip;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;202;909.1909,-2168.652;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;281;1001.1,1395.453;Inherit;False;Property;_RefractionRate;RefractionRate;20;1;[Header];Create;True;1;UnderWater;0;0;False;0;False;0.5;0.45;-0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-2885.694,-272;Inherit;False;Property;_WaterColorDepthB;WaterColorDepthB;16;0;Create;True;0;0;0;False;0;False;0;10;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;246;-3207.954,505.3959;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;-2303.149,584.7846;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;345;-2046.047,-897.5573;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-244.8809,-277.6341;Inherit;False;Property;_ReflectionIOR;ReflectionIOR;5;1;[Header];Create;True;1;Reflection;0;0;False;0;False;0.75;0.75;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-3955.931,379.9514;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;207;763.0476,-1770.59;Inherit;False;206;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;190;23.01464,-1830.56;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;256;-2123.861,401.883;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;203;597.0706,-2008.55;Inherit;False;Property;_SpecularPow;SpecularPow;18;0;Create;True;0;0;0;False;0;False;0.989;248;1;500;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;311;-4739.162,831.2217;Inherit;False;Global;_DemoTime;_DemoTime;22;0;Create;True;0;0;0;False;0;False;0;63.35966;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-5165.906,-839.7916;Inherit;False;Property;_RefractIntensity;RefractIntensity;9;0;Create;True;0;0;0;False;0;False;1;1.6;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;157;-5604.893,-1825.914;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;347;-1646.131,-903.0156;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;180;-4733.246,722.4629;Inherit;False;Property;_WaveMoveZ;WaveMoveZ;4;0;Create;True;0;0;0;False;0;False;0;-0.36;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;131;-5813.059,-440.3543;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;252;-3019.84,647.0261;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;119;-5884.04,-1087.957;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;88;-2242.403,-687.5909;Inherit;False;Property;_WaterColorDeep;WaterColorDeep;14;0;Create;True;0;0;0;False;0;False;0,0.4181452,0.425,1;0.9213836,0.9989005,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;166;-2885.694,-160;Inherit;False;Property;_WaterColorDepthA;WaterColorDepthA;15;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;354;-5388.587,-1295.727;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;174;-3952.47,279.0074;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RefractOpVec;121;-5508.829,-1150.842;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;1110.191,-2035.454;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;195;767.9851,-2168.161;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;278;-1184.722,-735.4883;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;355;-5123.719,-1175.089;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;178;-4143.864,645.084;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;165;-1360.478,-728.7465;Inherit;False;3;0;FLOAT3;1,1,1;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;257;-2453.467,423.2705;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;343;3202.98,-163.4039;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-5902.783,-984.5931;Inherit;False;Property;_RefractEta;RefractEta;8;1;[Header];Create;True;1;Refraction;0;0;False;0;False;0.9;0.95;0.5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;340;-1923.288,-1120.725;Inherit;False;FLOAT3;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;314;-4473.976,888.8704;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;335;3728.717,-306.5017;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;250;-3482.539,350.7131;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;129;-4598.618,-1104.391;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;356;-5119.247,-1025.353;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;186;-17.05066,-1972.948;Inherit;False;#if defined (SHADERGRAPH_PREVIEW)$	LightDir = normalize (half3 (0, 1, -0.4))@$	LightColor = half3 (1, 1, 1)@$#else$	Light lit = GetMainLight ()@$	LightDir = lit.direction@$	LightColor = lit.color *  lit.distanceAttenuation@$#endif;7;Create;2;True;LightDir;FLOAT3;0,1,-0.4;Out;;Inherit;False;True;LightColor;FLOAT3;1,1,1;Out;;Inherit;False;LightInfo;True;False;0;;False;3;0;FLOAT;0;False;1;FLOAT3;0,1,-0.4;False;2;FLOAT3;1,1,1;False;3;FLOAT;0;FLOAT3;2;FLOAT3;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;326;-1402.005,-1128.927;Inherit;False;DeformedScreenColor;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;360;-4780.332,-1030.835;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;185;-6119.856,-832.3671;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SwizzleNode;344;3509.438,-165.8845;Inherit;False;FLOAT3;0;1;2;3;1;0;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SurfaceDepthNode;306;-3061.694,-368;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;366;-5922.278,-765.0879;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;294;1702.731,498.8189;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;92;-1268.496,502.4427;Inherit;False;WorldNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;147;803.374,-388.3745;Inherit;False;FLOAT4;0;1;2;3;1;0;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;170;-2325.694,-208;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;157.5756,-762.7899;Inherit;False;Property;_ReflectionPow;ReflectionPow;6;0;Create;True;0;0;0;False;0;False;1;1.07;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;80;544.5636,-937.8839;Inherit;False;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;142;668.7524,-381.8226;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;332;-4921.844,-320.4767;Inherit;False;331;WaterDepth;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;12;-3685.694,-1424;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;244;-2726.862,350.4641;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;304;189.4403,-245.4794;Inherit;False;Property;_ReflectionIntensity;ReflectionIntensity;7;0;Create;True;0;0;0;False;0;False;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;274;2361.877,-287.338;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;124;-3173.694,-1136;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;122;-5710.903,-1240.286;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwitchByFaceNode;309;-4848.232,-831.9316;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;376;-239.7146,-612.7193;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;152;-5812.284,-1826.843;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;27;32.54157,-382.4248;Inherit;False;SchlickIOR;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0.75;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;513.5558,-384.067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;364;-5790.781,-833.4586;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;167;-1840.12,-908.7826;Inherit;False;Property;_WaterColorSurface;WaterColorSurface;12;0;Create;True;1;WaterColor;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;258;-2126.676,562.6691;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CustomExpressionNode;382;1235.268,714.4019;Inherit;False;half3 reflectV = reflect(-ViewDirWS, NormalWS)@$#if UNITY_VERSION >= 202220$    return GlossyEnvironmentReflection(reflectV, WorldPos, 0, 1, ScreenUV)@$#else $    return GlossyEnvironmentReflection(reflectV, 0, 1)@$#endif;3;Create;4;True;ViewDirWS;FLOAT3;0,0,1;In;;Inherit;False;True;NormalWS;FLOAT3;0,1,0;In;;Inherit;False;True;WorldPos;FLOAT3;0,0,0;In;;Inherit;False;True;ScreenUV;FLOAT2;0,0;In;;Inherit;False;ReflectionProbe Compatible ForwardPlus;True;False;0;;False;4;0;FLOAT3;0,0,1;False;1;FLOAT3;0,1,0;False;2;FLOAT3;0,0,0;False;3;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;379;890.9326,586.4165;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ScreenPosInputsNode;381;884.3997,945.5166;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;378;883.2838,731.8148;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;367;-1596.337,606.8599;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;198;569.1345,-2169.915;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.997;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;268;-213.399,-381.3776;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;86;-1600.478,-696.7465;Inherit;False;3;0;FLOAT3;1,1,1;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;380;888.5139,803.9266;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CustomExpressionNode;377;145.209,-936.0928;Inherit;False;half3 reflectV = reflect(-ViewDirWS, NormalWS)@$#if UNITY_VERSION >= 202220$    return GlossyEnvironmentReflection(reflectV, WorldPos, 0, 1, ScreenUV)@$#else $    return GlossyEnvironmentReflection(reflectV, 0, 1)@$#endif;3;Create;4;True;ViewDirWS;FLOAT3;0,0,1;In;;Inherit;False;True;NormalWS;FLOAT3;0,1,0;In;;Inherit;False;True;WorldPos;FLOAT3;0,0,0;In;;Inherit;False;True;ScreenUV;FLOAT2;0,0;In;;Inherit;False;ReflectionProbe Compatible ForwardPlus;True;False;0;;False;4;0;FLOAT3;0,0,1;False;1;FLOAT3;0,1,0;False;2;FLOAT3;0,0,0;False;3;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwizzleNode;350;1466.072,1229.739;Inherit;False;FLOAT3;0;0;0;3;1;0;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StepOpNode;295;1328.915,1228.437;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;284;1169.943,1225.754;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;369;894.8151,1310.485;Inherit;False;92;WorldNormal;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;206;298.2821,-386.7972;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;120;-5869.089,-1246.406;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;65;371.5105,-65.06307;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;63;371.5105,-65.06307;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;True;3;False;;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;61;964.7587,-975.7555;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;;0;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;64;371.5105,-65.06307;Float;False;False;-1;2;UnityEditor.ShaderGraphUnlitGUI;0;5;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;False;False;True;False;False;False;False;0;False;;False;False;False;False;False;False;False;False;False;True;1;False;;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;62;4151.431,-311.2085;Float;False;True;-1;2;;0;5;Hidden/WaterCausticsModules/DEMO_WaterSurface;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;2;False;;False;False;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Transparent=Queue=0;True;5;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;;0;False;;1;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;255;False;;255;False;;255;False;;7;False;;1;False;;1;False;;1;False;;7;False;;1;False;;1;False;;1;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForwardOnly;False;False;2;Pragma;multi_compile_fragment _ _FORWARD_PLUS;False;;Custom;Include;;False;;Native;Hidden/InternalErrorShader;0;0;Standard;22;Surface;0;0;  Blend;0;0;Two Sided;0;637995669477988101;Cast Shadows;0;637978437264238690;  Use Shadow Threshold;0;0;Receive Shadows;0;637978437272836858;GPU Instancing;0;638061995600205397;LOD CrossFade;0;0;Built-in Fog;1;637995690840785435;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,;0;  Type;0;0;  Tess;16,False,;0;  Min;10,False,;0;  Max;25,False,;0;  Edge Length;16,False,;0;  Max Displacement;25,False,;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;True;False;False;;True;0
WireConnection;307;0;305;0
WireConnection;307;1;306;0
WireConnection;260;0;258;0
WireConnection;260;1;256;0
WireConnection;181;0;179;0
WireConnection;181;1;180;0
WireConnection;146;0;155;0
WireConnection;358;0;356;0
WireConnection;173;0;181;0
WireConnection;173;1;314;0
WireConnection;359;0;357;0
WireConnection;251;0;246;0
WireConnection;251;1;250;1
WireConnection;251;2;250;2
WireConnection;87;0;326;0
WireConnection;87;1;278;0
WireConnection;305;0;124;0
WireConnection;175;0;18;2
WireConnection;175;1;178;1
WireConnection;292;0;367;0
WireConnection;103;0;12;0
WireConnection;169;0;170;0
WireConnection;125;0;129;0
WireConnection;125;1;309;0
WireConnection;338;0;124;0
WireConnection;259;0;254;0
WireConnection;259;1;244;0
WireConnection;313;0;311;0
WireConnection;313;1;17;0
WireConnection;136;0;125;0
WireConnection;136;1;134;0
WireConnection;197;0;199;0
WireConnection;197;1;34;0
WireConnection;263;0;257;0
WireConnection;263;1;23;0
WireConnection;137;0;136;0
WireConnection;137;1;164;0
WireConnection;253;0;251;0
WireConnection;253;1;269;0
WireConnection;199;0;205;0
WireConnection;199;1;186;3
WireConnection;245;0;174;0
WireConnection;245;1;20;0
WireConnection;245;2;175;0
WireConnection;34;0;308;0
WireConnection;34;1;80;0
WireConnection;34;2;147;0
WireConnection;127;0;122;0
WireConnection;127;1;364;0
WireConnection;127;2;123;0
WireConnection;308;1;326;0
WireConnection;308;0;87;0
WireConnection;331;0;157;0
WireConnection;168;0;169;0
WireConnection;163;0;332;0
WireConnection;163;1;162;0
WireConnection;362;0;261;0
WireConnection;362;1;367;0
WireConnection;187;0;186;2
WireConnection;187;1;190;0
WireConnection;134;0;135;0
WireConnection;134;1;133;0
WireConnection;342;0;343;0
WireConnection;135;0;131;0
WireConnection;135;1;132;0
WireConnection;18;0;32;0
WireConnection;192;0;191;0
WireConnection;349;0;88;0
WireConnection;254;0;252;0
WireConnection;254;1;269;0
WireConnection;248;0;250;2
WireConnection;248;1;247;0
WireConnection;161;0;307;0
WireConnection;161;1;102;0
WireConnection;249;0;250;0
WireConnection;249;1;250;1
WireConnection;249;2;250;2
WireConnection;164;0;163;0
WireConnection;155;0;161;0
WireConnection;261;0;260;0
WireConnection;357;0;355;0
WireConnection;202;0;195;0
WireConnection;202;1;203;0
WireConnection;246;0;250;0
WireConnection;246;1;247;0
WireConnection;264;0;259;0
WireConnection;264;1;23;0
WireConnection;345;0;89;0
WireConnection;20;0;18;1
WireConnection;20;1;313;0
WireConnection;190;0;192;0
WireConnection;190;1;188;0
WireConnection;256;0;247;0
WireConnection;256;1;263;0
WireConnection;157;0;152;0
WireConnection;157;1;154;0
WireConnection;347;0;167;0
WireConnection;252;0;250;0
WireConnection;252;1;250;1
WireConnection;252;2;248;0
WireConnection;174;0;18;0
WireConnection;174;1;178;0
WireConnection;121;0;122;0
WireConnection;121;1;119;0
WireConnection;121;2;123;0
WireConnection;205;0;202;0
WireConnection;205;1;204;0
WireConnection;205;2;207;0
WireConnection;195;0;198;0
WireConnection;278;0;165;0
WireConnection;355;0;354;0
WireConnection;355;1;121;0
WireConnection;178;0;173;0
WireConnection;165;0;347;0
WireConnection;165;1;86;0
WireConnection;165;2;168;0
WireConnection;257;0;253;0
WireConnection;257;1;244;0
WireConnection;343;0;334;0
WireConnection;343;1;337;0
WireConnection;340;0;338;0
WireConnection;314;0;311;0
WireConnection;314;1;315;0
WireConnection;335;0;341;0
WireConnection;335;1;274;0
WireConnection;335;2;344;0
WireConnection;250;0;245;0
WireConnection;129;0;359;0
WireConnection;129;1;360;0
WireConnection;356;0;354;0
WireConnection;356;1;127;0
WireConnection;326;0;340;0
WireConnection;360;0;358;0
WireConnection;344;0;342;0
WireConnection;366;0;185;0
WireConnection;294;0;327;0
WireConnection;294;1;382;0
WireConnection;294;2;350;0
WireConnection;92;0;362;0
WireConnection;147;0;142;0
WireConnection;170;0;307;0
WireConnection;170;1;166;0
WireConnection;80;0;377;0
WireConnection;80;1;79;0
WireConnection;142;0;303;0
WireConnection;244;0;249;0
WireConnection;244;1;269;0
WireConnection;274;0;197;0
WireConnection;274;1;294;0
WireConnection;124;0;103;0
WireConnection;124;1;137;0
WireConnection;122;0;120;0
WireConnection;309;0;126;0
WireConnection;309;1;310;0
WireConnection;27;0;268;0
WireConnection;27;2;77;0
WireConnection;303;0;206;0
WireConnection;303;1;304;0
WireConnection;364;0;185;0
WireConnection;364;1;366;0
WireConnection;258;1;264;0
WireConnection;258;2;247;0
WireConnection;382;0;379;0
WireConnection;382;1;378;0
WireConnection;382;2;380;0
WireConnection;382;3;381;0
WireConnection;367;0;261;0
WireConnection;198;0;187;0
WireConnection;198;1;200;0
WireConnection;86;0;345;0
WireConnection;86;1;349;0
WireConnection;86;2;146;0
WireConnection;377;0;374;0
WireConnection;377;1;373;0
WireConnection;377;2;375;0
WireConnection;377;3;376;0
WireConnection;350;0;295;0
WireConnection;295;0;284;0
WireConnection;295;1;281;0
WireConnection;284;0;283;0
WireConnection;284;1;369;0
WireConnection;206;0;27;0
WireConnection;62;2;335;0
ASEEND*/
//CHKSM=823DDE90127CA7F74CD2AA8F3B11B5877082DDF3