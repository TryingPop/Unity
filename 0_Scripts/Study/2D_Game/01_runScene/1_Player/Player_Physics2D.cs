using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Physics2D : MonoBehaviour
{

    // ����
    // Inspector���� �����ϱ� ���� �Ӽ�
    public float speed = 12.0f;         // �÷��̾� ĳ������ �ӵ�
    public float jumpPower = 500.0f;    // �÷��̾� ĳ������ ���������� ���� �Ŀ�
    public Sprite[] run;                // �÷��̾� ĳ������ �޸��� ��������Ʈ
    public Sprite[] jump;               // �÷��̾� ĳ������ ���� ��������Ʈ

    // ���ο��� �ٷ�� ����
    int animIndex;                      // �÷��̾� ĳ���� �ִϸ��̼� ��� �ε���
    bool grounded;                      // ���� üũ
    bool goalCheck;                     // �����ߴ��� üũ
    float goalTime;                     // ���� Ÿ��

    // �߰��� �Լ�
    Rigidbody2D rigidbody2D;
    SpriteRenderer image;
    GameObject goCam;

    // �޽����� ������ �ڵ�

    // ������Ʈ ���� ����
    void Start()
    {

        // �ʱ�ȭ
        animIndex = 0;
        grounded = false;
        goalCheck = false;

        // 
        rigidbody2D = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
        goCam = Camera.main.gameObject;         // Main Camera �±׷� ��ϵ� ������Ʈ�� �����´�
    }

    // �÷��̾� ĳ���Ϳ� ����� �浹 ���� ������ �ٸ� ���� ������Ʈ�� �浹 ���� ������ ������ ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // ���Դ��� Ȯ��
        if (collision.gameObject.name == "Stage_Gate")
        {

            // ���Դ�
            goalCheck = true;
            goalTime = Time.timeSinceLevelLoad;
        }
    }

    // ������ �ٽ� ����
    void Update()
    {

        // ���Դ��� Ȯ��
        Transform groundCheck = transform.Find("GroundCheck");
        grounded = (Physics2D.OverlapPoint(groundCheck.position) != null) ? true : false;
        if (grounded)
        {

            // ����
            if (Input.GetButtonDown("Fire1"))
            {

                // ���� ó��
                rigidbody2D.AddForce(new Vector2(0.0f, jumpPower));

                // ���� ��������Ʈ �̹����� ��ȯ
                // GetComponent<SpriteRenderer>().sprite = jump[0];
                image.sprite = jump[0];
            }
            else
            {

                // �޸��� ó��
                animIndex++;

                if (animIndex >= run.Length)
                {

                    animIndex = 0;
                }
            }

            // �޸��� ��������Ʈ �̹����� ��ȯ
            // GetComponent<SpriteRenderer>().sprite = run[animIndex];
            image.sprite = run[animIndex];
        }

        // ���ۿ� �����°�?
        if (transform.position.y < -10.0f)
        {

            // ���ۿ� �����ٸ� ���������� �ٽ� �о� �ʱ�ȭ�Ѵ�
            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ������ �ٽ� ����
    private void FixedUpdate()
    {

        // �̵� ���
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

        // ī�޶� �̵�
        // GameObject goCam = GameObject.Find("Main Camera");
        goCam.transform.position = new Vector3(
            transform.position.x + 5.0f, goCam.transform.position.y, goCam.transform.position.z);
    }

    // ����Ƽ GUI ǥ��
    private void OnGUI()
    {

        // ����� �ؽ�Ʈ
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 A]\n���콺 ���� ��ư�� ������ ����\n������ ����!]");

        if (goalCheck)
        {

            GUI.TextField(new Rect(10, 100, 330, 60),
                string.Format("***** Goal!! *****\nTime {0}", goalTime));
        }

        // ���� ��ư
        if(GUI.Button(new Rect(10, 80, 100, 20), "����"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // �޴��� ���ư���
        if(GUI.Button(new Rect(10, 110, 100, 20), "�޴�"))
        {

            // Application.LoadLevel("SelectMenu");
            SceneManager.LoadScene("SelectMenu");
        }
    }
}

