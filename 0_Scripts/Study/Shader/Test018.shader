Shader "Custom/Test/018"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _Bump("Normal Map", 2D) = "bump" {}

        // 림라이트
        _RimPow("Rim Power", Range(1, 10)) = 5
        // HDR은 인터페이스에서 HDR을 조절해 블룸 효과를 넣을 수 있다
        // 색에 기능 확장 어트리뷰트로 보면 될거 같다
        [HDR] _RimColor("Rim Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Bump;

        // 림라이트
        float _RimPow;
        float4 _RimColor;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_Bump;
            float3 viewDir;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float3 Bump = UnpackNormal(tex2D(_Bump, IN.uv_Bump));

            o.Normal = Bump;

            float rim = saturate(dot(o.Normal, IN.viewDir));
            // 값이 작아질 수록 외각이 커진다
            rim = pow(1 - rim, _RimPow);

            o.Emission = rim * _RimColor.rgb;
            o.Alpha = MainTex.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
