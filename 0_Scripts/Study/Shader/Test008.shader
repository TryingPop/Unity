Shader "Custom/Test/008"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex("Sub Texture", 2D) = "white" {}

        _Speed("Speed", Range(0, 10)) = 1
        _Sub("Sub Alpha?", Range(0, 2)) = 1
    }
    SubShader
    {

        // Opaque 불투명 
        // 랜더링 할 때 알파값이 존재해도 알파값은 싹 날아간다
        // Tags { "RenderType"="Opaque" }

        // xnaud dkfvkrk 
        Tags {"RenderType" = "Transparent"}
        LOD 200

        CGPROGRAM
        // #pragma surface surf Standard fullforwardshadows
        // 해당 코드로 바꿔줘야지 된다
        // 위 두 개를 바꿔준 결과 뒤에 마테리얼이 있으면 투영이 안된다...
        #pragma surface surf Standard alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SubTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        float _Speed;
        float _Sub;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // SubTex가 위 방향으로 올라가게 한다
            float4 SubTex = tex2D(_SubTex, float2(IN.uv_SubTex.x, IN.uv_SubTex.y - _Time.y * _Speed));
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r * _Sub);

            // 색 넣기
            o.Emission = MainTex.rgb;
            // Sub도 겹쳐 보이게 하려면
            // o.Emission = MainTex.rgb + SubTex.rgb;

            // 알파 값 추가 안하면 보이지 않는다!
            o.Alpha = MainTex.a;

            // 겹쳐 보이게 한다면 마찬가지로
            // o.Alpha = MainTex.a + SubTex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
