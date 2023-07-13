using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;          // �̵��� �� �̸�

    public Transform target;
    public BoxCollider2D targetBound;

    public bool flag = true;                // ����ȯ üũ ����
                                            // �� ��ȯ���� ������ �� ������Ǹ鼭
                                            // true�� �Ҵ�Ǿ� �� �̵� �ÿ� ����ȵ� ���� �ֱ� ������
                                            // ���� ��ũ��Ʈ�� ������ �Ѵ�
    private CameraManager theCamera;
    private PlayerManager thePlayer;

    private FadeManager theFade;
    private OrderManager theOrder;

    private void Start()
    {

        if (!flag)
        {

            theCamera = FindObjectOfType<CameraManager>();
        }
        thePlayer = FindObjectOfType<PlayerManager>();       // ���̶�Ű�� ��� ��ü�� ���� �ش� ������Ʈ�� �˻��ؼ� ����
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player")
        {

            StartCoroutine(TransferCoroutine());

        }
    }

    IEnumerator TransferCoroutine()
    {

        theOrder.NotMove();
        theFade.FadeOut();

        yield return new WaitForSeconds(1f);

        thePlayer.currentMapName = transferMapName;


        // �ʱ�ȭ ����!
        if (flag)
        {

            SceneManager.LoadScene(transferMapName);
        }
        else
        {

            theCamera.SetBound(targetBound);
            theCamera.transform.position = new Vector3(
                this.transform.position.x, this.transform.position.y,
                theCamera.transform.position.z);

            thePlayer.transform.position = target.transform.position;
        }

        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);

        theOrder.Move();
    }
}
