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

    private void Start()
    {

        if (!flag)
        {

            theCamera = FindObjectOfType<CameraManager>();
        }
        thePlayer = FindObjectOfType<PlayerManager>();       // ���̶�Ű�� ��� ��ü�� ���� �ش� ������Ʈ�� �˻��ؼ� ����

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player")
        {

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
        }
    }
}
