#include "UnityCG.cginc"

// Uniforms from AR Foundation
sampler2D _textureY;
sampler2D _textureCbCr;
sampler2D _HumanStencil;
sampler2D _EnvironmentDepth;
float4x4 _UnityDisplayTransform;

// Rcam parameters
float2 _DepthRange;
float _AspectFix;

// Hue encoding
float3 Hue2RGB(float hue)
{
    float h = hue * 6 - 2;
    float r = abs(h - 1) - 1;
    float g = 2 - abs(h);
    float b = 2 - abs(h - 2);
    return saturate(float3(r, g, b));
}

// yCbCr decoding
float3 YCbCrToSRGB(float y, float2 cbcr)
{
    float b = y + cbcr.x * 1.772 - 0.886;
    float r = y + cbcr.y * 1.402 - 0.701;
    float g = y + dot(cbcr, float2(-0.3441, -0.7141)) + 0.5291;
    return float3(r, g, b);
}

// Common vertex shader
void Vertex(float4 vertex : POSITION,
            float2 texCoord : TEXCOORD,
            out float4 outVertex : SV_Position,
            out float2 outTexCoord : TEXCOORD)
{
    outVertex = UnityObjectToClipPos(vertex);
    outTexCoord = texCoord;
}

// Fragment shader
float4 Fragment(float4 vertex : SV_Position,
                float2 texCoord : TEXCOORD) : SV_Target
{
#ifdef RCAM_MULTIPLEXER

    float4 tc = frac(texCoord.xyxy * float4(1, 1, 2, 2));

    // Aspect ration compensation & vertical flip
    tc.yw = (0.5 - tc.yw) * _AspectFix + 0.5;

    // Texture samples
    float y = tex2D(_textureY, tc.zy).x;
    float2 cbcr = tex2D(_textureCbCr, tc.zy).xy;
    float mask = tex2D(_HumanStencil, tc.zw).x;
    float depth = tex2D(_EnvironmentDepth, tc.zw).x;

    // Color plane
    float3 c1 = YCbCrToSRGB(y, cbcr);

    // Depth plane
    depth = (depth - _DepthRange.x) / (_DepthRange.y - _DepthRange.x);
    float3 c2 = Hue2RGB(clamp(depth, 0, 0.8));

    // Mask plane
    float3 c3 = mask;

    // Output
    float3 srgb = tc.x < 0.5 ? c1 : (tc.y < 0.5 ? c2 : c3);
    return float4(GammaToLinearSpace(srgb), 1);

#endif

#ifdef RCAM_MONITOR

    // Texture transform
    float2 uv = mul(float3(texCoord, 1), _UnityDisplayTransform).xy;

    // Texture samples
    float y = tex2D(_textureY, uv).x;
    float2 cbcr = tex2D(_textureCbCr, uv).xy;
    float mask = tex2D(_HumanStencil, uv).x;
    float depth = tex2D(_EnvironmentDepth, uv).x;

    // Color
    float3 srgb = YCbCrToSRGB(y, cbcr);

    // Composite stencil/depth
    srgb = lerp(srgb, float3(y, 0, 0), saturate(depth / 2));
    srgb = lerp(float3(0.2, 0.2, 0.5) * y, srgb, mask);

    // Letterboxing with 16:9
    srgb *= abs(uv.y - 0.5) * 2 < _AspectFix ? 1 : 0.5;

    // Output
    return float4(GammaToLinearSpace(srgb), 1);

#endif
}
