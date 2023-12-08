Shader "Custom/Test/003"
{
    Properties
    {

        // �ؽ���
        // ���������� _MainTex�� ����� ��� �ؽ��ĸ� ���Ƴ����� ȣȯ�� ����
        // white �Ӹ� �ƴ϶� red, black ��밡��, ������ white, black���� ���
        // _MainTex("Texture", 2D) = "white" {}
        _MainTex("Texture", 2D) = "red" {}      // ���� ������ ǥ�õȴ�

        _LerpValue("Lerp Value", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        // Texture�� sample2D�� �����´�
        // �ȼ� �ϳ��ϳ��� ����� ������ ǥ������(Sampling)
        // �ؽ��� �ȼ��� �ϳ��ϳ��� ��� ����� ������Ʈ�� ������ ��ġ�� ���� �ھ��ִ� ��
        // ������ ���� �������� �޾ƿ´�
        sampler2D _MainTex;

        float _LerpValue;
        
        // ������ ��ġ�� UV ��ǥ���̰� 2������ �ʿ�
        // RGB = XYZ =UVW ����Ƽ�� ���� �ϴ��� 0,0 �𸮾��� ���� ����� 0, 0
        // UV���� �𵨸� �����Ϳ� ���ԵǾ� �ִ� ���� �޾ƿ��� ���ε� �̷� �� ����ϴ� Input�̴�
        struct Input
        {

            // UV ��ǥ�� �޾ƿ´�
            // ��� ������ �޾ƿ´�
            float2 uv_MainTex;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {

            // �ؽ��ĸ� UV�� ���� ����ϱ� ���� ����ϴ� �Լ��� tex2D(Tex, TexUV)
            // tex2D(_MainTex, Input.uv_MainTex); 
            // �� �� �� ���� ���� struct Input�� ����ü �����̶� ���� �ȴ�
            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            // ���� �־��ֱ�
            // MainTex�� float4�ε� Emission�� float3 �̴� ����Ƽ���� ©�� ���°� ���̴�
            // o.Emission = MainTex;
            // o.Emission = MainTex.rgb;

            // ȸ������ ����� ��
            // r, g, b �� ���� ���� ��� ȸ�� �̹Ƿ�
            // o.Emission = (MainTex.r + MainTex.g + MainTex.b) / 3;

            // �ڿ������� ���鷯�� lerp�Լ��� �̿� ������ 004�� �ִ�
            o.Emission = lerp(MainTex.rgb, (MainTex.r + MainTex.g + MainTex.b) / 3, _LerpValue);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
