// Phong Shader
// Lambert에는 Specular가 없다
// Lambert에 Speculer 추가해보는게 목표!
Shader "Custom/Test/023"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test023
        #pragma target 3.0

        sampler2D 
        _MainTex;

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
            o.Alpha = 1;
        }

        // 해당 순서로 매개변수를 기입해야 viewDir을 불러올 수 있다
        float4 LightingTest023(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) 
        {

            // Speculer는 정반사를 의미하는데
            // 정 반사된 빛이 눈에 들어오면 Speculer가 선명해지고
            // 각도가 멀어질수록 Speculer가 흐려진다
            // 이를 코드로 구현하면 Phong Shading
            float3 ReV = reflect(normalize(-lightDir), normalize(s.Normal));    // -light를 뒤집는다 s.Normal을 기준으로
            float rdotv = saturate(dot(ReV, viewDir));                          // 보는 방향 반사된 빛을 내적

            rdotv = pow(rdotv, 20);                                             // 강도 조절
            return rdotv;

            // 하드웨어 발달로 Reflection Vector을 자유롭게 이용 가능하나
            // 이전에는 꽤 무거웠다고 한다
            // 그래서 대안으로 Blin-PhongSading 방법을 이용
            // Specular reflection 연산을 보다 빠르게 하기위해 다음과 같은 방법을 이용
            float3 h = normalize(viewDir + lightDir);                           // viewDir과 lightDir의 하프 벡터
            float spec = saturate(dot(s.Normal, h));                            // 노말과 하프벡터의 내적(스펙큘러)

            spec = pow(spec, 80);                                               // 강도 조절
            return spec;                                            

        }
        ENDCG
    }
    FallBack "Diffuse"
}
