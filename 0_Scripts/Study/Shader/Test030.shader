// 2pass 하는법
// 먼저 CGPROGRAM 에서 ENDCG까지 복사해서 붙여넣는다
Shader "Custom/Test/030"
{
    Properties
    {

        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        // 면 뒤집기
        // 큐브에 넣어보면 앞 면은 투명해지고 뒷면이 드러난다
        // 스피어에서는 확인이 힘들다
        cull front

        /// 
        /// 1pass
        /// 
        CGPROGRAM
        #pragma surface surf Lambert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG


        /// 
        /// 2pass
        /// 
        // forward render에서만 가능한 기법
        // 언리얼에서는 외각선을 Pos-tprocess로 처리한다고 한다
        CGPROGRAM
        #pragma surface surf Lambert

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
