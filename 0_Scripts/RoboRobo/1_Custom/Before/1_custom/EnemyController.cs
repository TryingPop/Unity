using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Stats
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

		GetComp();
    }

    protected override void GetComp()
    {

		base.GetComp();

		state = GetComponent<EnemyState>();
		idle = GetComponent<StateIdle>();
		atk = GetComponent<StateMeleeAtk>();
		anim = GetComponent<EnemyAnimation>();

		targetStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
    }

    private void Start()
	{

		idle.SetTotalWeight();

		SetHp();

        myWC.Attack += Attack;

        GameManager.instance.huntingMission.ChangeTargetNum();
	}

	void Update()
	{
		if (!deadBool)
		{

			state.ChkState(ref targetTrans);

			FSM(state.GetState());

			Move(status.MoveSpd * Time.deltaTime);

			if (moveDir == Vector3.zero && idle.actionBool && idle.moveBool)
			{

				idle.moveBool = false;
				anim.ChkAnimation(1, false);
			}

			if (state.chkBool)
			{

				idle.actionBool = !idle.actionBool;
				atk.actionBool = !idle.actionBool;

				anim.ChkAnimation(2, atk.actionBool);
				
				if (atk.actionBool)
				{

					StartCoroutine(Attack());
				}
				else
				{

					StopAllCoroutines();
					myWC.AtkColActive(false);
				}

			}
		}
	}

	private void FSM(EnemyState.State state)
	{

		switch (state)
		{

			case EnemyState.State.idle:
				idle.Action(ref moveDir);

				// 움직이는 애니메이션 시작
				if (idle.moveStartBool) 
				{

					idle.moveStartBool = false;
					anim.ChkAnimation(1, true); 
				}
				break;

			case EnemyState.State.attack:
				atk.Action(ref moveDir, targetTrans);

				break;

			default:
				break;
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
		base.Dead();
		StopAllCoroutines();
		targetTrans = null;
		GameManager.instance.ChkWin();
	}
}