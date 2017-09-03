// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//*************************************************************
// by Seve. 2017/02/06
// Thanks for Jasper Degens @Teamlab, Sakato Yoshiaki@Teamlab
//*************************************************************

Shader "Seve/ParticleShaderHolo1" {
	Properties {
		_MainTex ("Particle Tex", 2D) = "white" {}
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	}
	SubShader {
		Pass {
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 200
			ZWrite Off
			Blend SrcAlpha One
			Cull Off

			CGPROGRAM
			#pragma enable_d3d11_debug_symbols
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			float3 _Transform;
			float4 _Rotation;
			
			struct ParticleProperty {
				float3 pos;
				bool active;
			};

			StructuredBuffer<float2> _VertexBuffer;
			StructuredBuffer<float2> _UVBuffer;
			StructuredBuffer<float3> _PositionBuffer;

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _Scale;

			#include "UnityCG.cginc"

			struct appdata_t {
				uint vid: SV_VERTEXID;
				uint iid: SV_INSTANCEID;
				fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;

				fixed4 color : COLOR;
			};

			v2f vert(appdata_t v)
			{
				v2f o;

				float3 vertex = float3(_VertexBuffer[v.vid], 0);
				float2 uv = _UVBuffer[v.vid];
				float3 pos = _PositionBuffer[v.iid];

				vertex = (vertex + pos) * _Scale;
				vertex += _Transform;

				//float4 wPos;
				//wPos = mul(unity_ObjectToWorld, float4(vertex, 1));
				//wPos.xyz += normalize(UNITY_MATRIX_V[0].xyz) + normalize(UNITY_MATRIX_V[1].xyz);
				//o.vertex = mul(UNITY_MATRIX_VP, wPos);

				//o.vertex = mul(UNITY_MATRIX_P , mul(UNITY_MATRIX_MV , float4(0.0,0.0,0.0,1.0)) + float4(vertex.x, vertex.y, 0.0 , 0.0)); 

				o.vertex = float4(vertex, 1);
				o.vertex = mul(UNITY_MATRIX_VP, o.vertex);
				o.uv = uv;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = _TintColor * 2;
				clip(col.a);
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
