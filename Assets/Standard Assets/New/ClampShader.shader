Shader "Custom/ClampShader" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}		
	}

	SubShader{
		Pass{
			ZTest Always Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			half _Cutoff;

			fixed4 frag(v2f_img i) : SV_Target
			{
				fixed4 orig = tex2D(_MainTex, i.uv);
				fixed4 color = clamp(orig, 0, _Cutoff);

				return color;
			}
			ENDCG
		}
	}
	Fallback off
}