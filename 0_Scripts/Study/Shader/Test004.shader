Shader "Custom/Test/004"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "red" {}

        _SubTex("Sub Texture" , 2D) = "red" {}

        _LerpValue("Lerp Value", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SubTex;

        float _LerpValue;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float4 SubTex = tex2D(_SubTex, IN.uv_SubTex);

            // MainTex에서 SubTex로 변환 하는 코드
            // lerp 함수를 이용 lerp(x, y, s) 여기서 x, y는 같은 단위!
            // o.Emission = lerp(MainTex, SubTex, _LerpValue);

            // MainTex 중 이미지 있는 부분만 SubTex 이미지를 불러오게 한다
            // 나머지부분은 흰색!
            // o.Emission = lerp(MainTex, SubTex, MainTex.a * _LerpValue);

            // SubTex를 배경으로 MainTex를 불러오는 방법
            o.Emission = lerp(MainTex, SubTex, 1 - MainTex.a * _LerpValue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
