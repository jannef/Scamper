﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Sprites/Partial-Beam-Mask"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_WorldX("World X", Float) = 0
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
	float _WorldX;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
		OUT.color = IN.color * _Color;
		OUT.texcoord = IN.texcoord;
		OUT.worldpos = mul(unity_ObjectToWorld, IN.vertex).xy;

#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;

	fixed4 frag(v2f IN) : SV_Target
	{
		fixed4 c = tex2D(_MainTex, IN.texcoord);
	if (IN.texcoord[0] > _WorldX)
	{
		c.rgba = 0;
	}
		return c;
	}
		ENDCG
	}
	}
}