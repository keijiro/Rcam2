#include "Packages/jp.keijiro.noiseshader/Shader/SimplexNoise3D.hlsl"

void TextUVNoise_float
  (float2 uv, float frequency, float speed, float amplitude,
   out float2 output)
{
    float3 np = float3(uv * frequency, _Time.y * speed);
    output = snoise_grad(np).xy * amplitude;
}
