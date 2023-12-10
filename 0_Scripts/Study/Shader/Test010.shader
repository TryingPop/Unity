Shader "Custom/Test/010"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _RTex("R Texture", 2D) = "white" {}
        _GTex("G Texture", 2D) = "white" {}
        _BTex("B Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        sampler2D _RTex;
        sampler2D _GTex;
        sampler2D _BTex;

        struct Input
        {

            float2 uv_MainTex;

            float4 color:COLOR;

            float2 uv_RTex;
            float2 uv_GTex;
            float2 uv_BTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            float4 RTex = tex2D(_RTex, IN.uv_RTex);
            float4 GTex = tex2D(_GTex, IN.uv_GTex);
            float4 BTex = tex2D(_BTex, IN.uv_BTex);

            // o.Emission = MainTex.rgb;
            // MainTex�� ����� ������ ���ؽ��� ���� ������ �Ǿ��� �ִµ��� 
            // RTex�� ĥ�ض�� �ڵ�
            o.Emission = lerp(MainTex, RTex, IN.color.r);

            // ���� ��ĥ �� ������ ������� ĥ���� ���� GTex�� ���� ������ �ڵ�
            o.Emission = lerp(o.Emission, GTex, IN.color.g);

            // ����ϰ� ���ؽ��� �Ķ������� ĥ���� ���� BTex�� ������ �ڵ�
            // �ش� �ڵ���� ����Ǹ� ���ý��� �������� RTx, ����� GTex, �Ķ����� BTex�� ���� ��������
            // �� �ڸ��� MainTex�� �ִ�
            o.Emission = lerp(o.Emission, BTex, IN.color.b);

            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
