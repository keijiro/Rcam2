#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"
#include "Packages/jp.keijiro.klak.lineargradient/Shader/LinearGradient.hlsl"
#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

sampler2D _ColorTexture;
sampler2D _DepthTexture;
float4 _ProjectionVector;
float4x4 _InverseViewMatrix;

float _BGOpacity;
LinearGradient _EffectGradient;
float2 _EffectParams;

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

    float lm = Luminance(FastLinearToSRGB(c.rgb));

#if defined(RCAM_FX0)

    // Noise slits
    float3 np1 = float3(p.y * 16, 0, _Time.y);
    float3 np2 = float3(p.y * 32, 0, _Time.y * 2) * 0.8;
    float pt = (lm - 0.5) + snoise(np1) + snoise(np2);
    float3 fill = abs(pt) < _EffectParams.x;
    fill *= 1 + _EffectParams.y * 4;

#endif

#if defined(RCAM_FX1)

    // Marble-like gradient
    float freq = lerp(2.75, 20, _EffectParams.x);
    float3 np = p * float3(1.2, freq, 1.2);
    np += float3(0.024, 0.084, 0.745) * _Time.y;
    float pt = 0.5 + (lm - 0.5) * 0.4 + snoise(np) * 0.7;
    float3 fill = SampleLinearGradientColor(_EffectGradient, pt);
    fill = FastSRGBToLinear(fill);
    fill *= 1 + _EffectParams.y * 4;

#endif

#if defined(RCAM_FX2) || defined(RCAM_FX3)

    // Slicer seed
    float freq = lerp(10, 100, _EffectParams.x);
    uint seed1 = floor(p.y * freq + 200) * 2;
    float width = lerp(0.5, 2, Hash(seed1 + 0));
    float speed = lerp(1.0, 5, Hash(seed1 + 1));
    float pt = (p.x + 100 + _Time.y * speed) * width;
    uint seed2 = (uint)floor(pt) * 0x87893u;

#endif

#if defined(RCAM_FX2)

    // Slicer color
    float pr = frac(Hash(seed2) + (lm - 0.5) * 0.2);
    float3 fill = SampleLinearGradientColor(_EffectGradient, pr);
    fill = FastSRGBToLinear(fill);

#endif

#if defined(RCAM_FX3)

    // Slicer displacement
    float2 disp = float2(Hash(seed2), Hash(seed2 + 1)) - 0.5;
    float3 fill = tex2D(_ColorTexture, frac(uv + disp * 0.1)).rgb;

#endif

#if defined(RCAM_FX2) || defined(RCAM_FX3)

    // Slicer emission
    fill *= 1 + (Hash(seed2 + 2) < _EffectParams.y) * 10;

#endif

#if !defined(RCAM_NOFX)

    c.rgb = lerp(c.rgb, fill, c.a);

#endif

    // BG opacity
    float3 bg = FastSRGBToLinear(FastLinearToSRGB(c.rgb) * _BGOpacity);
    c.rgb = lerp(bg, c.rgb, c.a);

    // Depth mask
    bool mask = c.a > 0.5 || _BGOpacity > 0;

    // Output
    outColor = c;
    outDepth = DistanceToDepth(d) * mask;
}
