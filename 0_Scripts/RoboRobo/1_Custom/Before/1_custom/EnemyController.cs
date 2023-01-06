using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Stats
{
    #region Convertible Variable
    [Header("����")]
	[Tooltip("�̵� �ӵ�")] [SerializeField]
	private float moveSpeed; 

	[Tooltip("ã�� �ݰ�")] [SerializeField]
	private float findRadius;

	[Tooltip("ã�� ����")] [SerializeField]
	private float findAngle; 

	[Tooltip("���� ����")] [SerializeField]
	private float attackRange; 

	[Tooltip("������ �ν��� ���̾�")] [SerializeField]
	private LayerMask playerMask;

    [Tooltip("��ֹ��� �ν��� ���̾�")] [SerializeField]
    private LayerMask obstacleMask;

	[Tooltip("�÷��̾� �±�")] [SerializeField] 
	private string playerTag;

    // [Tooltip("���̵� ��� �ð�")] [SerializeField]
    // private float maxReinforcementTime; // ���̵� ��� �ð�
    #endregion

    


    private float distance;         // �ڽŰ� ������ �Ÿ�


    private bool isAtk;				// ���� ����?

    private Transform targetTrans; // Ÿ���� transform
	private Animation myAnim; // ���� �ִϸ��̼�

	private List<string> stateList;

    private void Awake()
    {
		GetComp();
    }

    private void Start()
	{


		if (stateList == null) { stateList = new List<string>() { "0_idle", "1_walk", "2_attack", "3_attacked" }; }

        myAnim = GetComponent<Animation>();

		SetHp();

		// �ൿ�� ���� �ϰ� �ϱ� ���� ���̾� ����
		// ���� ��� �� �ϳ��� �۵��Ѵ�
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
			FindDistance(); // �þ� �ȿ� �������� Ȯ��
			Action(); // �þ߾ȿ� ���� ��� �ൿ
		}
	}

	void FindDistance() // ������ �Ÿ� ����
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
		if (targetTrans == null) // Ÿ�� ���� �Ǻ�
		{
			SelectAnimation(1, false);
			return;
		}

		SelectAnimation(1, true);
		Move(); // Ÿ���� ������ Ÿ������ �̵�!
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

	void Move()
	{
        myRd.MovePosition(transform.position + moveDir * moveSpeed * Time.deltaTime);
		transform.LookAt(transform.position + moveDir);
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
		
		// dmgCol.enabled = false;
	}


}