Shader "Hidden/Rcam2/Demux"
{
    Properties { _MainTex("", 2D) = "black"{} }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #define RCAM_DEMUX_COLOR
            #include "Demux.hlsl"
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
        }
        Pass
        {
            HLSLPROGRAM
            #define RCAM_DEMUX_DEPTH
            #include "Demux.hlsl"
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDHLSL
       }
    }
}
