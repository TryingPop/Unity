using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Physics2DAndMecanim : MonoBehaviour
{

    // ����
    // Inspector���� �����ϱ� ���� �Ӽ�
    public float speed = 12.0f;             // �÷��̾� ĳ������ �ӵ�
    public float jumpPower = 1600.0f;       // �÷��̾� ĳ���͸� ���������� ���� ��

    // ���ο��� �ٷ�� ����
    bool grounded;                          // ���� Ȯ��
    bool goalCheck;                         // ���Դ��� Ȯ��
    float goalTime;                         // ���� �ð�

    Rigidbody2D rigidbody2D;
    Transform groundCheck;
    Animator anim;
    GameObject goCam;

    // �޽����� ������ �ڵ�
    // ������Ʈ ���� ����
    void Start()
    {

        // �ʱ�ȭ
        grounded = false;
        goalCheck = false;

        rigidbody2D= GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        goCam = GameObject.Find("Main Camera");
        anim = GetComponent<Animator>();
    }

    // �÷��̾� ĳ���Ϳ� ����� �浹 ���� ������ �ٸ� ���� ������Ʈ�� �浹 ���� ������ ���ƴ�
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // ���Դ��� Ȯ��
        if(collision.gameObject.name == "Stage_Gate")
        {

            // ���Դ�
            goalCheck = true;
            goalTime = Time.timeSinceLevelLoad;
            
            anim.enabled = false;       // �ִϸ��̼� ������Ʈ ��Ȱ��ȭ�ؼ� �ִϸ��̼� ����
        }
    }

    // ������ �ٽ� ����
    void Update()
    {

        // ���鿡 ��Ҵ��� Ȯ��
        // Transform groundCheck = transform.Find("GroundCheck");
        grounded = (Physics2D.OverlapPoint(groundCheck.position) != null) ? true : false;

        if (grounded)
        {

            // ���� ��ư Ȯ��
            if (Input.GetButtonDown("Fire1"))
            {

                // ���� ó��
                rigidbody2D.AddForce(new Vector2(0.0f, jumpPower));
            }

            // �޸��� �ִϸ��̼� ����
            // GetComponent<Animator>().SetTrigger("Run");
            anim.SetTrigger("Run");
        }
        else
        {

            // GetComponent<Animator>().SetTrigger("Jump");
            anim.SetTrigger("Jump");
        }

        // ���ۿ� �����°�?
        if (transform.position.y < -10.0f)
        {

            // ���ۿ� �����ٸ� ���������� �ٽ� �о�鿩�� �ʱ�ȭ�Ѵ�
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
        goCam.transform.position = new Vector3(transform.position.x + 5.0f,
            goCam.transform.position.y, goCam.transform.position.z);
    }

    // ����Ƽ GUI ǥ��
    private void OnGUI()
    {

        // ����� �ؽ�Ʈ
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 C]\n���콺 ���� ��ư�� ������ ����!");

        if (goalCheck)
        {

            GUI.TextField(new Rect(10, 100, 330, 60),
                string.Format("***** Goal!! *****\nTime {0}", goalTime));
        }

        // �ʱ�ȭ�ϴ� ���� ��ư
        if (GUI.Button(new Rect(10, 80, 100, 20), "����"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*
        // �޴��� ���ư���
        if (GUI.Button(new Rect(10, 110, 100, 20), "�޴�"))
        {

            // Application.LoadLevel("SelectMenu");
            SceneManager.LoadScene("SelectMenu");
        }
        */
    }
}
