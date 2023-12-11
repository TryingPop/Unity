// ������Ʈ
// �ǻ�ü �ڿ��� ���� ������ �־� �ǻ�ü�� ���� ���� �𼭸���,
// �ܰ��� �Ƿ翧�� ���� ���� ��(Rim)�� �����ִ°� RimLight
// ��, �ܰ����� ������ ����°�
Shader "Custom/Test/017"
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
        #pragma surface surf Test017
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            // float2 uv_MainTex;
            // ī�޶� ���͸� �ҷ��´�
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float rim = dot(o.Normal, IN.viewDir);
            // �ܰ��� ��Ӱ� �Ѵ�
            // ī�޶� ��ġ�� ������ ������ ��� ���δ�
            // o.Emission = rim;

            // �ܰ��� ��� �Ѵ�
            // o.Emission = 1 - rim;
            // �����ؼ� �ܰ��� ������ ���̰Բ� �Ѵ�
            // 10�� �ؼ� �ܰ��� 1�̰� ������ 0�� ������ �Ǿ���
            o.Emission = pow(1 - rim, 10);
        }

        float4 LightingTest017(SurfaceOutput s, float3 lightDir, float atten) 
        {

            return 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
