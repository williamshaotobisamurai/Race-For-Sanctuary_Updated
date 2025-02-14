// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Burned/Coal_Mask_notGlobal"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_CoalMasks("CoalMasks", 2D) = "white" {}
		_WoodAlbedo("WoodAlbedo", 2D) = "white" {}
		_WoodNormal("WoodNormal", 2D) = "bump" {}
		_CoalNormal("CoalNormal", 2D) = "bump" {}
		[HDR]_CoalColor("CoalColor", Color) = (0,0,0,0)
		_CinderPower("CinderPower", Range( 0 , 3)) = 2
		_StopBurning("StopBurning", Range( 0 , 1)) = 0.5
		_CinderSidePower("CinderSidePower", Range( 0 , 3)) = 1.55
		_CinderLow("CinderLow", Range( 0 , 3)) = 0.18
		_AshesPower("AshesPower", Float) = 3.6
		_UVTiling("UVTiling", Vector) = (1,1,0,0)
		_DetailWoodAlbedo("DetailWoodAlbedo", 2D) = "white" {}
		_Specular("Specular", Color) = (0,0,0,0)
		_WoodSm("WoodSm", Float) = 0
		_TriplanarMap("TriplanarMap", 2D) = "white" {}
		_AO_Coal1("AO_Coal", Float) = 0
		_StartBurning("StartBurning", Range( 0 , 1)) = 0
		_DetailWoodNormal("DetailWoodNormal", 2D) = "bump" {}
		_MaskPower("MaskPower", Float) = 1.15
		_TriplanarScale("TriplanarScale", Float) = 0.2
		_WoodOcclusion_AOR("WoodOcclusion_AO(R)", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
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
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _DetailWoodNormal;
		uniform sampler2D _WoodNormal;
		uniform float4 _WoodNormal_ST;
		uniform sampler2D _CoalNormal;
		uniform float2 _UVTiling;
		uniform sampler2D _CoalMasks;
		uniform float4 _CoalMasks_ST;
		uniform float _StartBurning;
		uniform sampler2D _WoodAlbedo;
		uniform float4 _WoodAlbedo_ST;
		uniform sampler2D _DetailWoodAlbedo;
		uniform float4 _DetailWoodAlbedo_ST;
		uniform float _AshesPower;
		uniform float _CinderSidePower;
		uniform float _CinderLow;
		uniform sampler2D _TriplanarMap;
		uniform float _TriplanarScale;
		uniform float _CinderPower;
		uniform float _StopBurning;
		uniform float4 _CoalColor;
		uniform float4 _Specular;
		uniform float _WoodSm;
		uniform sampler2D _WoodOcclusion_AOR;
		uniform float4 _WoodOcclusion_AOR_ST;
		uniform float _AO_Coal1;
		uniform float _MaskPower;
		uniform float _Cutoff = 0.5;


		inline float4 TriplanarSampling294( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_WoodNormal = i.uv_texcoord * _WoodNormal_ST.xy + _WoodNormal_ST.zw;
			float2 temp_output_45_0 = ( i.uv_texcoord * _UVTiling );
			float2 uv_CoalMasks = i.uv_texcoord * _CoalMasks_ST.xy + _CoalMasks_ST.zw;
			float clampResult43 = clamp( ( 1.0 - pow( tex2D( _CoalMasks, uv_CoalMasks ).b , (0.0 + (_StartBurning - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) ) ) , 0.0 , 1.0 );
			float3 lerpResult93 = lerp( BlendNormals( UnpackNormal( tex2D( _DetailWoodNormal, ( i.uv_texcoord * 5.0 ) ) ) , UnpackNormal( tex2D( _WoodNormal, uv_WoodNormal ) ) ) , UnpackNormal( tex2D( _CoalNormal, temp_output_45_0 ) ) , clampResult43);
			o.Normal = lerpResult93;
			float2 uv_WoodAlbedo = i.uv_texcoord * _WoodAlbedo_ST.xy + _WoodAlbedo_ST.zw;
			float4 tex2DNode10 = tex2D( _WoodAlbedo, uv_WoodAlbedo );
			float2 uv_DetailWoodAlbedo = i.uv_texcoord * _DetailWoodAlbedo_ST.xy + _DetailWoodAlbedo_ST.zw;
			float temp_output_9_0_g1 = tex2DNode10.r;
			float temp_output_18_0_g1 = ( 1.0 - temp_output_9_0_g1 );
			float3 appendResult16_g1 = (float3(temp_output_18_0_g1 , temp_output_18_0_g1 , temp_output_18_0_g1));
			float FirePower224 = _StartBurning;
			float4 tex2DNode24 = tex2D( _CoalMasks, temp_output_45_0 );
			float CoalColor205 = tex2DNode24.r;
			float3 temp_cast_2 = (CoalColor205).xxx;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float4 transform117 = mul(unity_ObjectToWorld,float4( ase_vertexNormal , 0.0 ));
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform152 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float3 ase_worldPos = i.worldPos;
			float4 triplanar294 = TriplanarSampling294( _TriplanarMap, ase_worldPos, ase_worldNormal, 1.0, _TriplanarScale, 1.0, 0 );
			float2 temp_cast_5 = (tex2D( _CoalMasks, ( ( i.uv_texcoord * float2( 0.5,0.5 ) ) + ( float2( 0.005,0.01 ) * _Time.y ) ) ).b).xx;
			float2 lerpResult175 = lerp( ( ( triplanar294.x * 0.5 ) * i.uv_texcoord ) , temp_cast_5 , 0.01);
			float4 tex2DNode29 = tex2D( _CoalMasks, lerpResult175 );
			float clampResult180 = clamp( ( CoalColor205 * _CinderLow * tex2DNode29.g ) , 0.0 , 1.0 );
			float clampResult155 = clamp( ( max( transform117.y , 0.0 ) + ( max( transform152.y , 0.0 ) * _CinderSidePower ) + clampResult180 ) , 0.0 , 1.0 );
			float temp_output_91_0 = ( clampResult155 * _CinderPower * 100.0 * pow( tex2DNode29.g , (6.0 + (_StopBurning - 0.0) * (-10.0 - 6.0) / (1.0 - 0.0)) ) );
			float clampResult192 = clamp( ( pow( CoalColor205 , _AshesPower ) * (0.0 + (temp_output_91_0 - 0.9) * (1.0 - 0.0) / (1.0 - 0.9)) * FirePower224 ) , 0.0 , 1.0 );
			float3 lerpResult306 = lerp( ( ( tex2DNode10.rgb * ( ( ( tex2D( _DetailWoodAlbedo, uv_DetailWoodAlbedo ).rgb * (unity_ColorSpaceDouble).rgb ) * temp_output_9_0_g1 ) + appendResult16_g1 ) ) * (0.77 + (FirePower224 - 0.0) * (0.0 - 0.77) / (1.0 - 0.0)) ) , temp_cast_2 , clampResult192);
			o.Albedo = lerpResult306;
			float2 temp_cast_6 = (( tex2D( _CoalMasks, ( i.uv_texcoord * 2.0 ) ).g * 0.5 )).xx;
			float2 panner21 = ( 1.0 * _Time.y * float2( 0.15,0 ) + temp_cast_6);
			float clampResult217 = clamp( ( tex2D( _CoalMasks, panner21 ).g * 4.0 * CoalColor205 ) , 0.0 , 1.0 );
			float4 lerpResult12 = lerp( float4( 0,0,0,0 ) , ( CoalColor205 * _CoalColor * clampResult217 ) , clampResult43);
			float clampResult49 = clamp( temp_output_91_0 , 0.0 , 1.0 );
			float temp_output_123_0 = ( 1.0 - clampResult49 );
			float4 lerpResult87 = lerp( float4( 0,0,0,0 ) , lerpResult12 , ( FirePower224 * temp_output_123_0 ));
			o.Emission = lerpResult87.rgb;
			float4 temp_cast_8 = (0.0).xxxx;
			float4 lerpResult270 = lerp( _Specular , temp_cast_8 , FirePower224);
			o.Specular = lerpResult270.rgb;
			float CoalSmAo207 = tex2DNode24.a;
			float lerpResult228 = lerp( _WoodSm , CoalSmAo207 , FirePower224);
			o.Smoothness = saturate( lerpResult228 );
			float2 uv_WoodOcclusion_AOR = i.uv_texcoord * _WoodOcclusion_AOR_ST.xy + _WoodOcclusion_AOR_ST.zw;
			float CoalNoise2206 = tex2DNode24.b;
			float lerpResult238 = lerp( pow( tex2D( _WoodOcclusion_AOR, uv_WoodOcclusion_AOR ).r , 1.0 ) , ( ( temp_output_123_0 * ( 1.0 - ( CoalNoise2206 + clampResult180 ) ) ) + _AO_Coal1 ) , FirePower224);
			o.Occlusion = saturate( lerpResult238 );
			o.Alpha = 1;
			float BurnedMask314 = ( (2.0 + (FirePower224 - 0.0) * (_MaskPower - 2.0) / (1.0 - 0.0)) * triplanar294.r );
			clip( ( BurnedMask314 * tex2DNode10.a ) - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardSpecular keepalpha fullforwardshadows nodynlightmap 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
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
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
-1920;6;1920;1013;624.6935;-225.5349;1;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;169;-4492.737,1297.04;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;170;-4469.048,1757.396;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;168;-4481.465,1602.799;Float;False;Constant;_Vector1;Vector 1;24;0;Create;True;0;0;0;False;0;False;0.005,0.01;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;305;-4531.613,842.5508;Float;True;Property;_TriplanarMap;TriplanarMap;19;0;Create;True;0;0;0;False;0;False;None;86104a73ce569514d93898e3cd85d452;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;296;-4494.289,1054.009;Float;False;Property;_TriplanarScale;TriplanarScale;24;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;167;-4478.03,1448.17;Float;False;Constant;_Vector0;Vector 0;24;0;Create;True;0;0;0;False;0;False;0.5,0.5;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;304;-4131.655,1225.085;Float;False;Constant;_TriplanarPower;TriplanarPower;23;0;Create;True;0;0;0;False;0;False;0.5;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;44;-3012.046,-586.5157;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;171;-4220.305,1607.928;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TriplanarNode;294;-4249.93,1035.843;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;0;None;Mid Texture 0;_MidTexture0;white;1;None;Bot Texture 0;_BotTexture0;white;0;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-4233.979,1431.352;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;236;-2970.067,-460.4288;Float;False;Property;_UVTiling;UVTiling;15;0;Create;True;0;0;0;False;0;False;1,1;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2636.472,-477.3878;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-4017.446,1525.468;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;-3848.458,1208.106;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;176;-3708.415,1719.024;Float;False;Constant;_CinderNoiseUVScale;CinderNoiseUVScale;10;0;Create;True;0;0;0;False;0;False;0.01;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-2369.598,-739.6808;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;0;False;0;False;-1;2d479324bd85dcd4facb271ae1f53661;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Instance;33;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;302;-3642.839,1275.173;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;174;-3824.649,1497.726;Inherit;True;Property;_TextureSample5;Texture Sample 5;5;0;Create;True;0;0;0;False;0;False;-1;None;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Instance;33;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;175;-3423.016,1476.735;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-1898.996,-896.9099;Float;False;CoalColor;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;149;-3258.145,792.7192;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;109;-3005.712,623.7775;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;208;-2918.182,1202.111;Inherit;False;205;CoalColor;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;152;-2989.173,792.129;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;178;-3005.34,1289.125;Float;False;Property;_CinderLow;CinderLow;13;0;Create;True;0;0;0;False;0;False;0.18;-0.29;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-3149.637,1446.938;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;0;False;0;False;-1;2d479324bd85dcd4facb271ae1f53661;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Instance;33;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;16;-3556.256,188.3745;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-3594.913,309.7477;Float;False;Constant;_CoalNoiseEmissionUvScale;CoalNoiseEmissionUvScale;4;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;153;-2699.257,838.8573;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-2777.392,969.3057;Float;False;Property;_CinderSidePower;CinderSidePower;12;0;Create;True;0;0;0;False;0;False;1.55;1.49;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;117;-2717.279,623.5734;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;182;-2621.456,1207.176;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;115;-2425.665,669.0167;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-2439.293,838.381;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;180;-2355.073,1091.586;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-3239.084,1718.399;Float;False;Property;_StopBurning;StopBurning;11;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-3215.866,189.0645;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;22;-2965.419,161.6456;Inherit;True;Property;_TextureSample0;Texture Sample 0;5;0;Create;True;0;0;0;False;0;False;-1;2d479324bd85dcd4facb271ae1f53661;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Instance;33;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;154;-2126.322,815.8316;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;235;-2878.849,1722.457;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;6;False;4;FLOAT;-10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2899.988,371.2204;Float;False;Constant;_CoalNoiseScale;CoalNoiseScale;9;0;Create;True;0;0;0;False;0;False;0.5;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;234;-1917.082,1000.55;Float;False;Constant;_Const;Const;16;0;Create;True;0;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;218;-2498.403,-106.1524;Float;False;Property;_StartBurning;StartBurning;21;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2563.081,212.026;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-2571.373,327.421;Float;False;Constant;_CoalNoiseUVPanner;CoalNoiseUVPanner;10;0;Create;True;0;0;0;False;0;False;0.15,0;0.15,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;206;-1773.581,-730.0219;Float;False;CoalNoise2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1974.856,928.1199;Float;False;Property;_CinderPower;CinderPower;10;0;Create;True;0;0;0;False;0;False;2;0.5;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;155;-1851.104,816.3149;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;114;-2569.023,1496.635;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;210;-579.4558,967.8086;Inherit;False;206;CoalNoise2;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-1512.314,812.6141;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;21;-2276.1,213.651;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;224;-2107.247,36.00164;Float;False;FirePower;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;309;-4240.425,825.0985;Float;False;Property;_MaskPower;MaskPower;23;0;Create;True;0;0;0;False;0;False;1.15;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;261;-1554.599,-824.3876;Float;False;Constant;_DetailNormalUVScale;DetailNormalUVScale;22;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;212;-1857.122,467.502;Inherit;False;205;CoalColor;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;219;-1808.443,-99.40897;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;262;-1610.021,-957.8255;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;313;-4256.765,731.6884;Inherit;False;224;FirePower;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-1869.718,381.0922;Float;False;Constant;_CoalNoisePower;CoalNoisePower;11;0;Create;True;0;0;0;False;0;False;4;2.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;33;-1909.745,-306.4492;Inherit;True;Property;_CoalMasks;CoalMasks;5;0;Create;True;0;0;0;False;0;False;-1;2d479324bd85dcd4facb271ae1f53661;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1962.441,184.8739;Inherit;True;Property;_rew;rew;5;0;Create;True;0;0;0;False;0;False;-1;2d479324bd85dcd4facb271ae1f53661;2d479324bd85dcd4facb271ae1f53661;True;0;False;white;Auto;False;Instance;33;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-313.5259,1066.087;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;49;-1194.961,812.6927;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;123;-865.5482,812.6558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;209;-1083.838,314.1763;Inherit;False;205;CoalColor;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;311;-4008.742,736.876;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;2;False;4;FLOAT;1.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1577.161,236.636;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;196;-1075.165,408.9827;Float;False;Property;_AshesPower;AshesPower;14;0;Create;True;0;0;0;False;0;False;3.6;3.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;147;-108.6176,967.7993;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;39;-1526.234,-231.0297;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;263;-1274.601,-957.6114;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;256;-1060.625,-985.342;Inherit;True;Property;_DetailWoodNormal;DetailWoodNormal;22;0;Create;True;0;0;0;False;0;False;-1;017469a3ea9ca3e4ea89e14f79a6e221;1dd0ff673f5f4fd4ba9643118392cce0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;308;-3780.184,909.428;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;110.5585,817.6112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;40;-1275.999,-230.1744;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;48;-1456.608,57.06815;Float;False;Property;_CoalColor;CoalColor;9;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;7.906699,2.607969,1.366079,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;193;-848.0005,456.4837;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0.9;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;-1394.893,-29.75921;Inherit;False;205;CoalColor;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-67.9334,-631.02;Inherit;True;Property;_WoodAlbedo;WoodAlbedo;6;0;Create;True;0;0;0;False;0;False;-1;None;23f9b61e82d1a484e8d09394b298c3f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;255;-67.6104,-440.0819;Inherit;True;Property;_DetailWoodAlbedo;DetailWoodAlbedo;16;0;Create;True;0;0;0;False;0;False;-1;a0fff3e053e70494ba9c91d152aeffa0;37cc52a2055f6934dad950c330a3ac79;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;195;-843.6543,319.1586;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;226;272.8216,-293.7007;Inherit;False;224;FirePower;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1170.334,-735.5777;Inherit;True;Property;_WoodNormal;WoodNormal;7;0;Create;True;0;0;0;False;0;False;-1;None;dda9654122c08ad4ebc1d2c4aef2224e;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;225;-858.2903,658.899;Inherit;False;224;FirePower;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;207;-1694.144,-649.9088;Float;False;CoalSmAo;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;239;-416.4312,620.5336;Inherit;True;Property;_WoodOcclusion_AOR;WoodOcclusion_AO(R);25;0;Create;True;0;0;0;False;0;False;-1;None;d67d76fd50a8ead43ab6c408277202a2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;316;-72.1934,733.0349;Inherit;False;Constant;_AO3;AO;18;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;217;-1348.136,238.1261;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;320;161.6564,945.2351;Inherit;False;Property;_AO_Coal1;AO_Coal;20;0;Create;True;0;0;0;False;0;False;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-1039.703,144.65;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;317;262.8065,650.0349;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;314;-3550.12,904.2125;Float;False;BurnedMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;223;501.7276,-288.1722;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.77;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-586.1567,434.5233;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;321;318.9566,817.8347;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;268;-683.7023,-585.7533;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;244;372.9306,-456.509;Inherit;False;Detail Albedo;1;;1;29e5a290b15a7884983e27c8f1afaa8c;0;3;12;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;9;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;43;-1042.258,-230.2259;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;229;-103.169,548.7671;Inherit;False;224;FirePower;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-62.15375,381.0183;Float;False;Property;_WoodSm;WoodSm;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;211;-102.7998,464.2261;Inherit;False;207;CoalSmAo;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-1168.178,-510.9188;Inherit;True;Property;_CoalNormal;CoalNormal;8;0;Create;True;0;0;0;False;0;False;-1;None;f91c76d43b07b034ba951400ad8c3133;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;12;-713.0228,123.4314;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;192;-374.9484,434.8003;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;315;544.5375,-602.3284;Inherit;False;314;BurnedMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;745.6387,-311.0451;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;214;504.3956,-122.4565;Inherit;False;205;CoalColor;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;272;429.8735,519.4047;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;271;223.6533,363.2494;Float;False;Constant;_SpecConst;SpecConst;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;93;-371.2772,-269.4022;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;228;224.5099,445.2271;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;220;-597.1912,663.3303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;257;210.7187,189.9626;Float;False;Property;_Specular;Specular;17;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.2735849,0.2232556,0.2232556,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;238;484.9165,651.384;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;253;-1836.276,-815.239;Float;False;CoalNoise1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;273;40.69743,-48.56853;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;310;793.0406,-553.5571;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;306;903.8019,-219.4928;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;87;-362.1723,101.7751;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;270;597.643,194.9285;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;318;629.3065,363.5349;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;319;782.3065,531.5349;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1413.548,74.58441;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Burned/Coal_Mask_notGlobal;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;171;0;168;0
WireConnection;171;1;170;0
WireConnection;294;0;305;0
WireConnection;294;3;296;0
WireConnection;172;0;169;0
WireConnection;172;1;167;0
WireConnection;45;0;44;0
WireConnection;45;1;236;0
WireConnection;173;0;172;0
WireConnection;173;1;171;0
WireConnection;303;0;294;1
WireConnection;303;1;304;0
WireConnection;24;1;45;0
WireConnection;302;0;303;0
WireConnection;302;1;169;0
WireConnection;174;1;173;0
WireConnection;175;0;302;0
WireConnection;175;1;174;3
WireConnection;175;2;176;0
WireConnection;205;0;24;1
WireConnection;152;0;149;0
WireConnection;29;1;175;0
WireConnection;153;0;152;2
WireConnection;117;0;109;0
WireConnection;182;0;208;0
WireConnection;182;1;178;0
WireConnection;182;2;29;2
WireConnection;115;0;117;2
WireConnection;158;0;153;0
WireConnection;158;1;157;0
WireConnection;180;0;182;0
WireConnection;15;0;16;0
WireConnection;15;1;17;0
WireConnection;22;1;15;0
WireConnection;154;0;115;0
WireConnection;154;1;158;0
WireConnection;154;2;180;0
WireConnection;235;0;28;0
WireConnection;20;0;22;2
WireConnection;20;1;18;0
WireConnection;206;0;24;3
WireConnection;155;0;154;0
WireConnection;114;0;29;2
WireConnection;114;1;235;0
WireConnection;91;0;155;0
WireConnection;91;1;96;0
WireConnection;91;2;234;0
WireConnection;91;3;114;0
WireConnection;21;0;20;0
WireConnection;21;2;19;0
WireConnection;224;0;218;0
WireConnection;219;0;218;0
WireConnection;9;1;21;0
WireConnection;181;0;210;0
WireConnection;181;1;180;0
WireConnection;49;0;91;0
WireConnection;123;0;49;0
WireConnection;311;0;313;0
WireConnection;311;4;309;0
WireConnection;99;0;9;2
WireConnection;99;1;100;0
WireConnection;99;2;212;0
WireConnection;147;0;181;0
WireConnection;39;0;33;3
WireConnection;39;1;219;0
WireConnection;263;0;262;0
WireConnection;263;1;261;0
WireConnection;256;1;263;0
WireConnection;308;0;311;0
WireConnection;308;1;294;1
WireConnection;127;0;123;0
WireConnection;127;1;147;0
WireConnection;40;0;39;0
WireConnection;193;0;91;0
WireConnection;195;0;209;0
WireConnection;195;1;196;0
WireConnection;207;0;24;4
WireConnection;217;0;99;0
WireConnection;47;0;213;0
WireConnection;47;1;48;0
WireConnection;47;2;217;0
WireConnection;317;0;239;1
WireConnection;317;1;316;0
WireConnection;314;0;308;0
WireConnection;223;0;226;0
WireConnection;194;0;195;0
WireConnection;194;1;193;0
WireConnection;194;2;225;0
WireConnection;321;0;127;0
WireConnection;321;1;320;0
WireConnection;268;0;256;0
WireConnection;268;1;11;0
WireConnection;244;12;10;0
WireConnection;244;11;255;0
WireConnection;244;9;10;1
WireConnection;43;0;40;0
WireConnection;13;1;45;0
WireConnection;12;1;47;0
WireConnection;12;2;43;0
WireConnection;192;0;194;0
WireConnection;85;0;244;0
WireConnection;85;1;223;0
WireConnection;272;0;229;0
WireConnection;93;0;268;0
WireConnection;93;1;13;0
WireConnection;93;2;43;0
WireConnection;228;0;267;0
WireConnection;228;1;211;0
WireConnection;228;2;229;0
WireConnection;220;0;225;0
WireConnection;220;1;123;0
WireConnection;238;0;317;0
WireConnection;238;1;321;0
WireConnection;238;2;229;0
WireConnection;253;0;24;2
WireConnection;273;0;93;0
WireConnection;310;0;315;0
WireConnection;310;1;10;4
WireConnection;306;0;85;0
WireConnection;306;1;214;0
WireConnection;306;2;192;0
WireConnection;87;1;12;0
WireConnection;87;2;220;0
WireConnection;270;0;257;0
WireConnection;270;1;271;0
WireConnection;270;2;272;0
WireConnection;318;0;228;0
WireConnection;319;0;238;0
WireConnection;0;0;306;0
WireConnection;0;1;273;0
WireConnection;0;2;87;0
WireConnection;0;3;270;0
WireConnection;0;4;318;0
WireConnection;0;5;319;0
WireConnection;0;10;310;0
ASEEND*/
//CHKSM=911B7D23963594C8EF648C9ACFFF20E6EEA5CF13