// NPR과 Cel Shading
// NPR은 Non Photorealistic Rendering의 약자로 비사실적 렌더링이라한다
// NR(Photorealistic Rendering, 사실적 렌더링)이 사진과 똑같이 만들어내는 것을 목표로 했다면,
// NPR은 회화나 드로잉, 도해, 만화처럼 인공적인 양식에 초점을 맞춘 그래픽 스타일이다
// 대표적인 예로 Cel Shading/Toon Shading이 있다

// 외각선을 만드는 방법

// 1. 2Pass
// 2Pass는 두 번 그려서 외각선을 만든다
// 버텍스를 노말 방향으로 확장
// 곡선으로 이루어진 오브젝트에서는 자연스럽게 외각선이 생성되지만
// 버텍스가 여러 개 뭉쳐서 구현이 되는 하드엣지의 경우 노말 방향으로 버텍스가 확대되기에 모서리가 벌어지며,
// 플렌처럼 뒷면이 없는 경우엔 외각선이 생성되지 않는다
// 또한 두 번 그리기에 드로우 콜이 늘어난다!

// 2. Fresnel (Rim)을 이용
// 외각선을 날카롭게 만들어 적용하는 기법,
// 선의 굵기가 일정하지 않다
// 또한 Normal Vector와 View Vector의 내적값으로 만들어진 외각선이기에 평평한 면에서는 작동하지 않는다

// 3. Post - Processing (후처리)
// 버퍼를 여러 번 겹쳐 외각선을 뽑아내거나, 소벨 마스크/라플라스 필터 등의 외각선 검출 필터를 통해
// 깊이 값과 노말값을 이용하여 외각선을 긜ㄹ 수 있다
// 외각선 검출 필터를 사용했을 때, 앞에 물체 등이 생긴다던가 깊이 값이 달라지면 문제가 발생
// 적절히 섞어서 사용해야하며, 무겂다는 단점이 있다

// 여기까지가 
// https://celestialbody.tistory.com/13

// 랜더링 파이프라인 (Rendering Pipeline)
// 3차원 이미지를 2차원 레스터 이미지로 표현하기 위한 단계적인 방법으로,
// 데이터들을 모니터에 출력하기 까지의 과정

// 랜더링 파이프라인은
// 가장 먼저 버텍스 정보(Position, Normal, Color 등)을 불러온 뒤 [정점 데이터]
// 해당 정보를 바탕으로 버텍스를 이어 삼각형의 폴리 곤으로 만들어 준다 [삼각형 분할]
// 그 다음으로 화면에 메쉬가 어떻게 보여질지 결정되는 것이 정점 쉐이더(Vertex Shader) 부분에서 이뤄진다
// 로컬 좌표가 월드 좌표에 적용이 되고 그게 또 카메라에 적용되고 등

// 이 정보를 바탕으로 화면에서 짤렸는지 안짤렸는지 (클리핑) 뒷면인지 아닌지 판단(후면 선별)

// 레스터화는 벡터이미지를 픽셀이미지로 변환하는 것을 의미한다
// 렌더링 파이프라인에서 레스터화는 앞선 정보들을 모니터 픽셀 하나하나에 찍어내어 랜더링(출력) 하는 것을 의미

// 이 단계를 마쳐야 픽셀이 된다 여기서부터 2D!

// 다음 단계로 픽셀 쉐이더 단계가 있다
// 이전 까지 공부했던것은 픽셀 쉐이더(Pixel Shader / Surface Shader) 단계로 텍스처를 넣고 쉐이딩 연산을 하는 곳

// 이후 마지막으로 진행되는 알파테스트 / 블랜딩, 깊이, 포그 등등의 친구들이 있다

// 쉐이더 코드를 통해 우리가 조작할 수 있는 단계는 정점 쉐이더와 픽셀 쉐이더
// 정점 쉐이더는 3.0에서 가능
Shader "Custom/Test/026"
{
    Properties
    {


    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        // 픽셀 쉐이더
        void surf (Input IN, inout SurfaceOutputStandard o)
        {


        }
        ENDCG
    }
    FallBack "Diffuse"
}
