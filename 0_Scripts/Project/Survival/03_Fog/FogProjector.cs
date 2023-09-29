using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogProjector : MonoBehaviour
{

    // ���� �ؽ�ó
    public RenderTexture fogTexture;

    // �ܻ� ǥ���� ���� �� ���� �ؽ�ó
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

        // ���ο� ���� �ؽ�ó ��ü ����
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

        // �� �ؽ�ó�� ���� _FogTex, _OldFogTex�� �Ҵ��ϵ��� �Ѵ�.
        projector.material.SetTexture("_FogTex", projecTexture);
        projector.material.SetTexture("_OldFogTex", oldTexture);

        // ���̴��� _Blend ������ �����ϰ� ���� �Ȱ� �ؽ�ó�� projectTexture�� �ű��.
        blendNameId = Shader.PropertyToID("_Blend");
        blend = 1;
        projector.material.SetFloat(blendNameId, blend);
        Graphics.Blit(fogTexture, projecTexture);
        UpdateFog();
    }

    // �� �ؽ�ó�� ��� �ܻ��� �����ϴ� �Լ�
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
