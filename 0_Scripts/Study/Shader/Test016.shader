// Lambert Ŀ���� ����Ʈ ����� ��
Shader "Custom/Test/016"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Lambert Ŀ���� ����Ʈ�� �������� �ѹ濡 �ذ� ����
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {


        }
        ENDCG
    }
    FallBack "Diffuse"
}
