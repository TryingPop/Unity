// 외곽선만 보이는 홀로그램
Shader "Custom/Test/021"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}
        
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1, 1)
        _RimPow("Rim Power", Range(1, 10)) = 5

        _HoloSpd("Holo Speed", float) = 1
        _HoloLineNum("Holo Line Num", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;

        float _RimPow;
        float4 _RimCol;

        float _HoloSpd;
        float _HoloLineNum;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;

            float3 viewDir;
            float3 worldPos;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            float3 BumpTex = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));

            o.Normal = BumpTex;

            float rim = saturate(dot(o.Normal, IN.viewDir));
            rim = pow(1 - rim, _RimPow);

            // frac은 소수값만 받아오는 함수
            // 100개의 줄무늬
            // float holo = pow(frac(IN.worldPos.y * 100 - _Time.y), 5);


            float holo = pow(frac(IN.worldPos.y * _HoloLineNum - _Time.y * _HoloSpd), 5);

            o.Emission = _RimCol.rgb;
            o.Alpha = (holo + rim) * abs(sin(_Time.y));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
