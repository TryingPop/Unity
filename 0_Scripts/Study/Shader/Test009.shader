Shader "Custom/Test/009"
{
    Properties
    {

        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // surface shader가 아닌 fragment shader에서는 시맨틱을 이용한 형태의 변수들을 사용
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        // 버텍스(Vertex) 안에 칼라 가져오기
        // 버텍스에 칼라 넣을러면 3D Max나 유니티의 PolyBrush를 이용
        struct Input
        {

            float2 uv_MainTex;
            // 타입 이름 시맨틱(Semantics)
            // 변수의 정확한 의미를 GPU에게 알려주는 태그
            // 쉐이더 인풋 구조체들이 어떤 자료와 연결될지 알려주는 것이 시맨틱
            float4 color:COLOR;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Emission = IN.color.rgb * MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
