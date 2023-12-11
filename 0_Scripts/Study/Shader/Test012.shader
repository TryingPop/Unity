Shader "Custom/Test/012"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Main Tex", 2D) = "white" {}
        _BumpTex ("Normal Tex", 2D) = "bump" {}
        _MeTTex ("Metallic Tex", 2D) = "black" {}

        _AOTex("AO Tex", 2D) = "white"{}
        _AOP("AO Power", Range(0, 1)) = 1

        _EMTex("Emission Tex", 2D) = "black" {}
        _EMP("Emission Power", Range(0, 1))= 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex, _BumpTex, _MeTTex, _AOTex, _EMTex;

        struct Input
        {

            float2 uv_MainTex, uv_BumpTex, uv_MeTTex, uv_AOTex, uv_EMTex;
        };

        float _EMP;
        float _AOP;
        float4 _Color;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 t = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            float4 m = tex2D(_MeTTex, IN.uv_MeTTex);
            float4 b = tex2D(_BumpTex, IN.uv_BumpTex);
            float4 e = tex2D(_EMTex, IN.uv_EMTex);
            float4 a = tex2D(_AOTex, IN.uv_AOTex);

            o.Albedo = t.rgb;
            o.Normal = UnpackNormal(b);

            o.Metallic = m.r;
            o.Smoothness = m.a;

            // occlusion 역할을 모르겠다.
            o.Occlusion = a.r + _AOP;

            o.Emission = e.rgb * _EMP;

            // 투명도
            o.Alpha = t.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}


// 빛에 관한 설명
// https://celestialbody.tistory.com/9