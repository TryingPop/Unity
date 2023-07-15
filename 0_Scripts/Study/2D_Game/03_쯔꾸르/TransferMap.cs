using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;          // �̵��� �� �̸�

    public Transform target;
    public BoxCollider2D targetBound;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction;                // ĳ���Ͱ� �ٶ󺸰� �ִ� ����
    private Vector2 vector;                 // (GetFloat"DirX", GetFloat"DirY")

    [Tooltip("���� ������ : true, ���� ������ : f alse")]
    public bool door;                       // ���� �ִ°� ���°�?

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

    private void OnTriggerStay2D(Collider2D other)
    {

        if (door)
        {

            if (other.gameObject.name == "Player")
            {

                if (Input.GetKeyDown(KeyCode.Z))
                {

                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {

                        case "UP":
                            if (vector.y == 1f)
                            {

                                StartCoroutine(TransferCoroutine());
                            }
                            break;
                        case "DOWN":
                            if (vector.y == -1f)
                            {

                                StartCoroutine(TransferCoroutine());
                            }
                            break;
                        case "RIGHT":
                            if (vector.x == 1f)
                            {

                                StartCoroutine(TransferCoroutine());
                            }
                            break;
                        case "LEFT":
                            if (vector.x == -1f)
                            {

                                StartCoroutine(TransferCoroutine());
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
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

        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();

        if (door)
        {

            anim_1.SetBool("Open", true);

            if (door_count == 2)
            {

                anim_2.SetBool("Open", true);
            }
        }
        yield return new WaitForSeconds(0.3f);

        theOrder.SetTransparent("player");

        if (door)
        {

            anim_1.SetBool("Open", false);

            if (door_count == 2)
            {

                anim_2.SetBool("Open", false);
            }
        }

        yield return new WaitForSeconds(0.7f);

        theOrder.SetUnTransparent("player");

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
