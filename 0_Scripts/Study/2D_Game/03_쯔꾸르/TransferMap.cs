using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{

    public string transferMapName;          // 이동할 맵 이름

    public Transform target;
    public BoxCollider2D targetBound;

    public Animator anim_1;
    public Animator anim_2;

    public int door_count;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction;                // 캐릭터가 바라보고 있는 방향
    private Vector2 vector;                 // (GetFloat"DirX", GetFloat"DirY")

    [Tooltip("문이 열린다 : true, 문이 없으면 : f alse")]
    public bool door;                       // 문이 있는가 없는가?

    public bool flag = true;                // 씬변환 체크 변수
                                            // 이 변환에서 주의할 껀 재생성되면서
                                            // true로 할당되어 씬 이동 시에 적용안될 수도 있기 때문에
                                            // 따로 스크립트를 만들어야 한다
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
        thePlayer = FindObjectOfType<PlayerManager>();       // 하이라키의 모든 객체에 대해 해당 컴포넌트를 검색해서 리턴
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


        // 초기화 주의!
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
