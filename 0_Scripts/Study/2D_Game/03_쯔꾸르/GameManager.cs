using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 씬 이동간 파괴되면 안된다!
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

        // 다른 객체들이 로드 될 때까지 대기
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
