// 월드 좌표를 쉐이더로 받아오기!!
Shader "Custom/Test/020"
{
    Properties
    {


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend
        #pragma target 3.0

        struct Input
        {

            // 월드 좌표 값을 받아온다
            float3 worldPos;
        };


        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            // 월드 좌표에 따라 색이 바뀐다
            // 0, 0, 0 에서 확인 가능
            o.Emission = IN.worldPos;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
