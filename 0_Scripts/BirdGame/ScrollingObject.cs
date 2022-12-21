using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] // �⺻ Ÿ�Ե��� private�� ����Ƽ���� �����ְ� �ϴ� ��Ʈ����Ʈ 
    private float _moveSpeed = 2f; // ����� �̵��ӵ�
    [SerializeField]
    private float _delTime = 5f;

    private Animator animator; // ��� ���
    private bool _isDead; // ��ü �̵� ���� �Ǻ�
    private void Start()
    {
        Destroy(gameObject, _delTime); // ��ֹ� 5�� �� �ı�
                                 // 5�ʴ� ȭ�� �������� ���ʿ� ����� �ð� 
                                 // ��� �޸𸮸� ���� �Ҵ��ؼ� ��ȿ����? ���� ������ �ϴ� ����
                                 // ���� ���������� ���Ϸ� ������� �����ϱ�

        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (!_isDead) // �����ϸ�
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed; // Ʈ���� ������ ��ü �̵�
        }
    }

    // �浹�� �״� ��� 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.Log("��ֹ��� �ִϸ����Ͱ� �����ϴ�.");
        }
    }
}
