// 2pass �ϴ¹�
// ���� CGPROGRAM ���� ENDCG���� �����ؼ� �ٿ��ִ´�
Shader "Custom/Test/030"
{
    Properties
    {

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
        CGPROGRAM
        #pragma surface surf Lambert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG


        /// 
        /// 2pass
        /// 
        // forward render������ ������ ���
        // �𸮾󿡼��� �ܰ����� Pos-tprocess�� ó���Ѵٰ� �Ѵ�
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
