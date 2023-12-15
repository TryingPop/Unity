// vertex 쉐이더!
Shader "Custom/Test/027"
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
        // 2 pass 로 외각선을 만들기 위해 Vertex shader를 가동
        #pragma surface surf Lambert vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;

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
            o.Alpha = MainTex.a;
        }

        // #pragma에 vertex:vert를 추가했으므로 다음 함수를 정의해줘야한다!
        // appdata_ 뒤에 추가되는 문자에 따라 받아오는 데이터가 달라진다
        // base, full, tan을 추가할 수 있는데 (appdata_base 와 같이 작성하면 된다!)
        // base는 position, normal and one texture coordinate
        // tan은 position, tagent, normal and one texture coordinate
        // full은 position, tangent, normal, four texture coordinates and color
        // 만약 버텍스 컬러가 필요없다면 _full은 필요하지 않다
        // 외각선 컬러를 바꾸기 위해 full로 불러왔다
        void vert(inout appdata_full v) 
        {

            // 버텍스 값에 + 1 오브젝트 포지션은 그대로인데 이미지는 1 위로 이동
            // v.vertex.y = v.vertex.y + 1;

            // 시간에 따라 위아래 왔다갔다 이동
            // 원래 자리에 가면 그림자가 묻어 나오는걸 볼 수 있다
            // 이는 그림자에는 따로 연산을 해주지 않았기 때문이다!
            v.vertex.y = v.vertex.y + sin(_Time.y);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
