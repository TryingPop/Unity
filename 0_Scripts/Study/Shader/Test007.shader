Shader "Custom/Test/007"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "red" {}
        _SubTex("Sub Texture", 2D) = "white" {}
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

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // MainTex를 배경으로 하고 SubTex가 덧붙여진 모습!
            // float4 SubTex = tex2D(_SubTex, IN.uv_SubTex);
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r);
            

            // Sub에는 중앙에만 색이 있고 가장자리 쪽은 색이 없는 것을 넣는다
            // 그러면 SubTexture가 배경이 MainTexutre에서 대각선 이동 하는 듯한 모습
            // 대각선 이동은 Time.y에의해 나타난다!
            float4 SubTex = tex2D(_SubTex, IN.uv_SubTex + _Time.y);

            // SubTex의 모습이 자세히 보인다
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r);
            // SubTex의 모습이 자세히 안보인다 물결 치는거 같다
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r * 0.01);

            // 적용 부분
            o.Emission = MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
