// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Distortion" 
{
	SubShader
	{
		// Draw ourselves after all opaque geometry
		Tags{ "Queue" = "Transparent" }

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		// Render the object with the texture generated above, and invert the colors
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 grabPos : TEXCOORD0;
				float4 pos : SV_POSITION;
				half4 scale : TEXCOORD1;
			};

			float rand(float3 co)
			{
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
			}

			v2f vert(appdata_base v) {
				v2f o;
				// use UnityObjectToClipPos from UnityCG.cginc to calculate 
				// the clip-space of the vertex
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 viewDir = normalize(mul(unity_WorldToObject, _WorldSpaceCameraPos.xyz - worldPos));
				float scale = dot(v.normal, viewDir);

				o.scale = half4(scale, scale, scale, 1);

				float3 dir = cross(v.normal, viewDir);

				o.pos = UnityObjectToClipPos(v.vertex + rand(v.normal)*0.01+0.05*v.normal*pow(sin((rand(v.normal)+_Time[1])), 2));
				float4 ssd = UnityObjectToClipPos(float4(dir, 1));
				// use ComputeGrabScreenPos function from UnityCG.cginc
				// to get the correct texture coordinate
				o.grabPos = ComputeGrabScreenPos(lerp(o.pos,o.pos+ssd,sin(1 - scale)));
				return o;
			}

			sampler2D _BackgroundTexture;

			half4 frag(v2f i) : SV_Target
			{
				half4 bgcolor = tex2Dproj(_BackgroundTexture, i.grabPos);
				return bgcolor;
			}
			ENDCG
		}

	}
}