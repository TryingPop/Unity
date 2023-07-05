using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{

    public static PlayerManager instance;    // �̱���

    public string currentMapName;           // ���� �� �̸�

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

        // �� ����Ű -1, �� ����Ű 1 ����
        // �� ����Ű 1, �� ����Ű -1
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

        // �ڷ�ƾ ������ ���� ����� �Ҹ��ϱ⿡ �ڷ�ƾ ������ �ּ�ȭ
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

            // �¿츦 �ֿ켱���� ����
            if (vector.x != 0)
            {

                vector.y = 0;
            }

            // �ִϸ����Ϳ� ����Ʈ���� DirX, DirY�� ���� ����
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

                    // ���� ��ġ���� �ش� ����ŭ �̵�
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