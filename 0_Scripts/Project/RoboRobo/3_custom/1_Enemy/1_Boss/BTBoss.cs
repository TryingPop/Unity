using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : Stat
{

    public enum Phase { first, second }                     // ���� ������

    public static Transform playerTrans;                    // �÷��̾� ��ġ

    public Transform missileTransform;                      // �̻��� ���� ��ġ

    public EnemyAnimation anim;                             // �� �ִϸ��̼�

    public GameObject[] missiles;                           // ���Ÿ� ���� ����ü��
    public GameObject[] summoners;                          // ��ȯ����

    [SerializeField] private GameObject atkZone;            // ���� �� �����ִ� ������Ʈ
    [SerializeField] private GameObject damagedText;        // ������ ��ġ UI

    [SerializeField] private AudioClip damagedSnd;          // �ǰ� ����
    [SerializeField] protected AudioScript myAS;            // �Ҹ� ��Ʈ�ѷ�        

    public NavMeshAgent agent;                              
    public Transform targetTrans;                           // Ÿ�� ��ġ

    public LayerMask targetLayer;                           // ã�� Layer
    public LayerMask obstacleLayer;                         // ������ �ν��� ���̾�

    public string targetTag;

    public Phase phase;         // ���� ������

    public bool nowIdleBool;    // ���� ��� ���� Ȯ��
    private bool beforIdleBool; // ������ ��� ���� Ȯ��

    public bool damagedBool;    // �ǰ� ���� Ȯ��

    public int NowHp {          // ü�� ������Ƽ
        get 
        { 

            return nowHp; 
        } 
        set 
        { 

            nowHp = value;
            if (nowHp < 0) { nowHp = 0; }
            else if (nowHp > status.Hp) { nowHp = status.Hp; }
        } 
    }

    [SerializeField] private int phaseHp = 60;      // ������ ���� hp

    [SerializeField] private RangedStatus chase;    // ���� ������ ���� ���� ��ũ���ͺ� ������Ʈ
    [SerializeField] private RangedStatus melee;    // ���� ���� ������ ���� ���� ��ũ���ͺ� ������Ʈ
    [SerializeField] private RangedStatus ranged;   // ��Ÿ� ���� ������ ���� ���� ��ũ���ͺ� ������Ʈ

    public int bulletNum = 6;       // ź�� ��

    private Node topNode;           // ������ ���

    private void Awake()
    {

        // �ʿ��� ������Ʈ ��������� ��������
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player")?.transform;
        if (anim == null) anim = GetComponent<EnemyAnimation>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        GetComp();
        
        // ���⿡ �浹 �� ������ �Լ� ����
        myWC.Attack += Attack;

        // ü�� �� �ʱ�ȭ
        Init();

        // �ൿ ����
        ConstructBehaviorTree();
    }

    private void Start()
    {
        
        // �ڷ�ƾ���� �ൿ ����
        StartCoroutine(Action());
    }
    
    /// <summary>
    /// ��� ����
    /// �ڼ��� ���� pdf �̹��� ����
    /// </summary>
    private void ConstructBehaviorTree()
    {

        IdleNode idleNode = new IdleNode(this);

        FindNode meleeFind = new FindNode(this, this.melee.RangeRadius, this.melee.RangeAngle);
        MeleeAtkNode meleeAtkNode = new MeleeAtkNode(this, status.Atk);

        FindNode chaseFind = new FindNode(this, this.chase.RangeRadius, this.chase.RangeAngle);
        ChaseNode chaseTarget = new ChaseNode(this);

        HealthNode chkPhase = new HealthNode(this, Phase.second);
        
        FindNode rangeFind = new FindNode(this, ranged.RangeRadius, ranged.RangeAngle);
        RangeAtkNode rangeAtkNode = new RangeAtkNode(this, status.Atk);

        Sequence melee = new Sequence(new List<Node> { meleeFind, meleeAtkNode });
        Sequence range = new Sequence(new List<Node> { rangeFind, rangeAtkNode });
        Sequence chase = new Sequence(new List<Node> { chaseFind, chaseTarget });

        Selector action1 = new Selector(new List<Node> { melee, chase, idleNode });
        Selector action2 = new Selector(new List<Node> { melee, range, new ChaseNode(this) });

        Sequence phase2 = new Sequence(new List<Node> { chkPhase, action2 });

        topNode = new Selector(new List<Node> { phase2, action1 });
    }

    /// <summary>
    /// �ൿ ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator Action()
    {

        while (!deadBool)
        {

            // ��� ���� Cnt ���ǿ�
            ResetIdleBool();

            // ��� üũ ����
            topNode.Evaluate();
            yield return new WaitForSeconds(0.3f);

            // �ǰ� ���� Ż��
            damagedBool = false;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {

        atkBool = true;

        // ���� ���� �� 1�ʰ� ���� ���� �����ֱ�
        atkZone.SetActive(true);
        yield return atkWaitTime;
        yield return atkWaitTime;

        // 1�ʵ��� ������ �ݶ��̴� Ȱ��ȭ
        myWC.AtkColActive(true);
        
        yield return atkWaitTime;
        yield return atkWaitTime;

        // ���� ���� ���� ���� ��Ȱ��ȭ
        atkZone.SetActive(false);
        myWC.AtkColActive(false);
        yield return atkWaitTime;

        atkBool = false;
    }


    /// <summary>
    /// idle üũ
    /// </summary>
    private void ResetIdleBool()
    {

        beforIdleBool = nowIdleBool;
        nowIdleBool = false;
    }



    /// <summary>
    /// idle�� ù �������� Ȯ��
    /// </summary>
    /// <returns>ù ���� ����</returns>
    public bool ChkIdle()
    {

        if (!beforIdleBool && nowIdleBool)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// �ȱ� �ִϸ��̼ǰ� �̵�
    /// </summary>
    /// <param name="start"></param>
    public void WalkAnim(bool start)
    {

        agent.enabled = start;
        anim?.ChkAnimation(1, start);
    }

    /// <summary>
    /// ���� �ִϸ��̼�
    /// </summary>
    /// <param name="start"></param>
    public void AtkAnim(bool start)
    {

        anim?.ChkAnimation(2, start);
    }

    /// <summary>
    /// �ǰ� �� ������ �޼ҵ�
    /// </summary>
    /// <param name="atk">���� ���ݷ�</param>
    public override void OnDamaged(int atk)
    {

        damagedBool = true;

        // �ǰ� �Ҹ�
        myAS.SetSnd(damagedSnd);
        myAS.GetSnd(false);

        // hp ��°�
        base.OnDamaged(atk);

        // ü�� UI ǥ��
        float hp = (float)nowHp / status.Hp;
        StatsUI.instance.SetEnemyHp(hp);

        // ������ üũ
        if (nowHp < phaseHp)
        {

            phase = Phase.second;
        }
        else
        {

            phase = Phase.first;
        }

        // ������ ��ġ ����
        if (damagedText != null)
        {

            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        // ��� üũ
        ChkDead();
    }

    /// <summary>
    /// ���
    /// </summary>
    protected override void Dead()
    {
        // ���� ����
        base.Dead();
            
        // �¸�
        GameManager.instance.ChkWin();
        
        // ��� �ڷ�ƾ ����
        StopAllCoroutines();
    }

    /// <summary>
    /// ���� Ȱ��ȭ �� ���
    /// </summary>
    public void ActiveWeapon()
    {

        // ���� ���°� �ƴ� ���
        if (!atkBool) 
        {
           
            WalkAnim(false);
            StartCoroutine(Attack());
            AtkAnim(true);
        }
    }

    /// <summary>
    /// ���� �ݶ��̴� �浹 �� Ȱ��ȭ�� �޼ҵ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    protected override void Attack(object sender, Collider other)
    {

        // ������ �ֱ�
        other.GetComponent<Stat>().OnDamaged(status.Atk);
        
        // ���� ���� ����
        atkZone.SetActive(false);
        base.Attack(sender, other);
    }

    /// <summary>
    /// ü�� ȸ���ؾ� ���� üũ
    /// </summary>
    /// <returns></returns>
    public bool ChkHeal()
    {

        if (NowHp < status.Hp)
        {

            return true;
        }

        return false;
    }
}

