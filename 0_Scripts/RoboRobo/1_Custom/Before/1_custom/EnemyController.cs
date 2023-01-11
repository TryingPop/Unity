using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Stats
{
    #region Convertible Variable
    [Header("Ω∫≈›")]
	[SerializeField] private EnemyState state;
	[SerializeField] private StateIdle idle;
	[SerializeField] private StateMeleeAtk atk;
	[SerializeField] private EnemyAnimation anim;

    #endregion


    public Transform targetTrans; // ≈∏∞Ÿ¿« transform

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
	}

	void Update()
	{
		if (!deadBool)
		{

			state.ChkState(ref targetTrans);

			FSM(state.GetState());

			Move(status.MoveSpd * Time.deltaTime);

			ChkAnimation();

			if (state.chkBool)
			{
				
				if (atk.activeBool)
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

		if (idle.moveBool || (!idle.moveBool && !idle.activeBool))
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