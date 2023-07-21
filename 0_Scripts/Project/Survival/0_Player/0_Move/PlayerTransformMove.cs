using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformMove : MonoBehaviour
{

    [SerializeField] private float walkSpeed;       // 걷는 속도
    [SerializeField] private float runSpeed;        // 달리기 속도
    private float applySpeed;                       // 실제 적용 속도

    private float hAxis;                            // h 축 입력 감지
    private float vAxis;                            // v 축 입력 감지

    private Vector3 moveDir;                        // 이동 방향

    private Animator playerAnimator;                // 플레이어 애니메이터
    private float moveAnim;                         // 걷는 애니메이션 속도

    [SerializeField] private bool runAnim;          // 달리기 속도 or 걷는 속도 적용 변수

    private void Awake()
    {

        playerAnimator = GetComponentInChildren<Animator>();
    }

    
    private void FixedUpdate()
    {

        // InputManager에 있는 키 세팅에 등록된 문자이다
        // 키보드 방향키나 WASD를 누르면 작동한다
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        // 정규화해서 크기 고정
        moveDir = new Vector3(hAxis, 0, vAxis).normalized;

        // 이동
        applySpeed = runAnim ? runSpeed : walkSpeed;

        transform.position += moveDir * applySpeed * Time.deltaTime;

        // 여기를 애니메이션에서 처리하고 싶다
        // 현재 알아보는 중이다
        if (moveDir.magnitude > 0)
        {

            moveAnim += Time.deltaTime;     
        }
        else
        {

            moveAnim -= 3 * Time.deltaTime;
        }

        moveAnim = Mathf.Clamp(moveAnim, 0, 2f);
        playerAnimator.SetFloat("Move", moveAnim);

        // 이동하는 방향으로 바라보기
        transform.LookAt(transform.position + moveDir);
    }

    /// <summary>
    /// 애니메이션용 이벤트, 0 : 걷는 속도, 1 : 달리는 속도
    /// </summary>
    /// <param name="num"></param>
    public void OnAnimationChkRun(int num)
    {

        runAnim = num == 0 ? false : true;
    }
}
