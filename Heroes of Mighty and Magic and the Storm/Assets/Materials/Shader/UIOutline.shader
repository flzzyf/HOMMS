// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnityCommunity/UIOutline"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
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
			ZTest[unity_GUIZTestMode]
			ZWrite Off
			Blend One OneMinusSrcAlpha

			//投射阴影
			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
				};

				fixed4 _Color;

				bool _UseClipRect;
				bool _UseAlphaClip;
				float4 _ClipRect;

				v2f vert(appdata_t IN)
				{
					v2f OUT;

					OUT.worldPosition = IN.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color *_Color;

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float _AlphaSplitEnabled;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);
					color.rgb = _Color.rgb;

					return color;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
					c.rgb *= c.a;

					if (_UseClipRect)
						c *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

					if (_UseAlphaClip)
						clip(c.a - 0.001);

					return c;
				}
			ENDCG
			}
		}
}