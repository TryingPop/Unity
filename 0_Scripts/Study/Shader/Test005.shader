Shader "Custom/Test/005"
{
    Properties
    {

        _MainTex("Texture", 2D) = "red" {}

        _U("Tiling U", Range(1, 5)) = 1
        _V("Tiling V", Range(1, 5)) = 1
        _X("Offset X", Range(-1, 1)) = 0
        _Y("Offset Y", Range(-1, 1)) = 0

        _MT("Multiple Time", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        float _U;
        float _V;

        float _X;
        float _Y;

        float _MT;
        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // uv 좌표가 1보다 커지면 해당 부분을 텍스쳐 타입의 WrapMode에 맞춰 메꾼다
            // 현재 2배가 되었으므로 원래 이미지는 1 / 4로 줄어들고 왼쪽 하단에 위치!
            // 그리고 나머지는 WrapMode 따라 3 / 4 가 메꿔진다
            // Repeat - 타일링
            // Clamp - 가장 끝 부분을 집어 늘린다
            // Mirror - y값이 커져서 위로 가면 x축 대칭 한번, x 값이 커지는 경우 y축 대칭 한번씩 하면서 진행
            // Per-axis - 커스텀 모드

            // 2 * 2로 만든다
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex * 2);

            // MainTex에 x, y, r, g는 사용 가능하나 u, v는 사용 불가능 
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex * float2(_U, _V));

            // 2D의 Tile과 Offset을 따로 변수 주어서 확인한 것 뿐이다
            // float4 MainTex = tex2D(_MainTex, float2(IN.uv_MainTex.x * _U + _X, IN.uv_MainTex.y * _V + _Y));

            // 미리 선언된 빌트인 셰이드 변수
            // _Time = float4(t/20, t, t*2, t*3);
            // 비슷하게 _SinTime, _CosTime, unity_DeltaTime이 있다
            // 이는 https://celestialbody.tistory.com/7 사이트 참고!
            float4 MainTex = tex2D(_MainTex, float2(IN.uv_MainTex.x * _U + _X + _Time.y * _MT, IN.uv_MainTex.y * _V + _Y));
            o.Emission = MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
