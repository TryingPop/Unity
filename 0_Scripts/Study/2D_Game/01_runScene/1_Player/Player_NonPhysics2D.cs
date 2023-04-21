using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_NonPhysics2D : MonoBehaviour
{

    // ����
    public float speed = 3.0f;      // �÷��̾� ĳ������ �ӵ�
    // public float speed = 15.0f;  // ù ��° ��ֹ� �Ÿ��� �ʹ� ���� ���� ������ ���� �ߴ�
    public Sprite[] run;            // �÷��̾� ĳ������ �޸��� ��������Ʈ
    public Sprite[] jump;           // �÷��̾� ĳ������ ���� ��������Ʈ

    // ���ο��� �ٷ�� ����
    float jumpVy;                   // �÷��̾� ĳ������ ��� �ӵ�
    int animIndex;                  // �÷��̾� ĳ���� �ִϸ��̼� ��� �ε���
    bool goalCheck;                 // �����ߴ��� üũ

    GameObject goCam;               // �߰��� ���� ���� ī�޶�

    // �޽����� ������ �ڵ�

    // ������Ʈ ���� ����
    void Start()
    {

        // �ʱ�ȭ
        jumpVy = 0;
        animIndex = 0;
        goalCheck = false;

        goCam = GameObject.Find("Main Camera");
    }

    // �÷��̾� ĳ���Ϳ� ����� �浹 ���� ������ �ٸ� ���� ������Ʈ�� �浹 ���� ������ ���ƴ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // ���Դ��� �˻�
        if (collision.gameObject.name == "Stage_Gate")
        {

            goalCheck = true;
            return;
        }

        // ���� ������ �ƴ� ���̶�� �ʱ�ȭ �ؾ��Ѵ�
        // Application.LoadLevel(Application.loadedLevelName);  // ���� �Ⱦ��� �ڵ�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ������ �ٽ� ����
    void Update()
    {
        
        // ���Դ��� �˻�
        if (goalCheck)
        {

            return; // ���Դٸ� ó���� �����
        }

        // ���� �÷��̾� ĳ���Ѱ� ��� ���̿� �ִ��� ���
        // float height = transform.position.y + jumpVy;    // �� �� ������ 20�̻� �پ���� �� �ִ�
        float height = transform.position.y + jumpVy * Time.deltaTime;

        if (height <= 0.0f)
        {

            // ���� �ʱ�ȭ
            height = 0.0f;
            jumpVy = 0.0f;


            // ���� Ȯ��
            if (Input.GetButtonDown("Fire1"))
            {

                // ���� ó��
                jumpVy = + 7.5f;    // �� ��Ȳ�� �°� �� ����
                // jumpVy = +1.3f;  // �ݹ� ���� ��´�

                // ���� ��������Ʈ �̹����� ��ȯ
                GetComponent<SpriteRenderer>().sprite = jump[0];
            }
            else
            {

                // �޸��� ó��
                animIndex++;
                if (animIndex >= run.Length)
                {

                    animIndex = 0;
                }

                // �޸��� ��������Ʈ �̹����� ��ȯ
                GetComponent<SpriteRenderer>().sprite = run[animIndex];
            }
        }
        else
        {

            // ���� �� �������� ����
            // jumpVy -= 0.2f;  
            jumpVy -= 6.0f * Time.deltaTime;     // ����� �� ó��
        }

        // �÷��̾� ĳ���� �̵�(��ǥ ����)
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, height, 0.0f);
        // �Ʒ��� ���� ��������� �̵��ϵ��� �ص� �ȴ�
        // transform.Translate(speed* Time.deltaTime, jumpVy, 0.0f);
        // transform.position += new Vector3(speed* Time.deltaTime, jumpVy, 0.0f);
        // �� ������ ���� ������δ� �������� �����Ƿ� �����ؾ� �Ѵ�
        // transform.position.Set(transform.position.x + speed * Time.deltaTime, height, 0.0f);

        // ī�޶� �̵� (��ǥ�� ��������� �̵���Ŵ)
        // GameObject goCam = GameObject.Find("Main Camer");    // �� �����Ӹ��� Find �ϴ°� ���� ���� ��� �Ծ� ���� ���� ����
        goCam.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        // Ȥ�� ī�޶� ������Ʈ�� ���� ī�޶� �±׸� ����� �� Ŭ���� ������ �̿�
        // Camera.main.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        
    }

    // ����Ƽ GUI ǥ��
    private void OnGUI()
    {

        // ����� �ؽ�Ʈ
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 A]\n���콺 ���� ��ư�� ������ ����\n������ ����!");

        // ���� ��ư
        if(GUI.Button(new Rect(10, 80, 100, 20), "����"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
