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

            // 버텍스 컬러 레드만 x축 이동
            // 따로 색상 설정 안하면 초기값으로 (1, 1, 1, 1)이 담겨서 x축 이동한다
            // v.vertex.x = v.vertex.x + ((sin(_Time.y) * 0.3) * v.color.r);
            // g 값이 있는 곳은 더 빠르게 이동
            // v.vertex.x = v.vertex.x + ((sin(_Time.y * 3 * v.color.g) * 0.1) * v.color.r);

            // lerp 함수를 써도 된다
            // lerp함수로 버텍스 무빙
            v. vertex.x = lerp(v.vertex.x, v.vertex.x + sin(_Time.y * 3), v.color.r);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
