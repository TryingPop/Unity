Shader "Custom/Test/002"
{
    Properties
    {

        _R("R", Range(0, 1)) = 0.5
        _G("G", Range(0, 1)) = 0.5
        _B("B", Range(0, 1)) = 0.5

        // 밝기 - 밝을 때는 흰색, 어두울 때는 검정색
        // 0, 1 은 연산이 복잡해진다
        // _Bright("Bright", Range(0, 1)) = 0.5
        _Bright("Bright", Range(-1, 1)) = 0.5
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        // float은 고정밀도
        // half는 중정밀도
        // fixed는 저정밀도
        float _R;
        float _G;
        float _B;
        float _Bright;

        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            o.Emission = float3(_R, _G, _B) + _Bright;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
