using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{

    public static PlayerManager instance;    // 싱글톤

    public string currentMapName;           // 현재 맵 이름

    public float runSpeed;

    private bool applyRunFlag = false;

    private bool canMove = true;


    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private AudioManager theAudio;


    private void Awake()
    {
        
        if (instance == null)
        {

            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    private void Start()
    {

        queue = new Queue<string>();

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        // audioSource = GetComponent<AudioSource>();

        theAudio = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {

        // 좌 방향키 -1, 우 방향키 1 리턴
        // 상 방향키 1, 하 방향키 -1
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            if (canMove)
            {

                StartCoroutine(MoveCoroutine());
                canMove = false;
            }
        }
    }

    IEnumerator MoveCoroutine()
    {

        // 코루틴 생성에 많은 비용을 소모하기에 코루틴 생성의 최소화
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {

                applySpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {

                applySpeed = walkSpeed;
                applyRunFlag = false;
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            // 좌우를 최우선으로 실행
            if (vector.x != 0)
            {

                vector.y = 0;
            }

            // 애니메이터에 블랜드트리로 DirX, DirY로 방향 설정
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            bool checkCollisionFlag = base.CheckCollision();

            if (checkCollisionFlag)
            {

                break;
            }

            /*
            // ..?
            if (base.CheckCollision())
            {

                break;
            }
            */

            animator.SetBool("Walking", true);

            int temp = Random.Range(1, 5);

            switch (temp)
            {

                case 1:
                    theAudio.Play(walkSound_1);
                    break;

                case 2:
                    theAudio.Play(walkSound_2);
                    break;

                case 3:
                    theAudio.Play(walkSound_3);
                    break;

                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {

                    // 현재 위치에서 해당 값만큼 이동
                    transform.Translate(vector.x * applySpeed * walkCount, 0, 0);
                }
                else if (vector.y != 0)
                {

                    transform.Translate(0, vector.y * applySpeed * walkCount, 0);
                }

                if (applyRunFlag) currentWalkCount++;

                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
        }

        animator.SetBool("Walking", false);
        currentWalkCount = 0;
        canMove = true;
    }
}