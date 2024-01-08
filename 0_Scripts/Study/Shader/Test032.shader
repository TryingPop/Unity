// 2pass �ϴ¹�
// ���� CGPROGRAM ���� ENDCG���� �����ؼ� �ٿ��ִ´�
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
        // �� ������
        // ť�꿡 �־�� �� ���� ���������� �޸��� �巯����
        // ���Ǿ���� Ȯ���� �����
        cull front

        /// 
        /// 1pass
        /// 
        // �ܰ������� ���Ǵ� 1pass�� 2pass���� Ŀ����
        // 1pass�� 2pass�� �������� �ʰ� ǥ���ȴ�
        // �ܰ����� ������ �׸��� ���� �� ������� ���� ��� ��� �����ؼ� �ִ��� ������ ����� �ִ°� ����
        CGPROGRAM
        // ���ؽ� ���̴� ���� �׸��� �Ⱦ���
        #pragma surface surf Test032 vertex:vert noshadow
        #pragma target 3.0

        sampler2D _MainTex;
        float _OutLine;

        struct Input
        {

            float4 color:COLOR;
        };

        // ������ ����Ƽ���� �������;
        void surf(Input IN, inout SurfaceOutput o) { }

        void vert(inout appdata_full v)
        {

            // �븻 �������� ���ؽ� �̵� (�ܰ��� ����)
            v.vertex.xyz = v.vertex.xyz + v.normal * _OutLine;
        }

        // ���⼭ ���������� �־��ش�
        float4 LightingTest032(SurfaceOutput s, float3 lightDir, float atten)
        {

            return float4(0, 0, 0, 1);
        }
        ENDCG


        /// 
        /// 2pass
        /// 
        // forward render������ ������ ���
        // �𸮾󿡼��� �ܰ����� Pos-tprocess�� ó���Ѵٰ� �Ѵ�
        // 2pass�� �ٽ� �ո鿡 ����
        // �տ��� �������� ������ �ٽ� ����� �������� ������
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

        // ����� ������ ������ �ҷ��´�
        float4 LightingTest032Ceil(SurfaceOutput s, float3 lightDir, float atten)
        {

            float ndotl = dot(normalize(s.Normal), normalize(lightDir)) * 0.5 + 0.5;        // ���� ����Ʈ
            ndotl = ndotl * ndotl * ndotl;

            // if���� ����� ������ ��ġ�� ���� �����ϱ� ������ else ������ �������� �����ؾ��Ѵ�
            if (ndotl > 0.6) ndotl = 1;
            else if (ndotl > 0.3) ndotl = 0.5;
            else ndotl = 0.1f;

            // ���� ������ �������� ������ �شٸ� ceil(�ø�) �Լ��� �̿��ϸ� �ȴ�
            // ndotl = ceil(ndotl * 4) / 4;
            return ndotl;

            /*
            // ���� �ִ� ���
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
