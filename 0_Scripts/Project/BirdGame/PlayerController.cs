using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _jumpPower = 10f; // 점프 파워

    private Rigidbody2D rigidbody2d; // 플레이어 강체
    private Animator animator; // 플레이어 애니메이터
    private AudioSource audioSource; // 사망시 울음소리
    void Start()
    {
        // 시작하자마자 자신의 컴포넌트 가져오기
        rigidbody2d = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!GameManager.instance.isGameover) // 게임오버가 아니면
        {
            // 점프
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 누르면
            {
                if (rigidbody2d != null) // 강체가 있는 경우
                {
                    rigidbody2d.velocity = Vector2.up * _jumpPower; // _jumpPower만큼 위로 속도
                }
                else // 강체가 없으면
                {
                    Debug.Log("플레이어에 강체가 없습니다."); // 경고 문구
                }
            }

            // 낮은 점프 기능 구현
            // 마우스에서 손을 떼면 속도 절반
            if (Input.GetMouseButtonUp(0) && rigidbody2d.velocity.y > 0) // 마우스 왼쪽 버튼에서 손을 떼고 y축 속도가 양수일 때
            {
                if (rigidbody2d != null) // 강체가 있으면 
                {
                    rigidbody2d.velocity *= 0.5f; // 속도 절반
                }
                else // 강체가 없으면
                {
                    Debug.Log("플레이어에 강체가 없습니다."); // 경고 문구
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // 충돌 하면
    {
        Die(); // 사망 메서드
    }

    // 죽으면 죽는 모션
    private void Die() // 사망 메서드
    {
        if (!GameManager.instance.isGameover) // 죽는 경우 1번만 나오게 하기위해 넣은 구문
                                              // 까마귀와 부딪혔을 때 한 번, 그리고 방지용 벽이랑 부딪혔을 때 한 번
                                              // 그래서 최소 2번 Die메서드 실행하고 독수리 소리 2번 나오게 된다.
        {
            if (audioSource != null) // 오디오 소스가 있으면
            {
                audioSource.Play(); // 소리 재생
            }
            else
            {
                Debug.Log("플레이어에 오디오 소스가 없습니다.");
            }

            if (animator != null) // 애니메이터가 있으면
            {
                animator.SetTrigger("Die"); // 사망 모션
            }
            else
            {
                Debug.Log("플레이어의 애니메이터가 없습니다.");
            }

            GameManager.instance.Gameover(); // 게임오버로 기둥의 생성 막기
        }
    }
}
