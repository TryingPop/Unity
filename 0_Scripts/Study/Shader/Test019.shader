// �����ǰ� �ܰ��� ���Դٰ� ������� ���̴� ������
Shader "Custom/Test/019"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}

        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1,1)
    }
    SubShader
    {

        // �����ϰ� �ش޶�� ��û
        // Queue�� ������Ʈ �׷����� ������ Transparent�� �ش޶�� ���̴�!
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        // ����Ʈ ��İ� ���� ����!
        // ���� ����� �� ������ ����!
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;

        float _RimPow;
        float4 _RimCol;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;

            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float3 BumpTex = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));

            o.Normal = BumpTex;

            // �� ����Ʈ
            float rim = saturate(dot(o.Normal, IN.viewDir));
            // 1 - rim ���ذ� �ܰ��� �����ֱ� ���ؼ�
            rim = pow(1 - rim, _RimPow);

            o.Emission = _RimCol.rgb;
            // o.Alpha = rim;

            // �� ��� ���� ���� ��� 0 �̵Ǳ⿡ ������ �ʹ� ���!
            // sin�� ���� �Լ��̴�!
            // o.Alpha = saturate(rim * sin(_Time.y))

            // ���Դٰ� ������ٰ� �ϴ� �ڵ�

            // ���� Half Lambert ���̵� ����!
            // ���� ��ȯ�ؼ� 0 ~ 1 ���̸� �Դٰ����ϴ� ���� �Լ��� �������!
            // o.Alpha = saturate(rim * sin(_Time.y) * 0.5 + 0.5 );

            // Ȥ�� ���밪�� �̿�
            // ���밪�� abs (absolute)
            // ���� Ƣ�� (�ٿ �ϴ�) ���� ������ �ش�
            o.Alpha = rim * abs(sin(_Time.y));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
