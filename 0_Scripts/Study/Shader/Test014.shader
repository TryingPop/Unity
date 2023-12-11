// �ۼ��ڿ� ������ Legacy�� Diffuse Shader�� ��ġ�Ѵٰ� �Ѵ�
Shader "Custom/Test014"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test
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

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            // o.Emission = MainTex.rgb;
            o.Albedo = MainTex.rgb;
        }

        // ������ ������ �̸��� ���� �ؾ��Ѵ�
        // lightDir�� �ܼ��� ����Ʈ�� ���⸸ �������⿡ ����Ʈ ����� ������ ������� �ʴ´�
        // �� �� ������ �ҷ��ð� atten�� _LightColor0�̴�
        float4 LightingTest(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // ���� ��ġ�� ������ ������� ���� ������ �ش� ���� �Ⱥ�ġ�� ������ ��ο� ������ �ش�
            // �� ���� ��ġ�� ������ ����� ������, ���� �Ⱥ�ġ�� ������ �������� ������
            float NdotL = dot(s.Normal, lightDir);

            // Light Vector�� �� �ݴ� ������ Normal Vector�� ������ �ִ� ���ؽ��� -1 �� ���� ���� �Ǿ� ���� ���꿡 ������ ���� �� �ִ�
            // �׷��� 0 ~ 1 ���̷� ���� �����ִ� saturate �Լ��� �̿�
            // -1�� ���� ���������� ���δ� �ݸ� 0�� ���� �ణ�� �׸��� ����ó�� ���δ�
            NdotL = saturate(NdotL);
            // return NdotL;

            float4 FinalColor;
            // _LightColor0�� ���� ���� ���� ���ص� �ȴ�
            // _LightColor0�� ����Ʈ �÷��� ������ �ҷ�����,
            // atten�� �׸��ڿ� ����ȿ���� �ҷ��´�
            FinalColor.rgb = s.Albedo * NdotL * _LightColor0.rgb * atten;
            // ������� �ʾƵ� �������ִ� ���� ����?
            FinalColor.a = s.Alpha;

            return FinalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
