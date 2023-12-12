// ���� ��ǥ�� ���̴��� �޾ƿ���!!
Shader "Custom/Test/020"
{
    Properties
    {


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        struct Input
        {

            // ���� ��ǥ ���� �޾ƿ´�
            float3 worldPos;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            // ���� ��ǥ�� ���� ���� �ٲ��
            // 0, 0, 0 ���� Ȯ�� ����
            o.Emission = IN.worldPos;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
