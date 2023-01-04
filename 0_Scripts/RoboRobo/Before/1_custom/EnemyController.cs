using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Stats
{
    #region Convertible Variable
    [Header("스텟")]
	[Tooltip("이동 속도")] [SerializeField]
	private float moveSpeed; 

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

    // [Tooltip("난이도 상승 시간")] [SerializeField]
    // private float maxReinforcementTime; // 난이도 상승 시간
    #endregion

    // 현재 미구현
    // private float reinforcementTime; // 난이도 상승에 잴 시간

    private float distance; // 자신과 적과의 거리
	private bool isAtk; // 공격 상태?

    private Transform targetTrans; // 타겟의 transform
	private Animation EnemyAnimation; // 적의 애니메이션
	private AudioSource EnemyAudioSource; // 적의 오디오소스

	private Vector3 dir; // 바라볼 방향

	private List<string> stateList;

	private void Start()
	{
		rd = GetComponent<Rigidbody>(); // 자신의 리지드바디
		EnemyAudioSource = GetComponent<AudioSource>(); // 자신의 오디오 소스
		if (stateList == null)
		{
			stateList = new List<string>() { "0_idle", "1_walk", "2_attack", "3_attacked" };
		}

        EnemyAnimation = GetComponent<Animation>();

		SetHp();

		// 행동을 따로 하게 하기 위해 레이어 구분
		// 빼면 모션 중 하나만 작동한다
        for (int i = 0; i < stateList.Count; i++)
		{
			EnemyAnimation[stateList[i]].layer = i;
		}

		EnemyAnimation.CrossFade(stateList[0], 0.2f);

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
			FindDistance(); // 시야 안에 들어오는지 확인
			Action(); // 시야안에 들어올 경우 행동
		}
	}

	void FindDistance() // 적과의 거리 측정
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

						dir = (targetTrans.position - transform.position);
						dir.y = 0;
						dir = dir.normalized;

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
		if (targetTrans == null) // 타겟 유무 판별
		{
			SelectAnimation(1, false);
			return;
		}

		SelectAnimation(1, true);
		Move(); // 타겟이 있으니 타겟으로 이동!
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
			EnemyAnimation.CrossFade(stateList[i], 0.1f);
		}
		else
		{
			EnemyAnimation.Stop(stateList[i]);
		}
	}

	void Move()
	{
        rd.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
		transform.LookAt(transform.position + dir);
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
		dmgCol.enabled = false;
		
        ThirdPersonController controller = obj.GetComponent<ThirdPersonController>();
		controller.ChangeColor(Color.red);
		controller.Damaged(atk);
        
		yield return waitTime;

		controller.ChangeColor(Color.white);

		dmgCol.enabled = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && isAtk) 
		{

			/*
			// 넉백
            Vector3 vec = (other.transform.position - transform.position).normalized;
            vec.y = 0;
            other.transform.localPosition += vec * 0.2f;
			*/
			StartCoroutine(Attack(other.gameObject, 0.5f)) ;
		}
	}

	public override void Damaged(int _damage)
	{
		if (targetTrans == null && !deadBool)
		{
			_damage *= 2;
		}

		SelectAnimation(3, true);
        
        base.Damaged(_damage);

        StatsUI.instance.SetEnemyHp();

        if (EnemyAudioSource != null && !EnemyAudioSource.isPlaying)
		{

			EnemyAudioSource.Play();
		}

		ChkDead();
	}

	protected override void Dead()
	{
		base.Dead();
		StopAllCoroutines();
		isAtk = false;
		targetTrans = null;
		GameManager.instance.ChkWin();
		
		dmgCol.enabled = false;
	}


}