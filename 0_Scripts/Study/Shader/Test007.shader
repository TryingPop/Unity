Shader "Custom/Test/007"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "red" {}
        _SubTex("Sub Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SubTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // MainTex�� ������� �ϰ� SubTex�� ���ٿ��� ���!
            // float4 SubTex = tex2D(_SubTex, IN.uv_SubTex);
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r);
            

            // Sub���� �߾ӿ��� ���� �ְ� �����ڸ� ���� ���� ���� ���� �ִ´�
            // �׷��� SubTexture�� ����� MainTexutre���� �밢�� �̵� �ϴ� ���� ���
            // �밢�� �̵��� Time.y������ ��Ÿ����!
            float4 SubTex = tex2D(_SubTex, IN.uv_SubTex + _Time.y);

            // SubTex�� ����� �ڼ��� ���δ�
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r);
            // SubTex�� ����� �ڼ��� �Ⱥ��δ� ���� ġ�°� ����
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r * 0.01);

            // ���� �κ�
            o.Emission = MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
