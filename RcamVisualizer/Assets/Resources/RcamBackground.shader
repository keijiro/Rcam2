Shader "FullScreen/RcamBackground"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    sampler2D _ColorTexture;
    sampler2D _DepthTexture;
    float4 _ProjectionVector;
    float4x4 _InverseViewMatrix;

    // Linear distance to Z depth
    float DistanceToDepth(float d)
    {
        return d < _ProjectionParams.y ? 0 :
          (0.5 / _ZBufferParams.z * (1 / d - _ZBufferParams.w));
    }

    // Inversion projection into the world space
    float3 DistanceToWorldPosition(float2 uv, float d)
    {
        float3 p = float3((uv - 0.5) * 2, -1);
        p.xy += _ProjectionVector.xy;
        p.xy /= _ProjectionVector.zw;
        return mul(_InverseViewMatrix, float4(p * d, 1)).xyz;
    }

    void FullScreenPass(Varyings varyings,
                        out float4 outColor : SV_Target,
                        out float outDepth : SV_Depth)
    {
        // Calculate the UV coordinates from varyings
        float2 uv =
          (varyings.positionCS.xy + float2(0.5, 0.5)) * _ScreenSize.zw;

        // Color/depth samples
        float4 c = tex2D(_ColorTexture, uv);
        float d = tex2D(_DepthTexture, uv).x;

        // Inverse projection
        float3 p = DistanceToWorldPosition(uv, d);

        // Gridlines
        float3 grid = smoothstep(0.49, 0.5, abs(0.5 - frac(p * 5)));

        // Output
        outColor = float4(max(c.rgb, grid), c.a);
        outDepth = DistanceToDepth(d);
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
