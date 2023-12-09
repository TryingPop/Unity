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

            // ���η� ������ ���� 0, �������� ��� 1
            // o.Emission = IN.uv_MainTex.x;

            // ���η� �Ʒ��� ���� 0, ������ ��� 1
            // o.Emission = IN.uv_MainTex.y;

            // uv�� ������ ���´�
            o.Emission = float3(IN.uv_MainTex.xy, 0);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
