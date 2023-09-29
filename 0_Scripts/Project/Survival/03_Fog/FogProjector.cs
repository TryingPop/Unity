using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogProjector : MonoBehaviour
{

    // 렌더 텍스처
    public RenderTexture fogTexture;

    // 잔상 표현을 위한 두 렌더 텍스처
    RenderTexture projecTexture;
    RenderTexture oldTexture;

    public Shader blurShader;

    [Range(1, 4)]
    public int upsample = 2;

    Material blurMaterial;
    public float blur = 1f;

    Projector projector;

    public float blendSpeed = 1f;
    float blend;
    int blendNameId;

    private void OnEnable()
    {

        projector = GetComponent<Projector>();

        blurMaterial = new Material(blurShader);
        blurMaterial.SetVector("_Parameter", new Vector4(blur, -blur, 0, 0));

        // 새로운 렌더 텍스처 객체 생성
        projecTexture = new RenderTexture(
        fogTexture.width * upsample,
                            fogTexture.height * upsample,
                            0,
                            fogTexture.format)
        { filterMode = FilterMode.Bilinear };


        oldTexture = new RenderTexture(
                        fogTexture.width * upsample,
                        fogTexture.height * upsample,
                        0,
                        fogTexture.format)
        { filterMode = FilterMode.Bilinear };

        // 두 텍스처를 각각 _FogTex, _OldFogTex에 할당하도록 한다.
        projector.material.SetTexture("_FogTex", projecTexture);
        projector.material.SetTexture("_OldFogTex", oldTexture);

        // 셰이더의 _Blend 변수를 연결하고 현재 안개 텍스처를 projectTexture로 옮긴다.
        blendNameId = Shader.PropertyToID("_Blend");
        blend = 1;
        projector.material.SetFloat(blendNameId, blend);
        Graphics.Blit(fogTexture, projecTexture);
        UpdateFog();
    }

    // 두 텍스처를 섞어서 잔상을 구현하는 함수
    public void UpdateFog()
    {
        Graphics.Blit(projecTexture, oldTexture);
        Graphics.Blit(fogTexture, projecTexture);

        RenderTexture temp = RenderTexture.GetTemporary(
            projecTexture.width,
            projecTexture.height,
            0,
            projecTexture.format);

        temp.filterMode = FilterMode.Bilinear;

        Graphics.Blit(projecTexture, temp, blurMaterial, 1);
        Graphics.Blit(temp, projecTexture, blurMaterial, 2);

        blend = 1f;
        projector.material.SetFloat(blendNameId, blend);

        RenderTexture.ReleaseTemporary(temp);
    }

    private void LateUpdate()
    {

        UpdateFog();
    }
}
