using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Unit
{
    #region Convertible Variable
    [Header("스텟")]
	[SerializeField] private EnemyState state;
	[SerializeField] private StateIdle idle;
	[SerializeField] private StateMeleeAtk atk;
	[SerializeField] private EnemyAnimation anim;

    #endregion


    public Transform targetTrans; // 타겟의 transform

    private void Awake()
    {

		// 컴포넌트 가져온다
		GetComp();
    }

	/// <summary>
	/// 컴포넌트 가져오기
	/// </summary>
    protected override void GetComp()
    {

		base.GetComp();

		if (state == null) state = GetComponent<EnemyState>();
		if (idle == null) idle = GetComponent<StateIdle>();
		if (atk == null) atk = GetComponent<StateMeleeAtk>();
		if (anim == null) anim = GetComponent<EnemyAnimation>();
		if (targetStats == null) targetStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Unit>();
    }

    private void Start()
	{

		// 대기 상태 토탈 weigtht 설정
		idle.SetTotalWeight();

		// 체력 설정
		SetHp();

		// weapon에 공격 담기
        myWC.Attack += Attack;
	}

	void Update()
	{
		if (!deadBool)
		{

			// 상태 체크
			state.ChkState(ref targetTrans);

			// 상태에 맞는 행동 
			FSM(state.GetState());

			// 이동
			Move(status.MoveSpd * Time.deltaTime);

			// 애니메이션 체크
			ChkAnimation();

			// 상태 변환이 있는 경우
			if (state.chkBool)
			{
				
				// 공격 실행해야 하는지 판별
				if (atk.activeBool)
				{

					// 공격 실행 1초당 1번 공격
					StartCoroutine(Attack());
				}
				// 공격 중지
				else
				{

					// 공격 코루틴 중지
					StopAllCoroutines();
					myWC.AtkColActive(false);
				}

			}
		}
	}

	/// <summary>
	/// 상태에 따른 행동 확인
	/// </summary>
	/// <param name="state"></param>
	private void FSM(EnemyState.State state)
	{

		// 상태 확인
		SetStateActiveBool(state);

		switch (state)
		{

			case EnemyState.State.idle:
				idle.Action(ref moveDir);
				break;

			case EnemyState.State.attack:
				atk.Action(ref moveDir, targetTrans);
                break;

			default:
				break;
		}
	}

	private void SetStateActiveBool(EnemyState.State state)
	{

		idle.SetActiveBool(state);
		atk.SetActiveBool(state);
	}

	private void ChkAnimation()
	{

		if (idle.ChkState(StateIdle.State.Wander) || (!idle.ChkState(StateIdle.State.Wander) && atk.activeBool))
		{

			anim.ChkAnimation(1, true);
		}

		if (moveDir == Vector3.zero || deadBool)
		{

			anim.ChkAnimation(1, false);
		}

		if (state.chkBool)
		{

			anim.ChkAnimation(2, atk.activeBool);
		}
		
		if (deadBool)
		{

			anim.ChkAnimation(1, false);
			anim.ChkAnimation(2, false);
		}
	}

    protected override IEnumerator Attack()
    {

		while (true)
		{

			myWC.AtkColActive(true);
			
			yield return atkWaitTime;
		}
    }

    protected override void Attack(object sender, Collider other)
	{

		targetStats.OnDamaged(status.Atk);
		base.Attack(sender, other);
	}

	public override void OnDamaged(int _damage)
	{
		if (targetTrans == null && !deadBool)
		{
			_damage *= 2;
		}

		anim.ChkAnimation(3, true);
        
        base.OnDamaged(_damage);

        StatsUI.instance.SetEnemyHp(this);

		ChkDead();
	}

	protected override void Dead()
	{

		myWC.Attack -= Attack;
		myWC.AtkColActive(false);
		base.Dead();
		ChkAnimation();
		StopAllCoroutines();
		targetTrans = null;
		GameManager.instance.ChkWin();
	}
}