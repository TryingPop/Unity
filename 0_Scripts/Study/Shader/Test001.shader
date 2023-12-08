// ��ο� �̸�
Shader "Custom/Test/001"
{

    // ����Ƽ ���� ���ο��� �������� ������ �� �մ� �������̽��� �����ִ� ��
    // ���⼭�� �۵����� �ʴ´�
    Properties
    {

        // _Color �ڵ� �ȿ��� ����ϴ� ���� ��, 
        // ù ��° �Ű����� "Color"�� ����Ƽ �������� ǥ���ϴ� ���� ��, 
        // �� ��° �Ű����� Color�� ���� Ÿ���̰�,
        // ������ (1, 1, 1, 1) �� �ʱ� �� ���
        _Color ("Color", Color) = (1,1,1,1)
        // 2D, white�� ã�ƺ��� �ڴ�.
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // Range�� Range ��Ʈ����Ʈ�� ����
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        // int Ÿ���� ��� �ȵȴ� float���� �νĵȴ�
        _Int("... int ...", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // �������� CG �ڵ带 �̿��ϰڴٴ� ���
        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        // ���� ������(snippet) 
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        // 3.0 �������� ������ ����
        #pragma target 3.0

        sampler2D _MainTex;

        // �������κ��� �޾ƿ;� �� �����͵��� �� �ȿ� �ִ´�
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
        
        // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        // �Լ� �����̶� ���� �ȴ�
        // inout�� ������ ���� ���� ���� ���� �ִٴ� ����
        // SurfaceOutputStandard ����ü�� ������ ���� ����
        // struct SurfaceOutputStandard {
        // 
        //     fixed3 Albedo;
        //     fixed3 Normal;
        //     fixed3 Emmision;
        //     half Metallic;
        //     half Smoothness;
        //     half Occlusion;
        //     half Alpha;
        // }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            ///
            /// �⺻
            /// 
            // r = 1f, g = 0f, b = 0f 
            // �� ������ ���׸���� �ǰ� �� ���� �޴´� �� �׸��ڰ� �ִ�
            // o.Albedo = float3(1, 0, 0);

            // r = 0f, g = 1f, b = 0f;
            // �� ���� �ȹ޴� ��� �� �׸��ڰ� ����
            // o.Emission = float3(0, 1, 0);

            ///
            /// ����
            /// 
            // ���� ���������� ���� ���� �ٸ��� �õ��� �غ��µ� ���� ���
            // float3(0, 1, 1) �� ����
            // �� r = 0f, g = 1f, b = 1f�� Cyan���� ����
            // ��Ģ������ ����ϰ� ����ȴ�
            // �׷��� �������� ���ſ��� �������� ���� ��ü�ؼ� ���!
            // o.Emission = float3(0, 1, 0) + float3(0, 0, 1);

            ///
            /// ����
            /// 
            // Cyan������ ���´�
            // 1 = float3(1, 1, 1) �� �����ϸ� �ȴ�
            // float3(0, 1, 1) �̵Ǿ� ���� ���� ���!
            // �� ����� ����(invert) �̶� �Ѵ�
            // o.Emission = 1 - float3(1, 0, 0);

            ///
            /// ���� ����
            /// 
            // ���⼭ ������ ���� �����ϴ�
            float3 R = float3(1, 0, 0);
            float3 G = float3(0, 1, 0);
            float3 B = float3(0, 0, 1);

            // float(1, 0, 0)�� ���Եȴ�!
            // o.Emission = R;

            // ���������� ���� ���굵 ����
            // R + G = float(1, 1, 0)
            // �̹Ƿ� ������� �ȴ�
            // o.Emission = R + G;

            ///
            /// Swizzling ���
            /// 
            // ���ڸ� ������ ����̴� ������ ������ R�� ���� ù ��° ���ڴ� r�̳� x, 
            // �� ��° ���ڴ� g�� y, 
            // �� ��° ���ڴ� b�� z�� ������ �� �ִ�
            // ���� float4 Ÿ���̸� �������� a�� w
            
            // R.rgb = R
            // o.Emission = R.rgb;
            // o.Emission = R.xyz;

            // R.gbr = B
            // o.Emission = R.gbr;
            // o.Emission = R.yzx;

            // R.brr = float3(0, 1, 1) Cyan
            // o.Emission = R.brr
            // �̷��� �� ä���� ���� �ٸ� ����� ����� ���� ���� ������(Swizzling)����̶� �Ѵ�
            // o.Emission = float3(0, R.rr); �� ���� ����ص� �ȴ�
            o.Emission = float3(0, R.xx);
        }
        ENDCG
    }
    FallBack "Diffuse"
}

// ������ ����Ʈ
// https://celestialbody.tistory.com/5