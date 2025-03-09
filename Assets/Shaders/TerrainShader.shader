Shader "Custom/TerrainShader"
{
    Properties {
        waterTexture("Water Tex", 2D) = "white"{}
        waterTextureScale("Scale", float) = 1

        sandTexture("Sand Tex", 2D) = "yellow"{}
        sandTextureScale("Scale", float) = 1

        dirtTexture("Dirt Tex", 2D) = "brown"{}
        dirtTextureScale("Scale", float) = 1

        grassTexture("Grass Tex", 2D) = "green"{}
        grassTextureScale("Scale", float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        float minHeight;
        float maxHeight;

        sampler2D waterTexture;
        sampler2D sandTexture;
        sampler2D dirtTexture;
        sampler2D grassTexture;

        float waterTextureScale;
        float sandTextureScale;
        float dirtTextureScale;
        float grassTextureScale;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        float inverseLerp(float min, float max, float value) {
            return saturate((value - min) / (max - min));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 scaledWorldPos = IN.worldPos / waterTextureScale;
            float3 blendedAxis = abs(IN.worldNormal);
            blendedAxis /= blendedAxis.x + blendedAxis.y + blendedAxis.z;

            float3 xProjection = tex2D(waterTexture, scaledWorldPos.yz) * blendedAxis.x;
            float3 yProjection = tex2D(waterTexture, scaledWorldPos.xz) * blendedAxis.y;
            float3 zProjection = tex2D(waterTexture, scaledWorldPos.xy) * blendedAxis.z;
            o.Albedo = xProjection + yProjection + zProjection;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
