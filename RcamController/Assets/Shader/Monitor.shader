Shader "Rcam2/Monitor"
{
    Properties
    {
        _MainTex ("", 2D) = "black" {}
        _textureY ("", 2D) = "black" {}
        _textureCbCr ("", 2D) = "black" {}
        _HumanStencil ("", 2D) = "black" {}
        _EnvironmentDepth ("", 2D) = "black" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _textureY;
    sampler2D _textureCbCr;
    sampler2D _HumanStencil;
    sampler2D _EnvironmentDepth;

    float3 YCbCrToSRGB(float y, float2 cbcr)
    {
        float b = y + cbcr.x * 1.772 - 0.886;
        float r = y + cbcr.y * 1.402 - 0.701;
        float g = y + dot(cbcr, float2(-0.3441, -0.7141)) + 0.5291;
        return float3(r, g, b);
    }

    void Vertex(float4 vertex : POSITION,
                float2 texCoord : TEXCOORD,
                out float4 outVertex : SV_Position,
                out float2 outTexCoord : TEXCOORD)
    {
        outVertex = UnityObjectToClipPos(vertex);
        outTexCoord = float2(texCoord.x, 1 - texCoord.y);
    }

    float4 Fragment(float4 vertex : SV_Position,
                    float2 texCoord : TEXCOORD) : SV_Target
    {
        float y = tex2D(_textureY, texCoord).x;
        float2 cbcr = tex2D(_textureCbCr, texCoord).xy;
        float mask = tex2D(_HumanStencil, texCoord).x;
        float depth = tex2D(_EnvironmentDepth, texCoord).x;

        float3 srgb = YCbCrToSRGB(y, cbcr);

        srgb = lerp(srgb, float3(y, 0, 0), saturate(depth / 2));
        srgb = lerp(float3(0.2, 0.2, 0.5) * y, srgb, mask);

        return float4(GammaToLinearSpace(srgb), 1);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            Cull Off ZTest Always ZWrite Off
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
