// Half Lambert
// 기존의 Lambert light를 개량하여 더 부드럽게 음영처리 되도록 만들어 준다
// 물리적으로 옳진 않으나 예쁘다!
Shader "Custom/Test/015"
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
        #pragma surface surf Test015
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = MainTex.rgb;
        }

        float4 LightingTest015(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // 하프 렘버트 공식을 코드에 넣기 위해서는 음수값이 필요해서 
            // saturate 함수는 사용하지 않는다
            float NdotL = dot(s.Normal, lightDir) * 0.5 + 0.5;
            // NdotL = saturate(NdotL);
            // 하프 렘버트가 생각보다 많이 부드럽게 만들기에 atten을 적용할 때 어색해진다고 한다
            // 그래서 제곱해서 음영을 진하게 만들어 작성자는 해결
            NdotL = NdotL * NdotL * NdotL;

            float4 FinalColor;
            FinalColor.rgb = s.Albedo * NdotL * _LightColor0.rgb * atten;
            FinalColor.a = s.Alpha;

            return FinalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
