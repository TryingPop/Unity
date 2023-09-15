Shader "Custom/FOWShader"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _MainTex("AlphaZero (RGB) Trans (A)", 2D) = "white" {}
        _MainTex2("Gray (RGB) Trans (A)", 2D) = "white" {}
    }
    SubShader
    {
        // Tags { "RenderType"="Opaque" }
        // LOD 200

        CGPROGRAM
        // surf 함수를 이용 lambert는 라이팅 모델 
        #pragma surface surf Lambert alpha:blend

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color = (0,0,0,1);

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 colorFocus = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 colorGray = tex2D(_MainTex2, IN.uv_MainTex);

            o.Albedo = _Color;
            float alpha = 1.0f - colorFocus.g - colorGray.g / 4;
            o.Alpha = alpha;
        }
        ENDCG
    }
}
