Shader "Custom/Test/013"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Standard�� PBR ���� ��� ����Ʈ ������ ���ϱ⿡ ���̴�
        // #pragma surface surf Standard fullforwardshadows

        // Help�� ����� ���� �̸� ������� �ص��ȴ�
        // #pragma surface surf Help
        // ambient�� ���ִ� �ڵ�
        // ��ѿ� ���� �������
        // #pragma surface surf Help noambient

        #pragma surface surf Help noambient nofog
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // Standard�� �̿���ؼ� SurfaceOutputStandard ����ü�� �̿��� �� ����
        // �׷��� ���� ����Ʈ�� SurfaceOutput ����ü�� �̿�
        // struct SurfaceOutput
        // {
        // 
        //     fixed3 Albedo;       // diffuse color
        //     fixed3 Normal;       // tangent space normal
        //     fixed3 Emission;
        //     half Specular;       // specular power in 0 ... 1 range
        //     fixed Gloss;         // specular intensity
        //     fixed Alpha;         // alpha for transparencies
        // 
        // }
        // void surf(Input IN, inout SurfaceOutputStandard o)
        void surf(Input IN, inout SurfaceOutput o)
        {

            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Emission = c.rgb;
        }

        // ����Ƽ�� ����� ���� �ƴϹǷ� �ش� �Լ��� �ʿ��ϴ�
        // SrufaceOutput�� ǥ�� ���̴��� ���� ���� �ڵ尡 ����
        // lightDir�� ���� ������ Ȯ�ν����ִ� ���̴�
        // atten�� ����Ʈ�� ���谪���� ���� ������ ���� ����Ʈ ���� ȿ���� �׸��ڸ� ǥ���� �� �ְ� ���ִ� ����
        float4 LightingHelp(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // ��� ���� ���⵵ ���� �ʴ� ���� ������ Unlit ���̴��� �ϼ�
            // ����Ʈ�� ���� �� �ַ� ���� ���̴�
            // nofog���� �ϸ� �� ����
            return float4(s.Albedo.rgb, 1);


            // ���⿡ emission�� ��ȯ�ϸ� emission�� �� �� �� ����Ǿ� �� ��ƺ��δ�
            // return float4(s.Emission.rgb, 1);
            // �񱳿� �ڵ�
            // return float4(0, 0, 0, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
