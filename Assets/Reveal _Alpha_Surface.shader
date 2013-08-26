Shader "GUI/Reveal _Alpha_Surface"
{
	Properties {
	  _MainTex ("Sprite Texture", 2D) = "white" {}
	  _Mask ("Mix Mask (A)", 2D) = "white" {}
	}
	SubShader {
        Tags { 
        	"Queue" = "Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }
        Cull Off
        ZWrite Off // don't write to depth buffer in order not to occlude other objects
	    Lighting Off
	    Blend One OneMinusSrcAlpha
	      CGPROGRAM
		      #pragma surface surf SpritesModel
		      half4 LightingSpritesModel (SurfaceOutput s, half3 lightDir, half atten) {
		          half4 c;
		          c.rgb = s.Albedo;
		          c.a = s.Alpha;
		          return c;
		      }
		      
		      struct Input {
		          float2 uv_MainTex; 
		          float2 uv_Mask;
		      };
		      
		      sampler2D _MainTex;
		      sampler2D _Mask;
		      void surf (Input IN, inout SurfaceOutput o) {
		      	  half maskAlpha = tex2D(_Mask, IN.uv_Mask).a;
		          half4 col = tex2D (_MainTex, IN.uv_MainTex);
		          
		          o.Albedo = col.rgb * maskAlpha; //Let's premultiply the color channel.
		          o.Alpha = maskAlpha;
		      }
	      ENDCG
    } 
    Fallback "Diffuse"
}