Shader "Custom/Test011"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _RTex("R Texutre", 2D) = "white" {}
        _GTex("G Texutre", 2D) = "white" {}
        _BTex("B Texutre", 2D) = "white" {}

        // 노말에 흰색이 들어갈 경우 에러는 안나지만 빛 연산이 이상해질 수 있다
        _BumpMap("Normal", 2D) = "bump" {}

        _NP("Normal Power", float) = 1
        _SM("Smoothness", Range(0, 1)) = 0.5

        _RNormal("R Normal", 2D) = "bump" {}
        _RSM("R Smoothness", Range(0, 1)) = 0.5;
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // 이전까지는 해당 코드가 없어도 잘 실행될 수 있으나 여기서는 안된다
        // 이 구문을 지우면 OpenGL ES2.0로 작동하는데 
        // 인터폴레이터를 최대 8개까지만 지원해서
        // 현재 텍스쳐 4개를 사용하고 있는 상황에서 노말맵 추가가 안된다
        // https://celestialbody.tistory.com/8
        // 사이트 참고!
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RTex, _GTex, _BTex;

        sampler2D _BumpMap;

        float _NP;
        float _SM;

        sampler2D _RNormal;
        float _RSM;

        struct Input
        {
            float2 uv_MainTex;

            float2 uv_RTex, uv_GTex, uv_BTex;

            float2 uv_BumpMap;

            float2 uv_RNormal;

            float4 color:COLOR;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);

            float4 RTex = tex2D(_RTex, IN.uv_RTex);
            float4 GTex = tex2D(_GTex, IN.uv_GTex);
            float4 BTex = tex2D(_BTex, IN.uv_BTex);


            o.Albedo = lerp(MainTex, RTex, IN.color.r);
            o.Albedo = lerp(o.Albedo, GTex, IN.color.g);
            o.Albedo = lerp(o.Albedo, BTex, IN.color.b);

            // Albedo와 Emission의 경우엔 별도의 함수 없이 알맞은 변수명만 가져와도 출력해주었다
            // Normal은 따로 전용함수를 붙여줘야 한다!
            // UnpackNormal로 텍스쳐에 노말을 빼온다고 보면 된다 타입은 float3
            // float4 NormalTex = tex2D(_BumpMap, IN.uv_BumpMap);
            // o.Normal = UnpackNormal(NormalTex);

            // 혹은 다음처럼 노말을 넣어도 된다
            // 노말을 더 잘 보이게 하려면 R, G 값을 건드리면 된다, B는 건드리면 에러 뜬다고 한다
            float3 Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            Normal = float3(Normal.r * _NP, Normal.g * _NP, Normal.b);
            // o.Normal = Normal;
            
            // Texture처럼 붉은색 버텍스에만 노말 적용 하는 예시!
            float3 RNormal = UnpackNormal(tex2D(_RNormal, IN.uv_RNormal));
            RNormal = float3(RNormal.r * _NP, RNormal.g * _NP, RNormal.b);
            o.Normal = lerp(Normal, RNormal, IN.color.r);


            // 물체의 질감을 결정짓는 프로퍼티
            // Normal map은 Speculer가 없으면 잘 느껴지지 않는다
            // 유니티에서는 Speculer를 Smoothness라는 이름으로 불린다
            // o.Smoothness = 0.5;
            
            // 버텍스가 붉은 색 부분에만 Smoothness 적용
            o.Smoothness = lerp(_SM, _RSM, IN.color.r);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
