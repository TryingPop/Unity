Shader "Custom/Test011"
{
    Properties
    {

        _MainTex("Main Texture", 2D) = "white" {}
        _RTex("R Texutre", 2D) = "white" {}
        _GTex("G Texutre", 2D) = "white" {}
        _BTex("B Texutre", 2D) = "white" {}

        // �븻�� ����� �� ��� ������ �ȳ����� �� ������ �̻����� �� �ִ�
        _BumpMap("Normal", 2D) = "bump" {}

        _NP("Normal Power", float) = 1
        _SM("Smoothness", Range(0, 1)) = 0.5

        _RNormal("R Normal", 2D) = "bump" {}
        _RSM("R Smoothness", Range(0, 1)) = 0.5;
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // ���������� �ش� �ڵ尡 ��� �� ����� �� ������ ���⼭�� �ȵȴ�
        // �� ������ ����� OpenGL ES2.0�� �۵��ϴµ� 
        // �����������͸� �ִ� 8�������� �����ؼ�
        // ���� �ؽ��� 4���� ����ϰ� �ִ� ��Ȳ���� �븻�� �߰��� �ȵȴ�
        // https://celestialbody.tistory.com/8
        // ����Ʈ ����!
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RTex, _GTex, _BTex;

        sampler2D _BumpMap;

        float _NP;
        float _SM;

        sampler2D _RNormal;
        float _RSM;

        struct Input
        {
            float2 uv_MainTex;

            float2 uv_RTex, uv_GTex, uv_BTex;

            float2 uv_BumpMap;

            float2 uv_RNormal;

            float4 color:COLOR;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 MainTex = tex2D(_MainTex, IN.uv_MainTex);

            float4 RTex = tex2D(_RTex, IN.uv_RTex);
            float4 GTex = tex2D(_GTex, IN.uv_GTex);
            float4 BTex = tex2D(_BTex, IN.uv_BTex);


            o.Albedo = lerp(MainTex, RTex, IN.color.r);
            o.Albedo = lerp(o.Albedo, GTex, IN.color.g);
            o.Albedo = lerp(o.Albedo, BTex, IN.color.b);

            // Albedo�� Emission�� ��쿣 ������ �Լ� ���� �˸��� ������ �����͵� ������־���
            // Normal�� ���� �����Լ��� �ٿ���� �Ѵ�!
            // UnpackNormal�� �ؽ��Ŀ� �븻�� ���´ٰ� ���� �ȴ� Ÿ���� float3
            // float4 NormalTex = tex2D(_BumpMap, IN.uv_BumpMap);
            // o.Normal = UnpackNormal(NormalTex);

            // Ȥ�� ����ó�� �븻�� �־ �ȴ�
            // �븻�� �� �� ���̰� �Ϸ��� R, G ���� �ǵ帮�� �ȴ�, B�� �ǵ帮�� ���� ��ٰ� �Ѵ�
            float3 Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            Normal = float3(Normal.r * _NP, Normal.g * _NP, Normal.b);
            // o.Normal = Normal;
            
            // Textureó�� ������ ���ؽ����� �븻 ���� �ϴ� ����!
            float3 RNormal = UnpackNormal(tex2D(_RNormal, IN.uv_RNormal));
            RNormal = float3(RNormal.r * _NP, RNormal.g * _NP, RNormal.b);
            o.Normal = lerp(Normal, RNormal, IN.color.r);


            // ��ü�� ������ �������� ������Ƽ
            // Normal map�� Speculer�� ������ �� �������� �ʴ´�
            // ����Ƽ������ Speculer�� Smoothness��� �̸����� �Ҹ���
            // o.Smoothness = 0.5;
            
            // ���ؽ��� ���� �� �κп��� Smoothness ����
            o.Smoothness = lerp(_SM, _RSM, IN.color.r);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
