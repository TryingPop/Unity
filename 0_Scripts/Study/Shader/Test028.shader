Shader "Custom/Test/028"
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
        // ����Ʈ ����� ��ǥ ���� �޾ƿ���, �׸��� ���ؽ� ���� �׸��ڿ��Ե� ī���Ѵ�
        // 027�� ���ϸ� 028 ���׸����� �׸��ڵ� �̵��ϴ� ���� �� �� �ִ�
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

        // ���ؽ� ����
        void vert(inout appdata_full v) 
        {

            v.vertex.y = v.vertex.y + sin(_Time.y);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
