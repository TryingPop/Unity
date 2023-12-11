Shader "Custom/Test/009"
{
    Properties
    {

        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // surface shader�� �ƴ� fragment shader������ �ø�ƽ�� �̿��� ������ �������� ���
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        // ���ؽ�(Vertex) �ȿ� Į�� ��������
        // ���ؽ��� Į�� �������� 3D Max�� ����Ƽ�� PolyBrush�� �̿�
        struct Input
        {

            float2 uv_MainTex;
            // Ÿ�� �̸� �ø�ƽ(Semantics)
            // ������ ��Ȯ�� �ǹ̸� GPU���� �˷��ִ� �±�
            // ���̴� ��ǲ ����ü���� � �ڷ�� ������� �˷��ִ� ���� �ø�ƽ
            float4 color:COLOR;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Emission = IN.color.rgb * MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
