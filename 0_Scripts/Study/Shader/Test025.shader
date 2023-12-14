// Test024�� ��¥ specular �߰� (Fake Specular)
// �׶󿡼����Ǿ��� ������ ��ũ�� �� �ϳ���,
// ���������� ���� ������ View Vector�� ������ �������� �����Ͽ� ������ ������ ��������� ���ָ鼭 �̻ڰ� �Ѵ�
Shader "Custom/Test/025"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}

        // Rim
        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1, 1)

        // Specular
        _SpPow("Specular Power", Range(1, 100)) = 50
        _SpCol("Specular Color", Color) = (1, 1, 1, 1)
        _GlossTex("Specular Texture", 2D) = "bump" {}

        // Fake Specular
        _FSpPow("Fake Specular Power", Range(1, 100)) = 50
        _FSpCol("Fake Specular Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test025
        #pragma target 3.0

        sampler2D _MainTex, _BumpTex, _GlossTex;
        
        float _RimPow, _SpPow, _FSpPow;
        float4 _RimCol, _SpCol, _FSpCol;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;
            float2 uv_GlossTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float4 BumpTex = tex2D(_BumpTex, IN.uv_BumpTex);
            float4 GlossTex = tex2D(_GlossTex, IN.uv_GlossTex);

            o.Normal = UnpackNormal(BumpTex);
            o.Gloss = GlossTex.a;
            o.Albedo = MainTex.rgb;
            o.Alpha = MainTex.a;
        }

        float4 LightingTest025(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) 
        {

            // Diffuse - Half Lambert
            float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
            ndotl = pow(ndotl, 3);
            float3 diffCol = s.Albedo * ndotl * _LightColor0.rgb * atten;

            // Rim
            float viewL = saturate(dot(viewDir, s.Normal));
            float rim = pow((1 - viewL), _RimPow);
            float3 rimCol = rim * _RimCol.rgb * _LightColor0.rgb;

            // Specular
            float3 h = normalize(viewDir + lightDir);
            float spec = saturate(dot(s.Normal, h));

            spec = pow(spec, _SpPow) * s.Gloss;
            float3 spCol = spec * _LightColor0.rgb * _SpCol;

            // 2nd Specular (Faker Specular)
            // Test024�� �߰��� ����
            float Fspec = pow(viewL, _FSpPow) * s.Gloss;
            float3 FspCol = Fspec * _FSpCol.rgb * _LightColor0.rgb;

            // 
            float4 finalColor;
            finalColor.rgb = diffCol + rimCol + spCol + FspCol;
            finalColor.a = s.Alpha;

            return finalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
