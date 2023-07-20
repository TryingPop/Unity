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
    private FadeManager theFade;
    private Menu theMenu;           // 여기에 캔버스가 있다
    private DialogueManager theDM;  // 여기에 캔버스가 있다
    private Camera cam;

    public GameObject hpBar;
    public GameObject mpBar;

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
        theFade = FindObjectOfType<FadeManager>();

        theMenu = FindObjectOfType<Menu>();
        theDM = FindObjectOfType<DialogueManager>();
        cam = FindObjectOfType<Camera>();

        {

            SpriteRenderer playerSpriteRenderer = thePlayer.GetComponent<SpriteRenderer>();
            Color color = playerSpriteRenderer.color;
            color.a = 1f;
            playerSpriteRenderer.color = color;
        }


        theCamera.target = GameObject.Find("Player");

        // 이렇게해도 짤릴 수 있다
        // 그래서 크기를 키우면서 해결하는 방법이 있다
        theMenu.GetComponent<Canvas>().worldCamera = cam;
        theDM.GetComponent<Canvas>().worldCamera = cam;

        for (int i = 0; i < bounds.Length; i++)
        {

            if (bounds[i].boundName == thePlayer.currentMapName)
            {

                bounds[i].SetBound();
            }
        }

        hpBar.SetActive(true);
        mpBar.SetActive(true);

        theFade.FadeIn();
    }
}
