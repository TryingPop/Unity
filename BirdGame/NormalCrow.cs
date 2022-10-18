using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCrow : MonoBehaviour
{
    [SerializeField] // 기본 타입들은 private라도 유니티에서 보여주게 하는 어트리뷰트 
    private float _moveSpeed = 2f; // 기둥의 이동속도

    private Animator animator; // 사망 모션
    private bool _isDead; // 물체 이동 여부 판별
    private void Start()
    {
        animator = GetComponent<Animator>(); // 애니메이터 받아오기
        Destroy(gameObject, 5f); // 장애물 5초 후 파괴
    }

    void Update()
    {
        if (!_isDead) // 생존하면
        {
            transform.position += Vector3.left * Time.deltaTime * _moveSpeed; // 트랜스 폼으로 물체 이동
        }
    }

    // 충돌시 죽는 모션 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.Log("장애물에 애니메이터가 없습니다.");
        }
    }
}
