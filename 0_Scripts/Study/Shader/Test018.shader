Shader "Custom/Test/018"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _Bump("Normal Map", 2D) = "bump" {}

        // ������Ʈ
        _RimPow("Rim Power", Range(1, 10)) = 5
        // HDR�� �������̽����� HDR�� ������ ��� ȿ���� ���� �� �ִ�
        // ���� ��� Ȯ�� ��Ʈ����Ʈ�� ���� �ɰ� ����
        [HDR] _RimColor("Rim Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Bump;

        // ������Ʈ
        float _RimPow;
        float4 _RimColor;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_Bump;
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float3 Bump = UnpackNormal(tex2D(_Bump, IN.uv_Bump));

            o.Normal = Bump;

            float rim = saturate(dot(o.Normal, IN.viewDir));
            // ���� �۾��� ���� �ܰ��� Ŀ����
            rim = pow(1 - rim, _RimPow);

            o.Emission = rim * _RimColor.rgb;
            o.Alpha = MainTex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
