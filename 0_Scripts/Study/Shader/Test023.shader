// Phong Shader
// Lambert���� Specular�� ����
// Lambert�� Speculer �߰��غ��°� ��ǥ!
Shader "Custom/Test/023"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test023
        #pragma target 3.0

        sampler2D 
        _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);

            o.Albedo = MainTex.rgb;
            o.Alpha = 1;
        }

        // �ش� ������ �Ű������� �����ؾ� viewDir�� �ҷ��� �� �ִ�
        float4 LightingTest023(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) 
        {

            // Speculer�� ���ݻ縦 �ǹ��ϴµ�
            // �� �ݻ�� ���� ���� ������ Speculer�� ����������
            // ������ �־������� Speculer�� �������
            // �̸� �ڵ�� �����ϸ� Phong Shading
            float3 ReV = reflect(normalize(-lightDir), normalize(s.Normal));    // -light�� �����´� s.Normal�� ��������
            float rdotv = saturate(dot(ReV, viewDir));                          // ���� ���� �ݻ�� ���� ����

            rdotv = pow(rdotv, 20);                                             // ���� ����
            return rdotv;

            // �ϵ���� �ߴ޷� Reflection Vector�� �����Ӱ� �̿� �����ϳ�
            // �������� �� ���ſ��ٰ� �Ѵ�
            // �׷��� ������� Blin-PhongSading ����� �̿�
            // Specular reflection ������ ���� ������ �ϱ����� ������ ���� ����� �̿�
            float3 h = normalize(viewDir + lightDir);                           // viewDir�� lightDir�� ���� ����
            float spec = saturate(dot(s.Normal, h));                            // �븻�� ���������� ����(����ŧ��)

            spec = pow(spec, 80);                                               // ���� ����
            return spec;                                            

        }
        ENDCG
    }
    FallBack "Diffuse"
}
