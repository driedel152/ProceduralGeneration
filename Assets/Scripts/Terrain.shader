Shader "Custom/Terrain"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
            float3 worldPos;
        };

        float inverseLerp(float a, float b, float value) {
            return saturate((value - a) / (b - a));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float maxHeight = 10;
            float minHeight = -10;
            float red = inverseLerp(maxHeight, minHeight, IN.worldPos.y);
            float green = inverseLerp(maxHeight, minHeight, IN.worldPos.x);
            float blue = inverseLerp(maxHeight, minHeight, IN.worldPos.z);
            o.Albedo = float3(red, green, blue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
