// 2pass �ϴ¹�
// ���� CGPROGRAM ���� ENDCG���� �����ؼ� �ٿ��ִ´�
Shader "Custom/Test/031"
{
    Properties
    {

        _OutLine("Out Line", Range(0, 1)) = 1
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        // �� ������
        // ť�꿡 �־�� �� ���� ���������� �޸��� �巯����
        // ���Ǿ���� Ȯ���� �����
        cull front

        /// 
        /// 1pass
        /// 
        // �ܰ������� ���Ǵ� 1pass�� 2pass���� Ŀ����
        // 1pass�� 2pass�� �������� �ʰ� ǥ���ȴ�
        // �ܰ����� ������ �׸��� ���� �� ������� ���� ��� ��� �����ؼ� �ִ��� ������ ����� �ִ°� ����
        CGPROGRAM
        // ���ؽ� ���̴� ���� �׸��� �Ⱦ���
        #pragma surface surf Test031 vertex:vert noshadow
        #pragma target 3.0

        sampler2D _MainTex;
        float _OutLine;

        struct Input
        {

            float4 color:COLOR;
        };

        // ������ ����Ƽ���� �������;
        void surf(Input IN, inout SurfaceOutput o) { }

        void vert(inout appdata_full v)
        {

            // �븻 �������� ���ؽ� �̵� (�ܰ��� ����)
            v.vertex.xyz = v.vertex.xyz + v.normal * _OutLine;
        }

        // ���⼭ ���������� �־��ش�
        float4 LightingTest031(SurfaceOutput s, float3 lightDir, float atten)
        {

            return float4(0, 0, 0, 1);
        }
        ENDCG


        /// 
        /// 2pass
        /// 
        // forward render������ ������ ���
        // �𸮾󿡼��� �ܰ����� Pos-tprocess�� ó���Ѵٰ� �Ѵ�
        // 2pass�� �ٽ� �ո鿡 ����
        // �տ��� �������� ������ �ٽ� ����� �������� ������
        cull back
        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
