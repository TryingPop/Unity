// 림라이트
// 피사체 뒤에서 강한 조명을 주어 피사체의 위나 측면 모서리등,
// 외각의 실루엣을 따라 빛의 테(Rim)을 보여주는게 RimLight
// 즉, 외각선이 빛나게 만드는거
Shader "Custom/Test/017"
{
    Properties
    {

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test017
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            // float2 uv_MainTex;
            // 카메라 벡터를 불러온다
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float rim = dot(o.Normal, IN.viewDir);
            // 외각만 어둡게 한다
            // 카메라가 비치는 방향이 실제로 밝아 보인다
            // o.Emission = rim;

            // 외각만 밝게 한다
            // o.Emission = 1 - rim;
            // 제곱해서 외각만 선으로 보이게끔 한다
            // 10승 해서 외각만 1이고 나머진 0에 가깝게 되었다
            o.Emission = pow(1 - rim, 10);
        }

        float4 LightingTest017(SurfaceOutput s, float3 lightDir, float atten) 
        {

            return 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
