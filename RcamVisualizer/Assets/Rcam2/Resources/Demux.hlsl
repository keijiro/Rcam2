sampler2D _MainTex;

void Vertex(float4 vertex : POSITION,
            float2 texCoord : TEXCOORD,
            out float4 outVertex : SV_Position,
            out float2 outTexCoord : TEXCOORD)
{
    outVertex = float4(vertex.x * 2 - 1, 1 - vertex.y * 2, 1, 1);
    outTexCoord = texCoord;
}

#ifdef RCAM_DEMUX_COLOR

float4 Fragment(float4 vertex : SV_Position,
                float2 texCoord : TEXCOORD0) : SV_Target
{
    return tex2D(_MainTex, texCoord * float2(0.5, 1));
}

#endif

#ifdef RCAM_DEMUX_DEPTH_MASK

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

float2 _DepthRange;

// Hue value calculation
float RGB2Hue(float3 c)
{
    float minc = min(min(c.r, c.g), c.b);
    float maxc = max(max(c.r, c.g), c.b);
    float div = 1 / (6 * max(maxc - minc, 1e-5));
    float r = (c.g - c.b) * div;
    float g = 1.0 / 3 + (c.b - c.r) * div;
    float b = 2.0 / 3 + (c.r - c.g) * div;
    return lerp(r, lerp(g, b, c.g < c.b), c.r < max(c.g, c.b));
}

// Depth calculation
float RGB2Depth(float3 rgb)
{
    float hue = RGB2Hue(LinearToSRGB(rgb));
    return lerp(_DepthRange.x, _DepthRange.y, hue);
}

void Fragment(float4 vertex : SV_Position,
              float2 texCoord : TEXCOORD,
              out float4 depth : SV_Target0,
              out float4 mask : SV_Target1)
{
    texCoord /= 2;
    depth = RGB2Depth(tex2D(_MainTex, texCoord + float2(0.5, 0.5)).xyz);
    mask = tex2D(_MainTex, texCoord + float2(0.5, 0)).x;
}

#endif
