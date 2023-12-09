Shader "Custom/Test/005"
{
    Properties
    {

        _MainTex("Texture", 2D) = "red" {}

        _U("Tiling U", Range(1, 5)) = 1
        _V("Tiling V", Range(1, 5)) = 1
        _X("Offset X", Range(-1, 1)) = 0
        _Y("Offset Y", Range(-1, 1)) = 0

        _MT("Multiple Time", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        float _U;
        float _V;

        float _X;
        float _Y;

        float _MT;
        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            // uv ��ǥ�� 1���� Ŀ���� �ش� �κ��� �ؽ��� Ÿ���� WrapMode�� ���� �޲۴�
            // ���� 2�谡 �Ǿ����Ƿ� ���� �̹����� 1 / 4�� �پ��� ���� �ϴܿ� ��ġ!
            // �׸��� �������� WrapMode ���� 3 / 4 �� �޲�����
            // Repeat - Ÿ�ϸ�
            // Clamp - ���� �� �κ��� ���� �ø���
            // Mirror - y���� Ŀ���� ���� ���� x�� ��Ī �ѹ�, x ���� Ŀ���� ��� y�� ��Ī �ѹ��� �ϸ鼭 ����
            // Per-axis - Ŀ���� ���

            // 2 * 2�� �����
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex * 2);

            // MainTex�� x, y, r, g�� ��� �����ϳ� u, v�� ��� �Ұ��� 
            // float4 MainTex = tex2D(_MainTex, IN.uv_MainTex * float2(_U, _V));

            // 2D�� Tile�� Offset�� ���� ���� �־ Ȯ���� �� ���̴�
            // float4 MainTex = tex2D(_MainTex, float2(IN.uv_MainTex.x * _U + _X, IN.uv_MainTex.y * _V + _Y));

            // �̸� ����� ��Ʈ�� ���̵� ����
            // _Time = float4(t/20, t, t*2, t*3);
            // ����ϰ� _SinTime, _CosTime, unity_DeltaTime�� �ִ�
            // �̴� https://celestialbody.tistory.com/7 ����Ʈ ����!
            float4 MainTex = tex2D(_MainTex, float2(IN.uv_MainTex.x * _U + _X + _Time.y * _MT, IN.uv_MainTex.y * _V + _Y));
            o.Emission = MainTex.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
