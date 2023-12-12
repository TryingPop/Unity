Shader "Custom/Test/022"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texutre", 2D) = "bump" {}
        _MaskTex("Mask Texture", 2D) = "white" {}

        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1, 1)

        _HoloLineNum("Holo Line Num", float) = 1
        _HoloSpd("Holo Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;
        sampler2D _MaskTex;

        float _RimPow;
        float4 _RimCol;

        float _HoloLineNum;
        float _HoloSpd;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;
            float2 uv_MaskTex;

            float3 viewDir;
            float3 worldPos;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            // 마스크 쉐이더가 나왔다가 사라진다!
            float4 MaskTex = tex2D(_MaskTex, (IN.uv_MaskTex.x, IN.uv_MaskTex.y - _Time.y * _HoloSpd));

            float3 BumpTex = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));

            // 림 라이트
            float rim = saturate(dot(o.Normal, IN.viewDir));
            rim = pow(1 - rim, _RimPow);

            o.Emission = _RimCol.rgb;
            o.Alpha = (MaskTex * rim) * abs(sin(_Time.y));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
