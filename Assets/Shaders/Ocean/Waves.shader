// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Waves"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_Steepness("Steepness", Range( 0 , 1)) = 0
		_Wave3Dir("Wave3Dir", Vector) = (0,0,0,0)
		_Wave1Dir("Wave1Dir", Vector) = (0,0,0,0)
		_Wave0Dir("Wave0Dir", Vector) = (0,1,0,0)
		_Wave2Dir("Wave2Dir", Vector) = (0,0,0,0)
		_NumberOfWaves("NumberOfWaves", Float) = 1
		[HDR]_MainColor("Main Color", Color) = (0,0,0,0)
		_Amplitude("Amplitude", Float) = 1
		_FoamScale("FoamScale", Float) = 0
		_FoamMask("FoamMask", Float) = 0
		_FoamIntesnity("Foam Intesnity", Range( 0 , 1)) = 0
		[HDR][NoScaleOffset]_SpecularHDRi("SpecularHDRi", CUBE) = "white" {}
		_FresnelScale("FresnelScale", Float) = 1
		_SpecularIntensity("SpecularIntensity", Range( 0 , 1)) = 1
		_FresnelPower("FresnelPower", Float) = 5
		_FresnelBias("FresnelBias", Range( 0 , 1)) = 0.03529412
		[NoScaleOffset]_BigNormalMap("BigNormalMap", 2D) = "bump" {}
		[HDR]_CrestColor("Crest Color", Color) = (0,0,0,0)
		[NoScaleOffset]_SmallNormalMap("SmallNormalMap", 2D) = "bump" {}
		_BigNormalTiling("BigNormalTiling", Float) = 0
		_SmallNormalTiling("SmallNormalTiling", Float) = 0
		_Wavespeed("Wavespeed", Float) = 1
		_BigNormalIntensity("BigNormalIntensity", Float) = 0
		_SmallNormalIntensity("SmallNormalIntensity", Float) = 0
		_DepthFadeDistance("DepthFadeDistance", Float) = 0
		[HDR]_FoamColor("FoamColor", Color) = (1,1,1,0)
		_WaveLengthMultiplier("WaveLengthMultiplier", Range( 1 , 100)) = 0
		_WaveSpeedMultiplier("WaveSpeedMultiplier", Range( 1 , 100)) = 0
		_AmplitudeDivider("AmplitudeDivider", Range( 1 , 100)) = 0
		_FoamMaskFalloff("FoamMaskFalloff", Float) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_FoamMaskHardness("FoamMaskHardness", Float) = 0
		_FoamMoveSpeed("FoamMoveSpeed", Float) = 0
		_FoamTopLayerMaskFalloff("FoamTopLayerMaskFalloff", Float) = 0
		_FoamMoveDirection("FoamMoveDirection", Vector) = (0,0,0,0)
		_DepthFadeColorIntensity("DepthFadeColorIntensity", Range( 0 , 1)) = 0
		_Wavelenght("Wavelenght", Float) = 1
		_SmallNormalSpeed("SmallNormalSpeed", Vector) = (0,0,0,0)
		_BigNormalSpeed("BigNormalSpeed", Vector) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float3 _Wave0Dir;
		uniform float _Wavelenght;
		uniform float _Wavespeed;
		uniform float _Amplitude;
		uniform float _Steepness;
		uniform float _NumberOfWaves;
		uniform float3 _Wave1Dir;
		uniform float3 _Wave2Dir;
		uniform float _WaveLengthMultiplier;
		uniform float _WaveSpeedMultiplier;
		uniform float _AmplitudeDivider;
		uniform float3 _Wave3Dir;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthFadeDistance;
		uniform sampler2D _BigNormalMap;
		uniform float2 _BigNormalSpeed;
		uniform float _BigNormalTiling;
		uniform float _BigNormalIntensity;
		uniform sampler2D _SmallNormalMap;
		uniform float2 _SmallNormalSpeed;
		uniform float _SmallNormalTiling;
		uniform float _SmallNormalIntensity;
		uniform float4 _MainColor;
		uniform float4 _CrestColor;
		uniform float _FoamMoveSpeed;
		uniform float2 _FoamMoveDirection;
		uniform float _FoamScale;
		uniform float _FoamMask;
		uniform float _FoamMaskFalloff;
		uniform float _FoamMaskHardness;
		uniform float _FoamTopLayerMaskFalloff;
		uniform float _FoamIntesnity;
		uniform float4 _FoamColor;
		uniform float _DepthFadeColorIntensity;
		uniform samplerCUBE _SpecularHDRi;
		uniform float _SpecularIntensity;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float _Smoothness;
		uniform float _EdgeLength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 temp_output_3_0_g146 = _Wave0Dir;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float dotResult14_g147 = dot( temp_output_3_0_g146 , ase_worldPos );
			float temp_output_5_0_g147 = _Wavelenght;
			float temp_output_10_0_g147 = sqrt( ( ( 6.28318548202515 / temp_output_5_0_g147 ) * 9.81 ) );
			float temp_output_25_0_g146 = ( ( dotResult14_g147 * temp_output_10_0_g147 ) + ( ( ( 2 / temp_output_5_0_g147 ) * _Wavespeed ) * _Time.y ) );
			float temp_output_6_0_g146 = cos( temp_output_25_0_g146 );
			float3 break10_g146 = temp_output_3_0_g146;
			float temp_output_8_0_g146 = _Amplitude;
			float temp_output_23_0_g146 = ( temp_output_8_0_g146 * ( _Steepness / ( temp_output_10_0_g147 * temp_output_8_0_g146 * _NumberOfWaves ) ) );
			float3 appendResult2_g146 = (float3(( temp_output_6_0_g146 * break10_g146.x * temp_output_23_0_g146 ) , ( sin( temp_output_25_0_g146 ) * temp_output_8_0_g146 ) , ( temp_output_6_0_g146 * break10_g146.z * temp_output_23_0_g146 )));
			float3 temp_output_3_0_g152 = _Wave1Dir;
			float dotResult14_g153 = dot( temp_output_3_0_g152 , ase_worldPos );
			float temp_output_5_0_g153 = _Wavelenght;
			float temp_output_10_0_g153 = sqrt( ( ( 6.28318548202515 / temp_output_5_0_g153 ) * 9.81 ) );
			float temp_output_25_0_g152 = ( ( dotResult14_g153 * temp_output_10_0_g153 ) + ( ( ( 2 / temp_output_5_0_g153 ) * _Wavespeed ) * _Time.y ) );
			float temp_output_6_0_g152 = cos( temp_output_25_0_g152 );
			float3 break10_g152 = temp_output_3_0_g152;
			float temp_output_8_0_g152 = _Amplitude;
			float temp_output_23_0_g152 = ( temp_output_8_0_g152 * ( _Steepness / ( temp_output_10_0_g153 * temp_output_8_0_g152 * _NumberOfWaves ) ) );
			float3 appendResult2_g152 = (float3(( temp_output_6_0_g152 * break10_g152.x * temp_output_23_0_g152 ) , ( sin( temp_output_25_0_g152 ) * temp_output_8_0_g152 ) , ( temp_output_6_0_g152 * break10_g152.z * temp_output_23_0_g152 )));
			float3 temp_output_3_0_g148 = _Wave2Dir;
			float dotResult14_g149 = dot( temp_output_3_0_g148 , ase_worldPos );
			float temp_output_90_0 = ( _Wavelenght * _WaveLengthMultiplier );
			float temp_output_5_0_g149 = temp_output_90_0;
			float temp_output_10_0_g149 = sqrt( ( ( 6.28318548202515 / temp_output_5_0_g149 ) * 9.81 ) );
			float temp_output_92_0 = ( _Wavespeed * _WaveSpeedMultiplier );
			float temp_output_25_0_g148 = ( ( dotResult14_g149 * temp_output_10_0_g149 ) + ( ( ( 2 / temp_output_5_0_g149 ) * temp_output_92_0 ) * _Time.y ) );
			float temp_output_6_0_g148 = cos( temp_output_25_0_g148 );
			float3 break10_g148 = temp_output_3_0_g148;
			float temp_output_93_0 = ( _Amplitude / _AmplitudeDivider );
			float temp_output_8_0_g148 = temp_output_93_0;
			float temp_output_23_0_g148 = ( temp_output_8_0_g148 * ( _Steepness / ( temp_output_10_0_g149 * temp_output_8_0_g148 * _NumberOfWaves ) ) );
			float3 appendResult2_g148 = (float3(( temp_output_6_0_g148 * break10_g148.x * temp_output_23_0_g148 ) , ( sin( temp_output_25_0_g148 ) * temp_output_8_0_g148 ) , ( temp_output_6_0_g148 * break10_g148.z * temp_output_23_0_g148 )));
			float3 temp_output_3_0_g150 = _Wave3Dir;
			float dotResult14_g151 = dot( temp_output_3_0_g150 , ase_worldPos );
			float temp_output_5_0_g151 = temp_output_90_0;
			float temp_output_10_0_g151 = sqrt( ( ( 6.28318548202515 / temp_output_5_0_g151 ) * 9.81 ) );
			float temp_output_25_0_g150 = ( ( dotResult14_g151 * temp_output_10_0_g151 ) + ( ( ( 2 / temp_output_5_0_g151 ) * temp_output_92_0 ) * _Time.y ) );
			float temp_output_6_0_g150 = cos( temp_output_25_0_g150 );
			float3 break10_g150 = temp_output_3_0_g150;
			float temp_output_8_0_g150 = temp_output_93_0;
			float temp_output_23_0_g150 = ( temp_output_8_0_g150 * ( _Steepness / ( temp_output_10_0_g151 * temp_output_8_0_g150 * _NumberOfWaves ) ) );
			float3 appendResult2_g150 = (float3(( temp_output_6_0_g150 * break10_g150.x * temp_output_23_0_g150 ) , ( sin( temp_output_25_0_g150 ) * temp_output_8_0_g150 ) , ( temp_output_6_0_g150 * break10_g150.z * temp_output_23_0_g150 )));
			float3 Offset17 = ( appendResult2_g146 + appendResult2_g152 + appendResult2_g148 + appendResult2_g150 );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth124 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_LOD( _CameraDepthTexture, float4( ase_screenPosNorm.xy, 0, 0 ) ));
			float distanceDepth124 = abs( ( screenDepth124 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) );
			float temp_output_126_0 = saturate( distanceDepth124 );
			float DepthFade131 = temp_output_126_0;
			v.vertex.xyz += ( Offset17 * DepthFade131 );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_97_0 = (ase_worldPos).xz;
			float2 panner99 = ( 1.0 * _Time.y * _BigNormalSpeed + ( temp_output_97_0 / _BigNormalTiling ));
			float2 panner104 = ( 1.0 * _Time.y * _SmallNormalSpeed + ( temp_output_97_0 / _SmallNormalTiling ));
			float3 Normals110 = saturate( BlendNormals( UnpackScaleNormal( tex2D( _BigNormalMap, panner99 ), _BigNormalIntensity ) , UnpackScaleNormal( tex2D( _SmallNormalMap, panner104 ), _SmallNormalIntensity ) ) );
			o.Normal = Normals110;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 lerpResult24 = lerp( _MainColor , _CrestColor , saturate( ase_vertex3Pos.y ));
			float mulTime172 = _Time.y * _FoamMoveSpeed;
			float2 temp_output_145_0 = (ase_worldPos).xz;
			float2 panner170 = ( mulTime172 * _FoamMoveDirection + temp_output_145_0);
			float simplePerlin2D28 = snoise( panner170*_FoamScale );
			simplePerlin2D28 = simplePerlin2D28*0.5 + 0.5;
			float2 panner171 = ( mulTime172 * _FoamMoveDirection + temp_output_145_0);
			float simplePerlin2D33 = snoise( panner171*_FoamMask );
			simplePerlin2D33 = simplePerlin2D33*0.5 + 0.5;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth124 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth124 = abs( ( screenDepth124 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFadeDistance ) );
			float temp_output_126_0 = saturate( distanceDepth124 );
			float DepthFade131 = temp_output_126_0;
			float4 Color25 = saturate( ( lerpResult24 + ( ( ( simplePerlin2D28 * saturate( ( pow( simplePerlin2D33 , _FoamMaskFalloff ) * _FoamMaskHardness ) ) * pow( saturate( ase_vertex3Pos.y ) , _FoamTopLayerMaskFalloff ) * _FoamIntesnity ) * _FoamColor ) + ( _FoamColor * ( 1.0 - DepthFade131 ) * _DepthFadeColorIntensity ) ) ) );
			o.Albedo = saturate( Color25 ).rgb;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float fresnelNdotV52 = dot( mul(ase_tangentToWorldFast,Normals110), ase_worldViewDir );
			float fresnelNode52 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV52, _FresnelPower ) );
			float4 Specular56 = ( texCUBE( _SpecularHDRi, -reflect( ase_worldViewDir , ase_worldNormal ) ) * _SpecularIntensity * fresnelNode52 );
			o.Specular = Specular56.rgb;
			o.Smoothness = _Smoothness;
			float Opacity128 = ( saturate( pow( ( 1.0 - saturate( ase_vertex3Pos.y ) ) , 1.75 ) ) * temp_output_126_0 );
			o.Alpha = Opacity128;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 screenPos : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
7;18;1906;1001;3956.843;2019.683;3.199104;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;144;-3502.285,-988.9279;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;173;-3516.906,-598.4845;Inherit;False;Property;_FoamMoveSpeed;FoamMoveSpeed;37;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;172;-3271.906,-603.4845;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;123;-3402.708,1088.822;Inherit;False;2141.851;766.5105;Normals;17;96;106;103;97;102;108;107;105;111;99;112;104;100;101;109;110;157;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;174;-3419.906,-776.4845;Inherit;False;Property;_FoamMoveDirection;FoamMoveDirection;39;0;Create;True;0;0;0;False;0;False;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ComponentMaskNode;145;-3248.138,-947.0601;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;42;-2801.086,-941.3415;Inherit;False;1508.481;914.1271;foam;19;32;29;37;33;30;41;28;38;132;133;136;146;147;148;149;150;159;160;175;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;-1220.063,320.063;Inherit;False;1706.954;552.8463;Opacity Control;12;71;72;73;120;124;119;122;126;127;128;125;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2888.52,-485.5188;Inherit;False;Property;_FoamMask;FoamMask;14;0;Create;True;0;0;0;False;0;False;0;4.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;125;-660.8486,756.9094;Inherit;False;Property;_DepthFadeDistance;DepthFadeDistance;29;0;Create;True;0;0;0;False;0;False;0;0.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;171;-2989.906,-781.4845;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;96;-3354.708,1368.542;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;148;-2532.218,-385.3255;Inherit;False;Property;_FoamMaskFalloff;FoamMaskFalloff;34;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;124;-411.8311,727.2736;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;33;-2639.215,-629.964;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-2853.913,1587.292;Inherit;False;Property;_SmallNormalTiling;SmallNormalTiling;25;0;Create;True;0;0;0;False;0;False;0;0.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-2772.301,1244.674;Inherit;False;Property;_BigNormalTiling;BigNormalTiling;24;0;Create;True;0;0;0;False;0;False;0;1.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;97;-3171.871,1369.423;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;107;-2762.073,1695.803;Inherit;False;Property;_SmallNormalSpeed;SmallNormalSpeed;42;0;Create;True;0;0;0;False;0;False;0,0;0.11,-0.02;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;105;-2660.212,1505.393;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-2250.096,-383.4036;Inherit;False;Property;_FoamMaskHardness;FoamMaskHardness;36;0;Create;True;0;0;0;False;0;False;0;1.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;108;-2651.228,1329.424;Inherit;False;Property;_BigNormalSpeed;BigNormalSpeed;43;0;Create;True;0;0;0;False;0;False;0,0;0.03,-0.04;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SaturateNode;126;-82.43949,661.7416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;29;-2469.575,-241.3187;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;102;-2625,1149.975;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;146;-2400.043,-634.5942;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;170;-2987.906,-939.4845;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-2393.959,1344.335;Inherit;False;Property;_BigNormalIntensity;BigNormalIntensity;27;0;Create;True;0;0;0;False;0;False;0;0.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;99;-2478.371,1138.822;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-2822.009,-764.6064;Inherit;False;Property;_FoamScale;FoamScale;13;0;Create;True;0;0;0;False;0;False;0;1.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;160;-2019.247,-130.4634;Inherit;False;Property;_FoamTopLayerMaskFalloff;FoamTopLayerMaskFalloff;38;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;30;-2198.01,-200.1807;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;104;-2513.582,1494.24;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;147;-2132.664,-637.4496;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-2331.972,1651.838;Inherit;False;Property;_SmallNormalIntensity;SmallNormalIntensity;28;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;131;125.5654,685.1126;Inherit;False;DepthFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;150;-1894.684,-680.4791;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;100;-2214.687,1168.993;Inherit;True;Property;_BigNormalMap;BigNormalMap;21;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;d39fd28d79579814382bf865d8b0e584;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;27;-2488.488,-1720.33;Inherit;False;1289.964;636.4135;Color;5;20;21;22;23;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;28;-2634.76,-908.6114;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;101;-2164.833,1441.686;Inherit;True;Property;_SmallNormalMap;SmallNormalMap;23;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;4c06e28a171e3844e9621276cef0c932;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-1773.198,-263.096;Inherit;False;Property;_FoamIntesnity;Foam Intesnity;15;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;159;-1901.247,-355.4634;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;132;-1690.003,-132.4981;Inherit;False;131;DepthFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;136;-1508.131,-569.1423;Inherit;False;Property;_FoamColor;FoamColor;30;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.990566,0.9008651,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;109;-1810.554,1262.9;Inherit;True;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;175;-1451.6,-104.2613;Inherit;False;Property;_DepthFadeColorIntensity;DepthFadeColorIntensity;40;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;-140.2632,-1649.902;Inherit;False;1719.342;1186.448;Vertex Offset;16;67;66;10;17;11;12;5;14;13;68;90;92;93;138;139;140;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1529.425,-858.6424;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;71;-1170.063,370.063;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;20;-2438.488,-1262.917;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;133;-1460.935,-224.731;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-38.61151,-1230.851;Inherit;False;Property;_Wavespeed;Wavespeed;26;0;Create;True;0;0;0;False;0;False;1;1.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;45;-3016.111,392.8703;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;44;-3006.635,192.7893;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;139;312.6673,-893.5229;Inherit;False;Property;_WaveSpeedMultiplier;WaveSpeedMultiplier;32;0;Create;True;0;0;0;False;0;False;0;2;1;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;138;352.6673,-784.5229;Inherit;False;Property;_WaveLengthMultiplier;WaveLengthMultiplier;31;0;Create;True;0;0;0;False;0;False;0;100;1;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-1245.369,-853.3354;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-64.96854,-1352.503;Inherit;False;Property;_Amplitude;Amplitude;12;0;Create;True;0;0;0;False;0;False;1;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;140;302.6673,-995.5229;Inherit;False;Property;_AmplitudeDivider;AmplitudeDivider;33;0;Create;True;0;0;0;False;0;False;0;10.4;1;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;137;-1196.384,-445.1581;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;22;-2029.477,-1670.33;Inherit;False;Property;_MainColor;Main Color;11;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,0.1193655,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;157;-1519.665,1261.929;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;23;-2031.075,-1494.583;Inherit;False;Property;_CrestColor;Crest Color;22;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.9607843,0.6538843,0.05882348,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;72;-909.7927,420.4582;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;21;-2136.522,-1219.779;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-34.6864,-1136.352;Inherit;False;Property;_Wavelenght;Wavelenght;41;0;Create;True;0;0;0;False;0;False;1;3.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;73;-679.2717,419.8086;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;2.858585,-1578.763;Inherit;False;Property;_Steepness;Steepness;5;0;Create;True;0;0;0;False;0;False;0;0.17;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;24;-1642.33,-1417.903;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;621.3568,-911.3427;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;67;225.4992,-610.9223;Inherit;False;Property;_Wave2Dir;Wave2Dir;9;0;Create;True;0;0;0;False;0;False;0,0,0;0.34,0,0.18;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;11;-59.44862,-1456.601;Inherit;False;Property;_NumberOfWaves;NumberOfWaves;10;0;Create;True;0;0;0;False;0;False;1;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;10;-41.26363,-1042.113;Inherit;False;Property;_Wave0Dir;Wave0Dir;8;0;Create;True;0;0;0;False;0;False;0,1,0;1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;93;624.6967,-1023.595;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;43;-2647.961,294.018;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;636.4277,-811.776;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;66;-44.60077,-725.5222;Inherit;False;Property;_Wave1Dir;Wave1Dir;7;0;Create;True;0;0;0;False;0;False;0,0,0;0.26,0,0.35;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;-1381.008,1250.956;Inherit;False;Normals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;135;-886.0793,-830.4988;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;120;-489.9731,577.3437;Inherit;False;Constant;_Float1;Float 1;28;0;Create;True;0;0;0;False;0;False;1.75;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;68;651.7795,-584.1353;Inherit;False;Property;_Wave3Dir;Wave3Dir;6;0;Create;True;0;0;0;False;0;False;0,0,0;1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-908.4043,-1308.619;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NegateNode;46;-2410.046,294.0183;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;179;1006.417,-1529.688;Inherit;False;GerstnerFunction;-1;;146;f368161b860e9ca46bc6377e013b738f;0;6;21;FLOAT;1;False;20;FLOAT;0;False;8;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT;10;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;55;-2621.85,830.8002;Inherit;False;Property;_FresnelPower;FresnelPower;19;0;Create;True;0;0;0;False;0;False;5;4.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;178;998.0114,-830.8046;Inherit;False;GerstnerFunction;-1;;150;f368161b860e9ca46bc6377e013b738f;0;6;21;FLOAT;1;False;20;FLOAT;0;False;8;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT;10;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;119;-385.9731,397.3438;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;176;977.5482,-1034.81;Inherit;False;GerstnerFunction;-1;;148;f368161b860e9ca46bc6377e013b738f;0;6;21;FLOAT;1;False;20;FLOAT;0;False;8;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT;10;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2669.924,654.3466;Inherit;False;Property;_FresnelBias;FresnelBias;20;0;Create;True;0;0;0;False;0;False;0.03529412;0.345;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;177;997.4727,-1291.237;Inherit;False;GerstnerFunction;-1;;152;f368161b860e9ca46bc6377e013b738f;0;6;21;FLOAT;1;False;20;FLOAT;0;False;8;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT;10;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;-2560.059,515.3373;Inherit;False;110;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2628.731,744.0494;Inherit;False;Property;_FresnelScale;FresnelScale;17;0;Create;True;0;0;0;False;0;False;1;2.37;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;1341.218,-1267.071;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2248.351,500.7867;Inherit;False;Property;_SpecularIntensity;SpecularIntensity;18;0;Create;True;0;0;0;False;0;False;1;0.532;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;134;-756.9386,-1302.508;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;48;-2184.51,263.8671;Inherit;True;Property;_SpecularHDRi;SpecularHDRi;16;2;[HDR];[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;ef7513b54a0670140b9b967af7620563;True;0;False;white;Auto;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;122;-111.9732,390.3438;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;52;-2194.819,649.8528;Inherit;False;Standard;TangentNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;1497.096,-1512.362;Inherit;False;Offset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-1797.527,254.6384;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;79.43387,498.6692;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-612.4674,-1317.735;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;262.8903,528.6457;Inherit;False;Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;599.1052,354.6216;Inherit;False;17;Offset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;-1538.152,245.2443;Inherit;False;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;631.9772,461.7852;Inherit;False;131;DepthFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;1017.282,-65.28445;Inherit;False;25;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;114;999.6434,31.27992;Inherit;False;110;Normals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;904.6434,90.27991;Inherit;False;56;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;1037.508,271.3437;Inherit;False;128;Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;152;1287.413,-52.32957;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;151;1162.499,163.4859;Inherit;False;Property;_Smoothness;Smoothness;35;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;887.0712,386.8231;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1495.644,-40.72004;Float;False;True;-1;6;ASEMaterialInspector;0;0;StandardSpecular;Waves;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;172;0;173;0
WireConnection;145;0;144;0
WireConnection;171;0;145;0
WireConnection;171;2;174;0
WireConnection;171;1;172;0
WireConnection;124;0;125;0
WireConnection;33;0;171;0
WireConnection;33;1;37;0
WireConnection;97;0;96;0
WireConnection;105;0;97;0
WireConnection;105;1;106;0
WireConnection;126;0;124;0
WireConnection;102;0;97;0
WireConnection;102;1;103;0
WireConnection;146;0;33;0
WireConnection;146;1;148;0
WireConnection;170;0;145;0
WireConnection;170;2;174;0
WireConnection;170;1;172;0
WireConnection;99;0;102;0
WireConnection;99;2;108;0
WireConnection;30;0;29;2
WireConnection;104;0;105;0
WireConnection;104;2;107;0
WireConnection;147;0;146;0
WireConnection;147;1;149;0
WireConnection;131;0;126;0
WireConnection;150;0;147;0
WireConnection;100;1;99;0
WireConnection;100;5;111;0
WireConnection;28;0;170;0
WireConnection;28;1;32;0
WireConnection;101;1;104;0
WireConnection;101;5;112;0
WireConnection;159;0;30;0
WireConnection;159;1;160;0
WireConnection;109;0;100;0
WireConnection;109;1;101;0
WireConnection;38;0;28;0
WireConnection;38;1;150;0
WireConnection;38;2;159;0
WireConnection;38;3;41;0
WireConnection;133;0;132;0
WireConnection;141;0;38;0
WireConnection;141;1;136;0
WireConnection;137;0;136;0
WireConnection;137;1;133;0
WireConnection;137;2;175;0
WireConnection;157;0;109;0
WireConnection;72;0;71;2
WireConnection;21;0;20;2
WireConnection;73;0;72;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;24;2;21;0
WireConnection;92;0;13;0
WireConnection;92;1;139;0
WireConnection;93;0;12;0
WireConnection;93;1;140;0
WireConnection;43;0;44;0
WireConnection;43;1;45;0
WireConnection;90;0;14;0
WireConnection;90;1;138;0
WireConnection;110;0;157;0
WireConnection;135;0;141;0
WireConnection;135;1;137;0
WireConnection;40;0;24;0
WireConnection;40;1;135;0
WireConnection;46;0;43;0
WireConnection;179;21;5;0
WireConnection;179;20;11;0
WireConnection;179;8;12;0
WireConnection;179;4;13;0
WireConnection;179;5;14;0
WireConnection;179;3;10;0
WireConnection;178;21;5;0
WireConnection;178;20;11;0
WireConnection;178;8;93;0
WireConnection;178;4;92;0
WireConnection;178;5;90;0
WireConnection;178;3;68;0
WireConnection;119;0;73;0
WireConnection;119;1;120;0
WireConnection;176;21;5;0
WireConnection;176;20;11;0
WireConnection;176;8;93;0
WireConnection;176;4;92;0
WireConnection;176;5;90;0
WireConnection;176;3;67;0
WireConnection;177;21;5;0
WireConnection;177;20;11;0
WireConnection;177;8;12;0
WireConnection;177;4;13;0
WireConnection;177;5;14;0
WireConnection;177;3;66;0
WireConnection;69;0;179;0
WireConnection;69;1;177;0
WireConnection;69;2;176;0
WireConnection;69;3;178;0
WireConnection;134;0;40;0
WireConnection;48;1;46;0
WireConnection;122;0;119;0
WireConnection;52;0;113;0
WireConnection;52;1;53;0
WireConnection;52;2;54;0
WireConnection;52;3;55;0
WireConnection;17;0;69;0
WireConnection;50;0;48;0
WireConnection;50;1;51;0
WireConnection;50;2;52;0
WireConnection;127;0;122;0
WireConnection;127;1;126;0
WireConnection;25;0;134;0
WireConnection;128;0;127;0
WireConnection;56;0;50;0
WireConnection;152;0;26;0
WireConnection;143;0;18;0
WireConnection;143;1;142;0
WireConnection;0;0;152;0
WireConnection;0;1;114;0
WireConnection;0;3;57;0
WireConnection;0;4;151;0
WireConnection;0;9;129;0
WireConnection;0;11;143;0
ASEEND*/
//CHKSM=D7727742B3FA64CE2D8B29B182BC931429538319