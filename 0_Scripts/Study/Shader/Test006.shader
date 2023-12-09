Shader "Custom/Test/006"
{
    Properties
    {

        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);

            // 세로로 왼쪽은 검정 0, 오른쪽은 흰색 1
            // o.Emission = IN.uv_MainTex.x;

            // 가로로 아래는 검정 0, 위쪽은 흰색 1
            // o.Emission = IN.uv_MainTex.y;

            // uv의 색상이 나온다
            o.Emission = float3(IN.uv_MainTex.xy, 0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
