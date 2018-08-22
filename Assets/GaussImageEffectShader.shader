Shader "Hidden/GaussImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			half4 frag(v2f i) : SV_Target
			{
				const float gx0 = -2.002473;
				const float gx1 = -0.8807971;
				const float gx2 = +1;
				const float gs0 = 0.1067146;
				const float gs1 = 0.8930215;
				const float gs2 = 0.0002638651;
				float2 uv0 = float2(i.uv.x + gx0 * _MainTex_TexelSize.x, i.uv.y);
				float2 uv1 = float2(i.uv.x + gx1 * _MainTex_TexelSize.x, i.uv.y);
				float2 uv2 = float2(i.uv.x + gx2 * _MainTex_TexelSize.x, i.uv.y);
				half4 c0 = tex2D(_MainTex, uv0) * gs0;
				half4 c1 = tex2D(_MainTex, uv1) * gs1;
				half4 c2 = tex2D(_MainTex, uv2) * gs2;
				half4 cx = c0 + c2 + c1;
				return cx;
			}
			ENDCG
		}

		GrabPass{}

		Pass
		{
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;


			fixed4 frag(v2f i) : SV_Target
			{
				const float gx0 = -2.002473;
				const float gx1 = -0.8807971;
				const float gx2 = +1;
				const float gs0 = 0.1067146;
				const float gs1 = 0.8930215;
				const float gs2 = 0.0002638651;
				float2 uv0 = float2(i.uv.x, i.uv.y + gx0 * _GrabTexture_TexelSize.y);
				float2 uv1 = float2(i.uv.x, i.uv.y + gx1 * _GrabTexture_TexelSize.y);
				float2 uv2 = float2(i.uv.x, i.uv.y + gx2 * _GrabTexture_TexelSize.y);
#if 1
				if ((_ProjectionParams.x < 0) != (_GrabTexture_TexelSize.y < 0))
				{
					uv0.y = 1 - uv0.y;
					uv1.y = 1 - uv1.y;
					uv2.y = 1 - uv2.y;
				}
#endif
				half4 c0 = tex2D(_GrabTexture, uv0) * gs0;
				half4 c1 = tex2D(_GrabTexture, uv1) * gs1;
				half4 c2 = tex2D(_GrabTexture, uv2) * gs2;
				half4 color = c0 + c2 + c1;
				return color;
			}
			ENDCG
		}
	}
}
