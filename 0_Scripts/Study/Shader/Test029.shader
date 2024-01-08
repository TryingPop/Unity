Shader "Custom/Test/029"
{
    Properties
    {

        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert addshadow

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
            o.Alpha = MainTex.a;
        }

        void vert(inout appdata_full v) 
        {

            // ���ؽ� �÷� ���常 x�� �̵�
            // ���� ���� ���� ���ϸ� �ʱⰪ���� (1, 1, 1, 1)�� ��ܼ� x�� �̵��Ѵ�
            // v.vertex.x = v.vertex.x + ((sin(_Time.y) * 0.3) * v.color.r);
            // g ���� �ִ� ���� �� ������ �̵�
            // v.vertex.x = v.vertex.x + ((sin(_Time.y * 3 * v.color.g) * 0.1) * v.color.r);

            // lerp �Լ��� �ᵵ �ȴ�
            // lerp�Լ��� ���ؽ� ����
            v. vertex.x = lerp(v.vertex.x, v.vertex.x + sin(_Time.y * 3), v.color.r);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
