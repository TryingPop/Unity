Shader "Custom/Test/004"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "red" {}

        _SubTex("Sub Texture" , 2D) = "red" {}

        _LerpValue("Lerp Value", Range(0, 1)) = 0.5
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

        float _LerpValue;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float4 SubTex = tex2D(_SubTex, IN.uv_SubTex);

            // MainTex���� SubTex�� ��ȯ �ϴ� �ڵ�
            // lerp �Լ��� �̿� lerp(x, y, s) ���⼭ x, y�� ���� ����!
            // o.Emission = lerp(MainTex, SubTex, _LerpValue);

            // MainTex �� �̹��� �ִ� �κи� SubTex �̹����� �ҷ����� �Ѵ�
            // �������κ��� ���!
            // o.Emission = lerp(MainTex, SubTex, MainTex.a * _LerpValue);

            // SubTex�� ������� MainTex�� �ҷ����� ���
            o.Emission = lerp(MainTex, SubTex, 1 - MainTex.a * _LerpValue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
