Shader "PSX/Lite/Unlit Cutout"
{
    Properties
    {
        _Color("Color (RGBA)", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _Cutoff("Alpha cutoff", Range(0,1)) = 0.1
    }
        SubShader
    {
        Tags {"RenderType" = "Opaque" }
        ZWrite On
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "PSX-Utils.cginc"

            #define PSX_CUTOUT_VAL _Cutoff
            float _Cutoff;
            #include "PSX-ShaderSrc-Lite.cginc"

            ENDCG
        }
    }
        Fallback "Unlit/Color"
}