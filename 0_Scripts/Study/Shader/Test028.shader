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
        // 램버트 기법과 좌표 정보 받아오기, 그리고 버텍스 연산 그림자에게도 카피한다
        // 027과 비교하면 028 마테리얼은 그림자도 이동하는 것을 알 수 있다
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

        // 버텍스 연산
        void vert(inout appdata_full v) 
        {

            v.vertex.y = v.vertex.y + sin(_Time.y);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
