// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Sprites/Custom-Jannef-2"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1, 1, 1, 1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_Fade("Fade", Float) = 1
		_WorldX("World X", Float) = 0
		_WorldY("World Y", Float) = 0
		_TileX("Tile X", Float) = 1
		_TileY("Tile Y", Float) = 1
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
	fixed _Fade;
	float _WorldX;
	float _WorldY;
	float _TileX;
	float _TileY;

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
		IN.texcoord.x = (IN.texcoord.x % (1 / _TileX));
		IN.texcoord.y = (IN.texcoord.y % (1 / _TileY));

		fixed4 c = tex2D(_MainTex, (frac(IN.texcoord) * float2(_TileX, _TileY))) * IN.color;
		c.a *= _Fade;
		c.rgb *= c.a;

		float2 l = { _WorldX, _WorldY };
		float dist = distance(IN.worldpos, l) / 8;
		float dist_norm = max(0.25, min(dist, 1));

		c.rgb -= (dist / 4);

		float L = (0.3 * c.r) + (0.6 * c.g) + (0.1 * c.b);
		c.r += dist_norm * (L - c.r);
		c.g += dist_norm * (L - c.g);
		c.b += dist_norm * (L - c.b);

		return c;
	}
		ENDCG
	}
	}
}