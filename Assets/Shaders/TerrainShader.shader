Shader "Custom/TerrainShader"
{
    Properties {
        waterTexture("Water Tex", 2D) = "white"{}
        sandTexture("Sand Tex", 2D) = "yellow"{}
        dirtTexture("Dirt Tex", 2D) = "brown"{}
        grassTexture("Grass Tex", 2D) = "green"{}
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

        const static float epsilon = 1E-4;

        float minHeight;
        float maxHeight;

        float baseStartHeights[4];
        float baseBlends[4];
        float texScales[4];

        sampler2D waterTexture;
        sampler2D sandTexture;
        sampler2D dirtTexture;
        sampler2D grassTexture;

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        float inverseLerp(float min, float max, float value) {
            return saturate((value - min) / (max - min));
        }

        float3 triplanar(float3 worldPos, float scale, float3 blendedAxis, int selectTex) {
            
            float3 scaledWorldPos = worldPos / scale;
            float3 xProjection;
            float3 yProjection;
            float3 zProjection;
            
            switch(selectTex) {
                case 1:
                xProjection = tex2D(sandTexture, scaledWorldPos.yz) * blendedAxis.x;
                yProjection = tex2D(sandTexture, scaledWorldPos.xz) * blendedAxis.y;
                zProjection = tex2D(sandTexture, scaledWorldPos.xy) * blendedAxis.z;
                break;

                case 2:
                xProjection = tex2D(dirtTexture, scaledWorldPos.yz) * blendedAxis.x;
                yProjection = tex2D(dirtTexture, scaledWorldPos.xz) * blendedAxis.y;
                zProjection = tex2D(dirtTexture, scaledWorldPos.xy) * blendedAxis.z;
                break;

                case 3:
                xProjection = tex2D(grassTexture, scaledWorldPos.yz) * blendedAxis.x;
                yProjection = tex2D(grassTexture, scaledWorldPos.xz) * blendedAxis.y;
                zProjection = tex2D(grassTexture, scaledWorldPos.xy) * blendedAxis.z;
                break;

                default:
                xProjection = tex2D(waterTexture, scaledWorldPos.yz) * blendedAxis.x;
                yProjection = tex2D(waterTexture, scaledWorldPos.xz) * blendedAxis.y;
                zProjection = tex2D(waterTexture, scaledWorldPos.xy) * blendedAxis.z;
                break;
            }
            
            return xProjection + yProjection + zProjection;
        }

        void surf (Input IN, inout SurfaceOutputStandard o) {

            float heightPercentage = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            float3 blendedAxis = abs(IN.worldNormal);
            blendedAxis /= blendedAxis.x + blendedAxis.y + blendedAxis.z;

            for(int i = 0; i < 4; i++) {
                float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon, baseBlends[i]/2, heightPercentage - baseStartHeights[i]);
                float3 texApplicator = triplanar(IN.worldPos, texScales[i], blendedAxis, i);
                o.Albedo = o.Albedo * (1 - drawStrength) + texApplicator * drawStrength;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
