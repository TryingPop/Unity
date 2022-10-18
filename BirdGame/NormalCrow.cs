using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCrow : MonoBehaviour
{
    [SerializeField] // �⺻ Ÿ�Ե��� private�� ����Ƽ���� �����ְ� �ϴ� ��Ʈ����Ʈ 
    private float _moveSpeed = 2f; // ����� �̵��ӵ�

    private Animator animator; // ��� ���
    private bool _isDead; // ��ü �̵� ���� �Ǻ�
    private void Start()
    {
        animator = GetComponent<Animator>(); // �ִϸ����� �޾ƿ���
        Destroy(gameObject, 5f); // ��ֹ� 5�� �� �ı�
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
