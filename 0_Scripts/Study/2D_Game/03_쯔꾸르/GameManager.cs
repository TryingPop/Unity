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
    private FadeManager theFade;
    private Menu theMenu;           // ���⿡ ĵ������ �ִ�
    private DialogueManager theDM;  // ���⿡ ĵ������ �ִ�
    private Camera cam;

    public GameObject hpBar;
    public GameObject mpBar;

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

        // �̷����ص� ©�� �� �ִ�
        // �׷��� ũ�⸦ Ű��鼭 �ذ��ϴ� ����� �ִ�
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
