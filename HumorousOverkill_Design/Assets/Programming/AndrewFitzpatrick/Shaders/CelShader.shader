Shader "CelShader"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Threshold("Threshold", Range(1, 20)) = 5
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
		_Ambient("Ambient Lighting", Range(0, 1)) = 0
	}
	SubShader
		{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CelShading fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		float _Threshold;
		float _Cutoff;
		float _Ambient;

#pragma lighting CelShading exclude_path:prepass
		inline half4 LightingCelShading(SurfaceOutput s, half3 lightDir, half atten)
		{
#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
#endif
			// take the normal of the light direction and the surface normal (lambert lighting)
			half4 ndotl = (dot(s.Normal, lightDir) + 1) * 0.5;

			ndotl += _Ambient;

			// clamp ndotl to create Cel Shading "cuts"
			float ramp = floor(ndotl * _Threshold) / _Threshold;

			// apply color
			half4 color;
			color.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			color.a = 0;
			return color;
		}


		struct Input
		{
			float2 uv_MainTex : TEXCOORD0;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o)
		{
			// sample texture / color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
