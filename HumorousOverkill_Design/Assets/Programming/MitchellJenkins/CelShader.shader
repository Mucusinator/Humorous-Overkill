Shader "CelShader"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_Threshold("Threshold", Range(1, 20)) = 5
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		#pragma surface surf ToonRamp fullforwardshadows

		sampler2D _MainTex;
		float4 _Color;
		float _Threshold;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir) * 0.5 + 0.5;
		float ramp = floor(d * _Threshold) / _Threshold;


		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}

	struct Input
	{
		float2 uv_MainTex : TEXCOORD0;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

	}

		Fallback "Diffuse"
}
