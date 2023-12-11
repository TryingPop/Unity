// 작성자에 따르면 Legacy의 Diffuse Shader와 일치한다고 한다
Shader "Custom/Test014"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test
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
            // o.Emission = MainTex.rgb;
            o.Albedo = MainTex.rgb;
        }

        // 위에서 정의한 이름에 맞춰 해야한다
        // lightDir은 단순히 라이트의 방향만 가져오기에 라이트 색상과 강도가 적용되지 않는다
        // 이 두 가지를 불러올게 atten과 _LightColor0이다
        float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // 빛이 비치는 방향은 밝아지는 듯한 느낌을 준다 빛이 안비치는 방향은 어두운 느낌을 준다
            // 즉 빛이 비치는 방향은 흰색에 가깝게, 빛이 안비치는 방향은 검정색에 가깝다
            float NdotL = dot(s.Normal, lightDir);

            // Light Vector와 정 반대 방향의 Normal Vector를 가지고 있는 버텍스는 -1 의 값을 갖게 되어 추후 연산에 지장이 생길 수 있다
            // 그래서 0 ~ 1 사이로 값을 맞춰주는 saturate 함수를 이용
            // -1의 완전 검정색으로 보인다 반면 0인 경우는 약간의 그림자 진거처럼 보인다
            NdotL = saturate(NdotL);
            // return NdotL;

            float4 FinalColor;
            // _LightColor0는 따로 변수 정의 안해도 된다
            // _LightColor0는 라이트 컬러와 강도를 불러오고,
            // atten은 그림자와 감쇠효과를 불러온다
            FinalColor.rgb = s.Albedo * NdotL * _LightColor0.rgb * atten;
            // 사용하지 않아도 선언해주는 것이 예의?
            FinalColor.a = s.Alpha;

            return FinalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
