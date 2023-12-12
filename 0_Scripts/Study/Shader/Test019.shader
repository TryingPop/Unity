// 투영되고 외곽이 나왔다가 사라지는 쉐이더 만들어보기
Shader "Custom/Test/019"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}

        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1,1)
    }
    SubShader
    {

        // 투영하게 해달라는 요청
        // Queue는 오브젝트 그려지는 순서를 Transparent로 해달라는 것이다!
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // 램버트 방식과 알파 블렌드!
        // 알파 블랜드는 반 투영이 가능!
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;

        float _RimPow;
        float4 _RimCol;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;

            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float3 BumpTex = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));

            o.Normal = BumpTex;

            // 림 라이트
            float rim = saturate(dot(o.Normal, IN.viewDir));
            // 1 - rim 해준건 외곽만 남겨주기 위해서
            rim = pow(1 - rim, _RimPow);

            o.Emission = _RimCol.rgb;
            // o.Alpha = rim;

            // 이 경우 음수 값은 모두 0 이되기에 공백이 너무 길다!
            // sin은 사인 함수이다!
            // o.Alpha = saturate(rim * sin(_Time.y))

            // 나왔다가 사라졌다가 하는 코드

            // 먼저 Half Lambert 아이디어를 응용!
            // 선형 변환해서 0 ~ 1 사이를 왔다갔다하는 사인 함수로 만들었다!
            // o.Alpha = saturate(rim * sin(_Time.y) * 0.5 + 0.5 );

            // 혹은 절대값을 이용
            // 절대값은 abs (absolute)
            // 통통 튀는 (바운스 하는) 듯한 느낌을 준다
            o.Alpha = rim * abs(sin(_Time.y));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
