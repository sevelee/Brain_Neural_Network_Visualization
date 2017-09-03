// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

//*************************************************************
// by Seve. 2017/02/06
// Thanks for Jasper Degens @Teamlab, Sakato Yoshiaki@Teamlab
//*************************************************************

Shader "Seve/ParticleShader" {
	Properties {
		_MainTex ("Particle Tex", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Pass {
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 200
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			#pragma enable_d3d11_debug_symbols
			#pragma vertex vert
			#pragma fragment frag

			float3 _Transform;
			float4 _Rotation;
			
			struct ParticleProperty {
				float3 pos;
				bool active;
			};

			StructuredBuffer<float2> _VertexBuffer;
			StructuredBuffer<float2> _UVBuffer;
			StructuredBuffer<ParticleProperty> _ParticlePropertyBuffer;

			sampler2D _MainTex;
			fixed4 _Color;
			float _Scale;

			#include "UnityCG.cginc"
			#include "Math.cginc"

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(uint vid: SV_VERTEXID, uint iid: SV_INSTANCEID)
			{
				v2f o;

				float3 vertex = float3(_VertexBuffer[vid], 0);
				float2 uv = _UVBuffer[vid];
				ParticleProperty pp = _ParticlePropertyBuffer[iid];

				vertex = (vertex + pp.pos) * _Scale;
				vertex += _Transform;

				//float4 wPos;
				//wPos = mul(unity_ObjectToWorld, float4(vertex, 1));
				//wPos.xyz += normalize(UNITY_MATRIX_V[0].xyz) + normalize(UNITY_MATRIX_V[1].xyz);
				//o.vertex = mul(UNITY_MATRIX_VP, wPos);

				//o.vertex = mul(UNITY_MATRIX_P , mul(UNITY_MATRIX_MV , float4(0.0,0.0,0.0,1.0)) + float4(vertex.x, vertex.y, 0.0 , 0.0)); 

				o.vertex = float4(vertex, 1);
				o.vertex = mul(UNITY_MATRIX_VP, o.vertex);
				o.uv = uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a = col.r;
				col.rgb = _Color.rgb;
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
