// 2pass 하는법
// 먼저 CGPROGRAM 에서 ENDCG까지 복사해서 붙여넣는다
Shader "Custom/Test/032"
{
    Properties
    {

        _OutLine("Out Line", Range(0, 1)) = 1
        _MainTex ("Main Texture", 2D) = "white" {}

        _Color1("Color 1", Color) = (0, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 0, 0, 1)
        _Color3("Color 3", Color) = (0, 0, 0, 1)
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
        // 외각선으로 사용되는 1pass는 2pass보다 커야지
        // 1pass가 2pass에 가려지지 않고 표현된다
        // 외각선의 역할을 그림자 연산 등 사용하지 않을 경우 모두 제거해서 최대한 가볍게 만들어 주는게 좋다
        CGPROGRAM
        // 버텍스 쉐이더 쓰고 그림자 안쓴다
        #pragma surface surf Test032 vertex:vert noshadow
        #pragma target 3.0

        sampler2D _MainTex;
        float _OutLine;

        struct Input
        {

            float4 color:COLOR;
        };

        // 없으면 유니티에서 에러뜬다;
        void surf(Input IN, inout SurfaceOutput o) { }

        void vert(inout appdata_full v)
        {

            // 노말 방향으로 버텍스 이동 (외각선 조절)
            v.vertex.xyz = v.vertex.xyz + v.normal * _OutLine;
        }

        // 여기서 검정색으로 넣어준다
        float4 LightingTest032(SurfaceOutput s, float3 lightDir, float atten)
        {

            return float4(0, 0, 0, 1);
        }
        ENDCG


        /// 
        /// 2pass
        /// 
        // forward render에서만 가능한 기법
        // 언리얼에서는 외각선을 Pos-tprocess로 처리한다고 한다
        // 2pass는 다시 앞면에 쓴다
        // 앞에서 뒤집었기 때문에 다시 뒤집어서 원점으로 돌린다
        cull back
        CGPROGRAM
        #pragma surface surf Test032Ceil Lambert
        #pragma target 3.0

        sampler2D _MainTex;

        float4 _Color1;
        float4 _Color2;
        float4 _Color3;

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

        // 명암을 계층을 나눠서 불러온다
        float4 LightingTest032Ceil(SurfaceOutput s, float3 lightDir, float atten)
        {

            float ndotl = dot(normalize(s.Normal), normalize(lightDir)) * 0.5 + 0.5;        // 하프 램버트
            ndotl = ndotl * ndotl * ndotl;

            // if문은 계산을 끝까지 마치고 값을 도출하기 때문에 else 구문이 많아지면 조심해야한다
            if (ndotl > 0.6) ndotl = 1;
            else if (ndotl > 0.3) ndotl = 0.5;
            else ndotl = 0.1f;

            // 만약 일정한 간격으로 계층을 준다면 ceil(올림) 함수를 이용하면 된다
            // ndotl = ceil(ndotl * 4) / 4;
            return ndotl;

            /*
            // 색을 넣는 방법
            float4 tonColor;

            if (ndotl > 0.6) tonColor = _Color1;
            else if (ndotl > 0.3) tonColor = _Color2;
            else tonColor = _Color3;

            
            return tonColor;
            */
        }
 
        ENDCG
    }
    FallBack "Diffuse"
}
