Shader "Custom/Test/013"
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
        // Standard는 PBR 물리 기반 라이트 연산을 다하기에 무겁다
        // #pragma surface surf Standard fullforwardshadows

        // Help는 사용자 지정 이름 마음대로 해도된다
        // #pragma surface surf Help
        // ambient를 꺼주는 코드
        // 희뿌연 색이 사라진다
        // #pragma surface surf Help noambient

        #pragma surface surf Help noambient nofog
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {

            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // Standard를 이용안해서 SurfaceOutputStandard 구조체를 이용할 수 없다
        // 그래서 구형 라이트용 SurfaceOutput 구조체를 이용
        // struct SurfaceOutput
        // {
        // 
        //     fixed3 Albedo;       // diffuse color
        //     fixed3 Normal;       // tangent space normal
        //     fixed3 Emission;
        //     half Specular;       // specular power in 0 ... 1 range
        //     fixed Gloss;         // specular intensity
        //     fixed Alpha;         // alpha for transparencies
        // 
        // }
        // void surf(Input IN, inout SurfaceOutputStandard o)
        void surf(Input IN, inout SurfaceOutput o)
        {

            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Emission = c.rgb;
        }

        // 유니티에 내장된 것이 아니므로 해당 함수가 필요하다
        // SrufaceOutput은 표면 셰이더로 여러 가지 코드가 압축
        // lightDir은 빛의 방향을 확인시켜주는 셰이더
        // atten은 라이트의 감쇠값으로 빛의 강도에 따라 라이트 감쇠 효과와 그림자를 표현할 수 있게 해주는 변수
        float4 LightingHelp(SurfaceOutput s, float3 lightDir, float atten) 
        {

            // 어떠한 빛의 영향도 받지 않는 가장 가벼운 Unlit 쉐이더가 완성
            // 이펙트를 만들 때 주로 쓰는 쉐이더
            // nofog까지 하면 더 좋다
            return float4(s.Albedo.rgb, 1);


            // 여기에 emission을 반환하면 emission이 한 번 더 연산되어 더 밝아보인다
            // return float4(s.Emission.rgb, 1);
            // 비교용 코드
            // return float4(0, 0, 0, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
