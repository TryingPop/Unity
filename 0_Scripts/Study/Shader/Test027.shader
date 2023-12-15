// vertex ���̴�!
Shader "Custom/Test/027"
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
        // 2 pass �� �ܰ����� ����� ���� Vertex shader�� ����
        #pragma surface surf Lambert vertex:vert
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

        // #pragma�� vertex:vert�� �߰������Ƿ� ���� �Լ��� ����������Ѵ�!
        // appdata_ �ڿ� �߰��Ǵ� ���ڿ� ���� �޾ƿ��� �����Ͱ� �޶�����
        // base, full, tan�� �߰��� �� �ִµ� (appdata_base �� ���� �ۼ��ϸ� �ȴ�!)
        // base�� position, normal and one texture coordinate
        // tan�� position, tagent, normal and one texture coordinate
        // full�� position, tangent, normal, four texture coordinates and color
        // ���� ���ؽ� �÷��� �ʿ���ٸ� _full�� �ʿ����� �ʴ�
        // �ܰ��� �÷��� �ٲٱ� ���� full�� �ҷ��Դ�
        void vert(inout appdata_full v) 
        {

            // ���ؽ� ���� + 1 ������Ʈ �������� �״���ε� �̹����� 1 ���� �̵�
            // v.vertex.y = v.vertex.y + 1;

            // �ð��� ���� ���Ʒ� �Դٰ��� �̵�
            // ���� �ڸ��� ���� �׸��ڰ� ���� �����°� �� �� �ִ�
            // �̴� �׸��ڿ��� ���� ������ ������ �ʾұ� �����̴�!
            v.vertex.y = v.vertex.y + sin(_Time.y);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
