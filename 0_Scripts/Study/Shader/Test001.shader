// 경로와 이름
Shader "Custom/Test/001"
{

    // 유니티 엔진 내부에서 변수들을 조작할 수 잇는 인터페이스를 정해주는 곳
    // 여기서는 작동되지 않는다
    Properties
    {

        // _Color 코드 안에서 사용하는 변수 명, 
        // 첫 번째 매개변수 "Color"은 유니티 엔진에서 표현하는 변수 명, 
        // 두 번째 매개변수 Color는 변수 타입이고,
        // 오른쪽 (1, 1, 1, 1) 은 초기 값 흰색
        _Color ("Color", Color) = (1,1,1,1)
        // 2D, white는 찾아봐야 겠다.
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // Range는 Range 어트리뷰트와 같다
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        // int 타입은 사용 안된다 float으로 인식된다
        _Int("... int ...", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // 이제부터 CG 코드를 이용하겠다는 선어문
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        // 설정 스니핏(snippet) 
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        // 3.0 버전으로 컴파일 지정
        #pragma target 3.0

        sampler2D _MainTex;

        // 엔진으로부터 받아와야 할 데이터들을 이 안에 넣는다
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        // 함수 형식이라 보면 된다
        // inout은 가지고 들어올 수도 나갈 수도 있다는 선언문
        // SurfaceOutputStandard 구조체는 다음과 같이 구성
        // struct SurfaceOutputStandard {
        // 
        //     fixed3 Albedo;
        //     fixed3 Normal;
        //     fixed3 Emmision;
        //     half Metallic;
        //     half Smoothness;
        //     half Occlusion;
        //     half Alpha;
        // }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            ///
            /// 기본
            /// 
            // r = 1f, g = 0f, b = 0f 
            // 즉 빨간색 마테리얼로 되고 빛 영향 받는다 즉 그림자가 있다
            // o.Albedo = float3(1, 0, 0);

            // r = 0f, g = 1f, b = 0f;
            // 빛 영향 안받는 녹색 즉 그림자가 없다
            // o.Emission = float3(0, 1, 0);

            ///
            /// 연산
            /// 
            // 같은 차원끼리만 덧셈 가능 다르면 시도는 해보는데 에러 뜬다
            // float3(0, 1, 1) 와 같다
            // 즉 r = 0f, g = 1f, b = 1f인 Cyan색과 같다
            // 사칙연산이 비슷하게 진행된다
            // 그런데 나눗셈은 무거워서 곱셈으로 보통 대체해서 사용!
            // o.Emission = float3(0, 1, 0) + float3(0, 0, 1);

            ///
            /// 반전
            /// 
            // Cyan색으로 나온다
            // 1 = float3(1, 1, 1) 로 생각하면 된다
            // float3(0, 1, 1) 이되어 위와 같은 결과!
            // 이 기법을 반전(invert) 이라 한다
            // o.Emission = 1 - float3(1, 0, 0);

            ///
            /// 변수 선언
            /// 
            // 여기서 변수도 선언 가능하다
            float3 R = float3(1, 0, 0);
            float3 G = float3(0, 1, 0);
            float3 B = float3(0, 0, 1);

            // float(1, 0, 0)이 대입된다!
            // o.Emission = R;

            // 마찬가지로 덧셈 연산도 가능
            // R + G = float(1, 1, 0)
            // 이므로 노란색이 된다
            // o.Emission = R + G;

            ///
            /// Swizzling 기법
            /// 
            // 각자리 꺼내는 방법이다 위에서 정의한 R에 대해 첫 번째 인자는 r이나 x, 
            // 두 번째 인자는 g나 y, 
            // 세 번째 인자는 b나 z로 꺼내올 수 있다
            // 만약 float4 타입이면 마지막껀 a나 w
            
            // R.rgb = R
            // o.Emission = R.rgb;
            // o.Emission = R.xyz;

            // R.gbr = B
            // o.Emission = R.gbr;
            // o.Emission = R.yzx;

            // R.brr = float3(0, 1, 1) Cyan
            // o.Emission = R.brr
            // 이렇게 각 채널을 섞어 다른 결과를 만들어 내는 것을 스위즐링(Swizzling)기법이라 한다
            // o.Emission = float3(0, R.rr); 과 같이 사용해도 된다
            o.Emission = float3(0, R.xx);
        }
        ENDCG
    }
    FallBack "Diffuse"
}

// 참고한 사이트
// https://celestialbody.tistory.com/5