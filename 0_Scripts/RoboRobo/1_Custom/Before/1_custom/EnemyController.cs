using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Stats
{
    #region Convertible Variable
    [Header("스텟")]
	[Tooltip("찾을 반경")] [SerializeField]
	private float findRadius;

	[Tooltip("찾을 각도")] [SerializeField]
	private float findAngle; 

	[Tooltip("공격 범위")] [SerializeField]
	private float attackRange; 

	[Tooltip("적으로 인식할 레이어")] [SerializeField]
	private LayerMask playerMask;

    [Tooltip("장애물로 인식할 레이어")] [SerializeField]
    private LayerMask obstacleMask;

	[Tooltip("플레이어 태그")] [SerializeField] 
	private string playerTag;

	[SerializeField] private EnemyState state;
	[SerializeField] private StateIdle idle;

    #endregion

    private float distance;         // 자신과 적과의 거리


    private bool isAtk;				// 공격 상태?

    private Transform targetTrans; // 타겟의 transform
	private Animation myAnim; // 적의 애니메이션

	private List<string> stateList;

    private void Awake()
    {

		GetComp();
    }

    protected override void GetComp()
    {

		base.GetComp();

        myAnim = GetComponent<Animation>();
		state = GetComponent<EnemyState>();
		idle = GetComponent<StateIdle>();
    }

    private void Start()
	{

		if (stateList == null) { stateList = new List<string>() { "0_idle", "1_walk", "2_attack", "3_attacked" }; }

		idle.SetTotalWeight();
		SetHp();

		// 행동을 따로 하게 하기 위해 레이어 구분
		// 빼면 모션 중 하나만 작동한다
        for (int i = 0; i < stateList.Count; i++)
		{
			myAnim[stateList[i]].layer = i;
		}

		myAnim.CrossFade(stateList[0], 0.2f);

		if (findRadius < attackRange)
		{
			findRadius = attackRange;
		}

		// GameManager.instance.ChkEnemyCount();
		GameManager.instance.huntingMission.ChangeTargetNum();
	}

	void Update()
	{
		if (!deadBool)
		{

			state.ChkState(targetTrans);

			FSM(state.GetState());

			// GetDistance(); // 시야 안에 들어오는지 확인
			// Action(); // 시야안에 들어올 경우 행동
		}
	}

	private void FSM(EnemyState.State state)
	{

		switch (state)
		{

			case EnemyState.State.idle:
				idle.Action(myRd, status.MoveSpd);
				break;

			case EnemyState.State.attack:

				break;

			default:
				break;
		}
	}


	void GetDistance() 
		// 적과의 거리 측정
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, findRadius,
												// LayerMask.GetMask("Player")|LayerMask.GetMask("Allies"));
												playerMask);

		if (cols.Length > 0)
		{
			if (Vector3.Angle(cols[0].gameObject.transform.position - transform.position, 
							transform.forward) < findAngle * 0.5f) 
			{
				RaycastHit hit;

				if (Physics.Raycast(transform.position, cols[0].gameObject.transform.position - transform.position, out hit, findRadius, playerMask | obstacleMask))
				{

					if (hit.transform.tag == playerTag)
					{

						distance = (cols[0].gameObject.transform.position - transform.position).magnitude;
						targetTrans = cols[0].gameObject.transform;

						moveDir = (targetTrans.position - transform.position);
						moveDir.y = 0;
						moveDir = moveDir.normalized;

						return;
					}
				}
			}
		}

		if (distance < findRadius)
		{

			distance = findRadius + 1;
		}

		targetTrans = null;
		return;
	}


	void Action()
    {
		if (targetTrans == null)				// 타겟 유무 판별
		{
			SelectAnimation(1, false);
			return;
		}

		SelectAnimation(1, true);
		Move(status.MoveSpd * Time.deltaTime);	// 타겟이 있으니 타겟으로 이동!
		AttackAnimation();
    }


	void SelectAnimation(int i, bool start = true)
	{
		if (i >= stateList.Count)
		{

			return;
		}

		if (start) 
		{

			myAnim.CrossFade(stateList[i], 0.1f);
		}
		else
		{

			myAnim.Stop(stateList[i]);
		}
	}

	void AttackAnimation()
	{
		if (distance < attackRange)
		{

			SelectAnimation(2, true);
			isAtk = true;
			
		}
		else
		{

			SelectAnimation(2, false);
			isAtk = false;
		}
	}

	IEnumerator Attack(GameObject obj, float time)
	{
		WaitForSeconds waitTime = new WaitForSeconds(time);
		// dmgCol.enabled = false;
		
        var controller = obj.GetComponent<PlayerController>();
		// controller.ChangeColor(Color.red);
		// controller.Damaged(status.Atk);
        
		yield return waitTime;

		// controller.ChangeColor(Color.white);

		// dmgCol.enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && isAtk) 
		{

			StartCoroutine(Attack(other.gameObject, 0.5f)) ;
		}
	}

	public override void OnDamaged(int _damage)
	{
		if (targetTrans == null && !deadBool)
		{
			_damage *= 2;
		}

		SelectAnimation(3, true);
        
        base.OnDamaged(_damage);

        StatsUI.instance.SetEnemyHp(this);

		ChkDead();
	}

	protected override void Dead()
	{
		base.Dead();
		StopAllCoroutines();
		isAtk = false;
		targetTrans = null;
		GameManager.instance.ChkWin();
		
	}


}