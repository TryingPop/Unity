Shader "Custom/Test/003"
{
    Properties
    {

        // 텍스쳐
        // 변수명으로 _MainTex를 사용할 경우 텍스쳐를 갈아끼워도 호환이 가능
        // white 뿐만 아니라 red, black 사용가능, 보통은 white, black으로 사용
        // _MainTex("Texture", 2D) = "white" {}
        _MainTex("Texture", 2D) = "red" {}      // 붉은 색으로 표시된다

        _LerpValue("Lerp Value", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        // Texture는 sample2D로 가져온다
        // 픽셀 하나하나만 집어내는 과정을 표본추출(Sampling)
        // 텍스쳐 픽셀을 하나하나씩 쏙쏙 집어내어 오브젝트의 지정된 위치에 콕콕 박아주는 것
        // 무엇을 입힐 것인지를 받아온다
        sampler2D _MainTex;

        float _LerpValue;
        
        // 지정된 위치는 UV 좌표값이고 2차원을 필요
        // RGB = XYZ =UVW 유니티는 좌측 하단이 0,0 언리얼은 좌측 상단이 0, 0
        // UV값은 모델링 데이터에 포함되어 있는 값을 받아오는 것인데 이럴 때 사용하는 Input이다
        struct Input
        {

            // UV 좌표를 받아온다
            // 어디에 입힐지 받아온다
            float2 uv_MainTex;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {

            // 텍스쳐를 UV에 맞춰 출력하기 위해 사용하는 함수가 tex2D(Tex, TexUV)
            // tex2D(_MainTex, Input.uv_MainTex); 
            // 는 할 수 없다 위에 struct Input은 구조체 선언문이라 보면 된다
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            // 색상 넣어주기
            // MainTex는 float4인데 Emission은 float3 이다 유니티에서 짤라서 들어가는거 뿐이다
            // o.Emission = MainTex;
            // o.Emission = MainTex.rgb;

            // 회색으로 만드는 법
            // r, g, b 의 값이 같은 경우 회색 이므로
            // o.Emission = (MainTex.r + MainTex.g + MainTex.b) / 3;

            // 자연스럽게 만들러면 lerp함수를 이용 설명은 004에 있다
            o.Emission = lerp(MainTex.rgb, (MainTex.r + MainTex.g + MainTex.b) / 3, _LerpValue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
