using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Unit
{
    #region Convertible Variable
    [Header("����")]
	[SerializeField] private EnemyState state;
	[SerializeField] private StateIdle idle;
	[SerializeField] private StateMeleeAtk atk;
	[SerializeField] private EnemyAnimation anim;

    #endregion


    public Transform targetTrans; // Ÿ���� transform

    private void Awake()
    {

		// ������Ʈ �����´�
		GetComp();
    }

	/// <summary>
	/// ������Ʈ ��������
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

		// ��� ���� ��Ż weigtht ����
		idle.SetTotalWeight();

		// ü�� ����
		SetHp();

		// weapon�� ���� ���
        myWC.Attack += Attack;
	}

	void Update()
	{
		if (!deadBool)
		{

			// ���� üũ
			state.ChkState(ref targetTrans);

			// ���¿� �´� �ൿ 
			FSM(state.GetState());

			// �̵�
			Move(status.MoveSpd * Time.deltaTime);

			// �ִϸ��̼� üũ
			ChkAnimation();

			// ���� ��ȯ�� �ִ� ���
			if (state.chkBool)
			{
				
				// ���� �����ؾ� �ϴ��� �Ǻ�
				if (atk.activeBool)
				{

					// ���� ���� 1�ʴ� 1�� ����
					StartCoroutine(Attack());
				}
				// ���� ����
				else
				{

					// ���� �ڷ�ƾ ����
					StopAllCoroutines();
					myWC.AtkColActive(false);
				}

			}
		}
	}

	/// <summary>
	/// ���¿� ���� �ൿ Ȯ��
	/// </summary>
	/// <param name="state"></param>
	private void FSM(EnemyState.State state)
	{

		// ���� Ȯ��
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