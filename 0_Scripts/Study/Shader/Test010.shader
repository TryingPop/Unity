Shader "Custom/Test/010"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _RTex("R Texture", 2D) = "white" {}
        _GTex("G Texture", 2D) = "white" {}
        _BTex("B Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        sampler2D _RTex;
        sampler2D _GTex;
        sampler2D _BTex;

        struct Input
        {

            float2 uv_MainTex;

            float4 color:COLOR;

            float2 uv_RTex;
            float2 uv_GTex;
            float2 uv_BTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            float4 RTex = tex2D(_RTex, IN.uv_RTex);
            float4 GTex = tex2D(_GTex, IN.uv_GTex);
            float4 BTex = tex2D(_BTex, IN.uv_BTex);

            // o.Emission = MainTex.rgb;
            // MainTex가 배경인 곳에서 버텍스에 빨간 점으로 되어져 있는데만 
            // RTex를 칠해라는 코드
            o.Emission = lerp(MainTex, RTex, IN.color.r);

            // 위에 덧칠 된 곳에서 녹색으로 칠해진 곳은 GTex로 덮어 씌우라는 코드
            o.Emission = lerp(o.Emission, GTex, IN.color.g);

            // 비슷하게 버텍스에 파란색으로 칠해진 곳은 BTex로 덮어씌우는 코드
            // 해당 코드까지 실행되면 버택스에 빨간색은 RTx, 녹색은 GTex, 파란색은 BTex로 덮어 씌여진다
            // 빈 자리는 MainTex가 있다
            o.Emission = lerp(o.Emission, BTex, IN.color.b);

            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
