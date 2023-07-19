using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� �̵��� �ı��Ǹ� �ȵȴ�!
/// </summary>
public class GameManager : MonoBehaviour
{

    private Bound[] bounds;
    private PlayerManager thePlayer;
    private CameraManager theCamera;

    public void LoadStart()
    {

        StartCoroutine(LoadWaitCoroutine());
    }

    IEnumerator LoadWaitCoroutine()
    {

        // �ٸ� ��ü���� �ε� �� ������ ���
        yield return new WaitForSeconds(0.5f);

        thePlayer = FindObjectOfType<PlayerManager>();
        bounds = FindObjectsOfType<Bound>();
        theCamera = FindObjectOfType<CameraManager>();

        theCamera.target = GameObject.Find("Player");
        
        for (int i = 0; i < bounds.Length; i++)
        {

            if (bounds[i].boundName == thePlayer.currentMapName)
            {

                bounds[i].SetBound();
            }
        }
    }
}
