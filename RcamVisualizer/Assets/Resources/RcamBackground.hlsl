#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"
#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

sampler2D _ColorTexture;
sampler2D _DepthTexture;
float4 _ProjectionVector;
float4x4 _InverseViewMatrix;
float _DepthOffset;

float2 _Opacity; // Background, Effect
float4 _EffectParams; // param, intensity, sin(r), cos(r)

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

// Foreground effect
float3 ForegroundEffect(float3 wpos, float2 uv, float luma)
{
#if defined(RCAM_FX0)

    // Animated zebra

    // Noise field positions
    float3 np1 = float3(wpos.y * 16, 0, _Time.y);
    float3 np2 = float3(wpos.y * 32, 0, _Time.y * 2) * 0.8;

    // Potential value
    float pt = (luma - 0.5) + snoise(np1) + snoise(np2);

    // Grayscale
    float gray = abs(pt) < _EffectParams.x + 0.02;

    // Emission
    float em = _EffectParams.y * 4;

    // Output
    return gray * (1 + em);

#endif

#if defined(RCAM_FX1)

    // Marble-like pattern

    // Frequency
    float freq = lerp(2.75, 20, _EffectParams.x);

    // Noise field position
    float3 np = wpos * float3(1.2, freq, 1.2);
    np += float3(0, -0.784, 0) * _Time.y;

    // Potential value
    float pt = 0.5 + (luma - 0.5) * 0.4 + snoise(np) * 0.7;

    // Random seed
    uint seed = (uint)(pt * 5 + _Time.y * 5) * 2;

    // Color
    float3 rgb = FastSRGBToLinear(HsvToRgb(float3(Hash(seed), 1, 1)));

    // Emission
    float em = Hash(seed + 1) < _EffectParams.y * 0.5;

    // Output
    return rgb * (1 + em * 8) + em;

#endif

#if defined(RCAM_FX2)

    // Slicer seed calculation

    // Slice frequency (1/height)
    float freq = 60;

    // Per-slice random seed
    uint seed1 = floor(wpos.y * freq + 200) * 2;

    // Random slice width
    float width = lerp(0.5, 2, Hash(seed1));

    // Random slice speed
    float speed = lerp(1.0, 5, Hash(seed1 + 1));

    // Effect direction
    float3 dir = float3(_EffectParams.z, 0, _EffectParams.w);

    // Potential value (scrolling strips)
    float pt = (dot(wpos, dir) + 100 + _Time.y * speed) * width;

    // Per-strip random seed
    uint seed2 = (uint)floor(pt) * 0x87893u;

    // Color mapping with per-strip UV displacement
    float2 disp = float2(Hash(seed2), Hash(seed2 + 1)) - 0.5;
    float3 cm = tex2D(_ColorTexture, frac(uv + disp * 0.1)).rgb;

    // Per-strip random color
    float3 cr = HsvToRgb(float3(Hash(seed2 + 2), 1, 1));

    // Color selection (color map -> random color -> black)
    float sel = Hash(seed2 + 3);
    float3 rgb = sel < _EffectParams.x * 2 ? cr : cm;
    rgb = sel < _EffectParams.x * 2 - 1 ? 0 : rgb;

    // Emission
    float3 em = Hash(seed2 + 4) < _EffectParams.y * 0.5;

    // Output
    return rgb * (1 + em * 8) + em;

#endif

#if defined(RCAM_NOFX)

    return 0;

#endif
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

#if !defined(RCAM_NOFX)

    // Source pixel luma value
    float lum = Luminance(FastLinearToSRGB(c.rgb));

    // Foreground effect
    float3 eff = ForegroundEffect(p, uv, lum);
    c.rgb = lerp(c.rgb, eff, c.a * _Opacity.y);

#endif

    // BG opacity
    float3 bg = FastSRGBToLinear(FastLinearToSRGB(c.rgb) * _Opacity.x);
    c.rgb = lerp(bg, c.rgb, c.a);

    // Depth mask
    bool mask = c.a > 0.5 || _Opacity.x > 0;

    // Output
    outColor = c;
    outDepth = DistanceToDepth(d) * mask + _DepthOffset;
}
