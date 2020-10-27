#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise2D.hlsl"

void Iris_float(float2 uv, float seed, out float4 output)
{
    float t = _Time.y;
    float2 p = (uv - 0.5) / 0.27;
    float l = length(p);
    float phi = atan2(p.x, p.y) / (PI * 2) + 0.5;

    // Random color
    float hue = frac(seed + t * 0.1 + l * 0.2);
    float3 c1 = SRGBToLinear(HsvToRgb(float3(hue, 0.4, 1)));
    float3 c2 = 0;
    float3 c3 = SRGBToLinear(HsvToRgb(float3(hue, 0.9, 0.8)));

    // Core radius
    float core_r = 0.35 + 0.1 * sin(t * 4);

    // Black region (as black-white parameter)
    float bw = smoothstep(0, 0.01, l - core_r);
    bw = bw - smoothstep(0.8, 1.1, l);

    // Noise 1
    float n1 = snoise(float2(phi * 30, l - t)) + 0.5;
    n1 *= 1 - smoothstep(0, 0.5, abs(l - 0.5));

    // Noise 2
    float n2 = snoise(float2(phi * 50, l - t * 1.3)) + 0.5;
    float l_n2 = (l - core_r) / (1 - core_r);
    n2 *= 1 - smoothstep(0, 0.6, abs(l_n2 - 0.5));

    // Color blending
    float3 nc = lerp(lerp(c1, c2, saturate(n1)), c3, saturate(n2)) * bw;

    output = float4(nc, 1);
}
