using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _jumpPower = 10f; // ���� �Ŀ�

    private Rigidbody2D rigidbody2d; // �÷��̾� ��ü
    private Animator animator; // �÷��̾� �ִϸ�����
    private AudioSource audioSource; // ����� �����Ҹ�
    void Start()
    {
        // �������ڸ��� �ڽ��� ������Ʈ ��������
        rigidbody2d = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameManager.instance.isGameover) // ���ӿ����� �ƴϸ�
        {
            // ����
            if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư�� ������
            {
                if (rigidbody2d != null) // ��ü�� �ִ� ���
                {
                    rigidbody2d.velocity = Vector2.up * _jumpPower; // _jumpPower��ŭ ���� �ӵ�
                }
                else // ��ü�� ������
                {
                    Debug.Log("�÷��̾ ��ü�� �����ϴ�."); // ��� ����
                }
            }

            // ���� ���� ��� ����
            // ���콺���� ���� ���� �ӵ� ����
            if (Input.GetMouseButtonUp(0) && rigidbody2d.velocity.y > 0) // ���콺 ���� ��ư���� ���� ���� y�� �ӵ��� ����� ��
            {
                if (rigidbody2d != null) // ��ü�� ������ 
                {
                    rigidbody2d.velocity *= 0.5f; // �ӵ� ����
                }
                else // ��ü�� ������
                {
                    Debug.Log("�÷��̾ ��ü�� �����ϴ�."); // ��� ����
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // �浹 �ϸ�
    {
        Die(); // ��� �޼���
    }

    // ������ �״� ���
    private void Die() // ��� �޼���
    {
        if (!GameManager.instance.isGameover) // �״� ��� 1���� ������ �ϱ����� ���� ����
                                              // ��Ϳ� �ε����� �� �� ��, �׸��� ������ ���̶� �ε����� �� �� ��
                                              // �׷��� �ּ� 2�� Die�޼��� �����ϰ� ������ �Ҹ� 2�� ������ �ȴ�.
        {
            if (audioSource != null) // ����� �ҽ��� ������
            {
                audioSource.Play(); // �Ҹ� ���
            }
            else
            {
                Debug.Log("�÷��̾ ����� �ҽ��� �����ϴ�.");
            }

            if (animator != null) // �ִϸ����Ͱ� ������
            {
                animator.SetTrigger("Die"); // ��� ���
            }
            else
            {
                Debug.Log("�÷��̾��� �ִϸ����Ͱ� �����ϴ�.");
            }

            GameManager.instance.Gameover(); // ���ӿ����� ����� ���� ����
        }
    }
}
