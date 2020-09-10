Shader "FullScreen/RcamBackground"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    sampler2D _ColorTexture;
    sampler2D _DepthTexture;

    // Linear distance to Z depth
    float DistanceToDepth(float d)
    {
        return d < _ProjectionParams.y ? 0 :
          (0.5 / _ZBufferParams.z * (1 / d - _ZBufferParams.w));
    }

    void FullScreenPass(Varyings varyings,
                        out float4 outColor : SV_Target,
                        out float outDepth : SV_Depth)
    {
        // Calculate the UV coordinates from varyings
        float2 uv =
          (varyings.positionCS.xy + float2(0.5, 0.5)) * _ScreenSize.zw;

        // Output
        outColor = tex2D(_ColorTexture, uv);
        outDepth = DistanceToDepth(tex2D(_DepthTexture, uv).x);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Cull Off ZWrite On ZTest Less
            HLSLPROGRAM
            #pragma target 4.5
            #pragma only_renderers d3d11
            #pragma vertex Vert
            #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
