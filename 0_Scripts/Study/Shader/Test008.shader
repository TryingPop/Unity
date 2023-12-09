Shader "Custom/Test/008"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex("Sub Texture", 2D) = "white" {}

        _Speed("Speed", Range(0, 10)) = 1
        _Sub("Sub Alpha?", Range(0, 2)) = 1
    }
    SubShader
    {

        // Opaque ������ 
        // ������ �� �� ���İ��� �����ص� ���İ��� �� ���ư���
        // Tags { "RenderType"="Opaque" }

        // xnaud dkfvkrk 
        Tags {"RenderType" = "Transparent"}
        LOD 200

        CGPROGRAM
        // #pragma surface surf Standard fullforwardshadows
        // �ش� �ڵ�� �ٲ������ �ȴ�
        // �� �� ���� �ٲ��� ��� �ڿ� ���׸����� ������ ������ �ȵȴ�...
        #pragma surface surf Standard alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SubTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_SubTex;
        };

        float _Speed;
        float _Sub;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // SubTex�� �� �������� �ö󰡰� �Ѵ�
            float4 SubTex = tex2D(_SubTex, float2(IN.uv_SubTex.x, IN.uv_SubTex.y - _Time.y * _Speed));
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex + SubTex.r * _Sub);

            // �� �ֱ�
            o.Emission = MainTex.rgb;
            // Sub�� ���� ���̰� �Ϸ���
            // o.Emission = MainTex.rgb + SubTex.rgb;

            // ���� �� �߰� ���ϸ� ������ �ʴ´�!
            o.Alpha = MainTex.a;

            // ���� ���̰� �Ѵٸ� ����������
            // o.Alpha = MainTex.a + SubTex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
