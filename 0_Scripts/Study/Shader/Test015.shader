// Half Lambert
// ������ Lambert light�� �����Ͽ� �� �ε巴�� ����ó�� �ǵ��� ����� �ش�
// ���������� ���� ������ ���ڴ�!
Shader "Custom/Test/015"
{
    Properties
    {

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test015
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = MainTex.rgb;
        }

        float4 LightingTest015(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // ���� ����Ʈ ������ �ڵ忡 �ֱ� ���ؼ��� �������� �ʿ��ؼ� 
            // saturate �Լ��� ������� �ʴ´�
            float NdotL = dot(s.Normal, lightDir) * 0.5 + 0.5;
            // NdotL = saturate(NdotL);
            // ���� ����Ʈ�� �������� ���� �ε巴�� ����⿡ atten�� ������ �� ��������ٰ� �Ѵ�
            // �׷��� �����ؼ� ������ ���ϰ� ����� �ۼ��ڴ� �ذ�
            NdotL = NdotL * NdotL * NdotL;

            float4 FinalColor;
            FinalColor.rgb = s.Albedo * NdotL * _LightColor0.rgb * atten;
            FinalColor.a = s.Alpha;

            return FinalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
