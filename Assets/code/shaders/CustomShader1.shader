﻿Shader "Sprites/Custom-Jannef-1"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Fade("Fade", Float) = 1
		_FadeColor("Fade Tint", Color) = (1, 1, 1, 1)
		_WorldX("World X", Float) = 0
		_WorldY("World Y", Float) = 0
	}

	SubShader
	{
	
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog{ Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 worldpos : TEXCOORD1;
			};

			fixed4 _Color;
			fixed4 _FadeColor;
			fixed _Fade;
			float _WorldX;
			float _WorldY;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.color = IN.color * _Color;
				OUT.texcoord = IN.texcoord;
				OUT.worldpos = mul(_Object2World, IN.vertex).xy;

				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
				c.rgb *= c.a;

				float2 l = { _WorldX, _WorldY };
				float dist = distance(IN.worldpos, l) / 6;
				float dist_norm = min(dist, 1);
				
				c.rgb -= (dist / 4);

				float L = (0.3 * c.r) + (0.6 * c.g) + (0.1 * c.b);
				c.r += dist_norm * (L - c.r);
				c.g += dist_norm * (L - c.r);
				c.b += dist_norm * (L - c.r);
				
				//c.rgb = lerp(c.rgb, monochrome.xyz, pow((distance(IN.worldpos, l) / 8), 2));

				return c;
			}
			ENDCG
		}
	}
}