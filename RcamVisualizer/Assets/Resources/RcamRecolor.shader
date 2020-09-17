Shader "FullScreen/RcamRecolor"
{
    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            #include "RcamRecolor.hlsl"
            ENDHLSL
        }
    }
    Fallback Off
}
