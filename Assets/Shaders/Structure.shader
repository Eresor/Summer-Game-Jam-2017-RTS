// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tutorial/VertFrag"
{
	Properties	//należy zdefiniować zmienne o analogicznym typie i nazwie
	{
		_Color("Color", Color) = (0,0,0,1)
		_Value("Value", float) = 1
		_MyTexture("My texture", 2D) = "white" {}	//white, black, gray
		_NormalMap("Normal map", 2D) = "bump" {}	// automatycznie normal mapa, #808080 
	}

	SubShader
	{
		//Common state
		Lighting On //Off

		//Tags
		Tags{ "Tag1" = "Value1" "Tag2" = "Value2" }
		/*
			Queue: Background 
			Geometry (domyślna) również "Geometry+1"
			AlphaTest 
			Transparent (back-to-front order; szkło, cząsteczki)
			Overlay 

			RenderType:		//"typy", wykorzystywane np. przy Shader Replacement
			Opaque
			Transparent
			TransparentCutout
			Background
			Overlay
			...
		*/

		UsePass "Shader/PASS_NAME"	//uppercase

		GrabPass
		{
			"_TargetTexture"	// trzeba zdefiniować zmienną sampler2D o analogicznej nazwie
		}

		Pass
		{
			Name "PASS_NAME"	//nie wymagane
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			half4 _Color;	//fixed, half, float
			float _Value;
			sampler2D _MyTexture;
			sampler2D _NormalMap;

			struct vertInput {
				float4 pos : POSITION;
				float3 norm : NORMAL;
				float4 color : COLOR;
				float2 myValue : TEXCOORD0;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
				float someValueToPass : TEXCOORD;	//wymagane jest podanie "sposobu interpolacji, ", TEXCOORD, COLOR
				float4 otherValueToPass : COLOR;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				/*
				UNITY_MATRIX_MVP
				_Object2World
				_World2Object
				_WorldSpaceCameraPos
				_Time[0-3]
				_SinTime[]
				_CosTime[]

				*/
				o.someValueToPass = 1;
				o.otherValueToPass = input.color;
				return o;
			}

			half4 frag(vertOutput output) : COLOR{	//również float4 fixed4
				return half4(1.0, 0.0, 0.0, 1.0);
			}

			
			ENDCG

			
		}

	}

	//po wszystkich SubShaderach
	Fallback "Diffuse"

}