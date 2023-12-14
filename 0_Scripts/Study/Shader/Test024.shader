// 지금까지 배운 라이트 쉐이더를 전부 합쳐본다
Shader "Custom/Test/024"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _BumpTex("Normal Texture", 2D) = "bump" {}

        // 림 라이트(Fresnel)
        _RimPow("Rim Power", Range(1, 10)) = 5
        [HDR] _RimCol("Rim Color", Color) = (1, 1, 1, 1)

        // Specular
        _SpPow("Specular Power", Range(1, 100)) = 50
        // _SpecColor 는 유니티 내장변수로 이미 선언되어 있다! 그래서 중복 안되게 다른 이름 써야한다!
        [HDR] _SpCol("Specular Color", Color) = (1, 1, 1, 1)
        // 원하는 부분만 스펙큘러 적용 되게 선언한 텍스쳐
        _GlossTex("Sepcular Texture", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test024
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpTex;
        sampler2D _GlossTex;


        float _RimPow;
        float4 _RimCol;

        float _SpPow;
        float4 _SpCol;

        struct Input
        {

            float2 uv_MainTex;
            float2 uv_BumpTex;
            float2 uv_GlossTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 MainTex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 BumpTex = tex2D(_BumpTex, IN.uv_BumpTex);
            fixed4 GlossTex = tex2D(_GlossTex, IN.uv_GlossTex);

            o.Normal = UnpackNormal(BumpTex);
            // Specular에 사용할 텍스쳐를 넣어준다
            o.Gloss = GlossTex.a;
            o.Albedo = MainTex.rgb;
            o.Alpha = MainTex.a;
        }

        // 매개변수 순서 중요!
        float4 LightingTest024(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) 
        {

            // Diffuse
            // Half Lambert
            float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
            ndotl = pow(ndotl, 3);
            // _LightColor0는 내장변수이고 
            // 라이트 칼라와 강도를 보관하고 있다! Test013에 있다!
            float3 diffCol = s.Albedo * ndotl * _LightColor0.rgb * atten;

            // Rim
            float rim = saturate(dot(viewDir, s.Normal));
            rim = pow(1 - rim, _RimPow);
            // RimCol이 라이트 컬러를 받아와서 보다 자연스럽게 맺히게 하기 위해 _LightColor0을 곱해줬다고 한다
            float3 rimCol = rim * _RimCol.rgb * _LightColor0.rgb;

            // Specular
            // Blin-Phong Shading
            float3 halfVec = normalize(viewDir + lightDir);
            float spec = saturate(dot(s.Normal, halfVec));

            // 코드를 보면 surf가 먼저 실행되고 Lighting이 뒤에 실행 되는거 같다
            // 앞에서 저장한 텍스쳐를 가져와 Specular를 표시한다
            // 0인 부분은 Specular가 표시되지 않고, 1인 부분은 선명하게 나타난다
            spec = pow(spec, _SpPow) * s.Gloss;
            float3 speCol = spec * _LightColor0.rgb * _SpCol;

            // Final
            float4 finalColor;
            
            // RimCol에 [HDR] 어트리뷰트를 선언해줬기에 1 이상의 컬러값을 사용할 수 있다
            finalColor.rgb = diffCol + rimCol + speCol;
            finalColor.a = s.Alpha;

            return finalColor;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
