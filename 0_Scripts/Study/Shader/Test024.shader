// ���ݱ��� ��� ����Ʈ ���̴��� ���� ���ĺ���
Shader "Custom/Test/024"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}

        // �� ����Ʈ(Fresnel)
        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1, 1)

        // Specular
        _SpPow("Specular Power", Range(1, 100)) = 50
        // _SpecColor �� ����Ƽ ���庯���� �̹� ����Ǿ� �ִ�! �׷��� �ߺ� �ȵǰ� �ٸ� �̸� ����Ѵ�!
        [HDR] _SpCol("Specular Color", Color) = (1, 1, 1, 1)
        // ���ϴ� �κи� ����ŧ�� ���� �ǰ� ������ �ؽ���
        _GlossTex("Sepcular Texture", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test024
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;
        sampler2D _GlossTex;


        float _RimPow;
        float4 _RimCol;

        float _SpPow;
        float4 _SpCol;

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

            fixed4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 BumpTex = tex2D(_BumpTex, IN.uv_BumpTex);
            fixed4 GlossTex = tex2D(_GlossTex, IN.uv_GlossTex);

            o.Normal = UnpackNormal(BumpTex);
            // Specular�� ����� �ؽ��ĸ� �־��ش�
            o.Gloss = GlossTex.a;
            o.Albedo = MainTex.rgb;
            o.Alpha = MainTex.a;
        }

        // �Ű����� ���� �߿�!
        float4 LightingTest024(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) 
        {

            // Diffuse
            // Half Lambert
            float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
            ndotl = pow(ndotl, 3);
            // _LightColor0�� ���庯���̰� 
            // ����Ʈ Į��� ������ �����ϰ� �ִ�! Test013�� �ִ�!
            float3 diffCol = s.Albedo * ndotl * _LightColor0.rgb * atten;

            // Rim
            float rim = saturate(dot(viewDir, s.Normal));
            rim = pow(1 - rim, _RimPow);
            // RimCol�� ����Ʈ �÷��� �޾ƿͼ� ���� �ڿ������� ������ �ϱ� ���� _LightColor0�� ������ٰ� �Ѵ�
            float3 rimCol = rim * _RimCol.rgb * _LightColor0.rgb;

            // Specular
            // Blin-Phong Shading
            float3 halfVec = normalize(viewDir + lightDir);
            float spec = saturate(dot(s.Normal, halfVec));

            // �ڵ带 ���� surf�� ���� ����ǰ� Lighting�� �ڿ� ���� �Ǵ°� ����
            // �տ��� ������ �ؽ��ĸ� ������ Specular�� ǥ���Ѵ�
            // 0�� �κ��� Specular�� ǥ�õ��� �ʰ�, 1�� �κ��� �����ϰ� ��Ÿ����
            spec = pow(spec, _SpPow) * s.Gloss;
            float3 speCol = spec * _LightColor0.rgb * _SpCol;

            // Final
            float4 finalColor;
            
            // RimCol�� [HDR] ��Ʈ����Ʈ�� ��������⿡ 1 �̻��� �÷����� ����� �� �ִ�
            finalColor.rgb = diffCol + rimCol + speCol;
            finalColor.a = s.Alpha;

            return finalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
