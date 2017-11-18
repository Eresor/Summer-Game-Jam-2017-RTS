Shader "Custom/BlackHoleFresnel" {
	Properties {
		_InsideColor ("Inside Color", Color) = (1,1,1,1)
		_OutsideColor("Outisde Color", Color) = (0,0,0,1)
		_Bias ("Bias", float ) = 1
		_Scale ("Scale", float) = 1
		_Power ("Power", float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float r;
		};

		fixed4 _InsideColor;
		fixed4 _OutsideColor;
		float _Bias;
		float _Scale;
		float _Power;

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float3 viewDir = normalize(mul(unity_WorldToObject, worldPos - _WorldSpaceCameraPos.xyz));
			o.r = _Bias + _Scale * pow(1.0 + dot(v.normal,viewDir), _Power);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = lerp(_InsideColor, _OutsideColor, IN.r).rgb;
			o.Emission = o.Albedo = lerp(_InsideColor, _OutsideColor, IN.r).rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
