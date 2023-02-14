using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AutoAttack : Unit
{

    private static Transform targetTrans;           // 플레이어 transform
    private static PlayerController controller;     // 데미지 줄 플레이어 controller

    private Vector3 dir;                            // 바라볼 방향

    private Animation myAnim;                       // 애니메이션

    public IObjectPool<AutoAttack> poolToReturn;    // 오브젝트 풀
    

    private void Awake()
    {

        // 타겟이 있는지 확인 없다면 추가
        if (targetTrans == null)
        {

            targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // 이후 플레이어 컨트롤러 확인
        if (controller == null)
        {

            controller = targetTrans.GetComponent<PlayerController>();
        }

        // 초기 컴포넌트 얻는다
        // 리지드바디, 소리 컴포넌트, 무기 컨트롤러, 공격 타이머 설정
        GetComp();

        // 애니메이션 컴포넌트 획득
        myAnim = GetComponent<Animation>();

        // 레이어 설정
        myAnim["0_idle"].layer = 0;
        myAnim["1_walk"].layer = 1;
        myAnim["2_attack"].layer = 2;
        myAnim["3_attacked"].layer = 3;

        // 공격 시 이벤트 연결
        myWC.Attack += Attack;
    }

    private void OnEnable()
    {
        
        // 재활성화 이므로 체력 초기화 및 애니메이션 다시 실행 그리고 공격 코루틴 실행
        Reset();
    }

    private void Update()
    {
        
        // 플레이어 방향으로 공격을 하며 이동
        Move();
    }

    /// <summary>
    /// 체력 회복 등 적 부활 시 실행하는 행동들
    /// </summary>
    public void Reset()
    {

        // 체력 초기화
        Init();

        // dmgCol.enabled = true;
        myAnim.CrossFade("0_idle", 0.2f);
        myAnim.CrossFade("1_walk", 0.1f);
        myAnim.CrossFade("2_attack", 0.1f);

        // 공격 간격마다 공격 콜라이더 활성화 시키는 코루틴 개시
        StartCoroutine(Attack());
    }

    /// <summary>
    /// 플레이어를 향해 이동
    /// </summary>
    private void Move()
    {

        // 방향 설정
        // y 값은 현재 y좌표를 유지한다
        dir = targetTrans.position - transform.position;
        dir.y = 0;
        dir = dir.normalized;

        // 방향으로 이동 및 바라보는 방향 설정
        myRd.MovePosition(transform.position + dir * status.MoveSpd * Time.deltaTime);
        transform.LookAt(transform.position + dir);
    }

    /// <summary>
    /// 피격 메소드 자신의 hp 깎는다
    /// </summary>
    /// <param name="atk">공격력</param>
    public override void OnDamaged(int atk)
    {

        // 데미지를 준다
        base.OnDamaged(atk);

        // hp % 수치 설정 및 UI로 표시
        float hp = (float)nowHp / status.Hp;
        StatsUI.instance.SetEnemyHp(hp);

        // 사망 확인
        ChkDead();
    }

    /// <summary>
    /// 설정된 타이머마다 공격 콜라이더 활성화
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {
       
        // 게임 중이고 생존 시만 작동 공격
        while (!deadBool && GameManager.instance.state == GameManager.GAMESTATE.Play)
        {
            
            myWC.AtkColActive(true);

            yield return atkWaitTime;
        }
    }

    /// <summary>
    /// 콜라이더 충돌 시 실행할 메소드
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    protected override void Attack(object sender, Collider other)
    {

        // 대상은 플레이어 뿐이므로 플레이어한해서 데미지를 준다
        controller.OnDamaged(status.Atk);

        base.Attack(sender, other);
    }

    /// <summary>
    /// 사망 시 실행하는 메소드
    /// </summary>
    protected override void Dead()
    {

        // 상태 변경
        base.Dead();

        // 코루틴 중지 GameObject를 disable로 취소되긴 하지만
        // 혹시 몰라서 중지
        StopAllCoroutines();

        // 승리 조건 메소드
        GameManager.instance.ChkWin();

        // pool에서 설정된 release 실행 즉, 비활성화
        poolToReturn.Release(element: this);
    }
}
