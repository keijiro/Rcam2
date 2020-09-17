Shader "FullScreen/RcamRecolor"
{
    HLSLINCLUDE

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"
#include "Packages/jp.keijiro.klak.lineargradient/Shader/LinearGradient.hlsl"

float4 _LineColor;
float2 _LineParams;
float _DitherStrength;

LinearGradient _BackGradient;
LinearGradient _FrontGradient;

float4 SampleColorSRGB(uint2 pcs)
{
    float4 cs = LOAD_TEXTURE2D_X_LOD(_ColorPyramidTexture, pcs, 0);
    return FastLinearToSRGB(cs);
}

float4 FullScreenPass(Varyings varyings) : SV_Target
{
    // Clip space position
    uint2 pcs = varyings.positionCS.xy;
    uint2 pcsm4 = pcs % 4;

    // Color samples in sRGB
    float4 c0 = SampleColorSRGB(pcs + uint2(0, 0));
    float4 c1 = SampleColorSRGB(pcs + uint2(1, 1));
    float4 c2 = SampleColorSRGB(pcs + uint2(1, 0));
    float4 c3 = SampleColorSRGB(pcs + uint2(0, 1));

    // Edge detection (Roberts cross operator)
    float3 g1 = (c1.rgb) - (c0.rgb);
    float3 g2 = (c3.rgb) - (c2.rgb);
    float g = sqrt(dot(g1, g1) + dot(g2, g2));
    g = saturate((g - _LineParams.x) * _LineParams.y);

    // Bayer dither matrix
    const float4x4 bayer =
      float4x4(0, 8, 2, 10, 12, 4, 14, 6, 3, 11, 1, 9, 15, 7, 13, 5) / 16;

    float dither = (bayer[pcsm4.x][pcsm4.y] - 0.5) * _DitherStrength;

    float lm = Luminance(c0.rgb) + dither;
    float3 bg = SampleLinearGradientColor(_BackGradient, lm);
    float3 fg = SampleLinearGradientColor(_FrontGradient, lm);
    fg = lerp(fg, _LineColor.rgb, g);

    return float4(FastSRGBToLinear(lerp(bg, fg, c0.a)), 1);
}

    ENDHLSL

    SubShader
    {
        Pass
        {
            Cull Off ZWrite Off ZTest Always
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
