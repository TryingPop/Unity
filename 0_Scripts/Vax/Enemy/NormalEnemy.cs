// #define InUnity

using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static BossEnemy;

public class NormalEnemy : MonoBehaviourPunCallbacks, EnemyDamaged
{
    /// <summary>
    /// Idle, Tracking, Attack, Damaged, Dead
    /// </summary>
    public enum NormalState 
    {
        Idle,        // ���
        Tracking,    // ����
        Attack,      // ����
        Damaged,     // �ǰ�
        Dead         // ���
    }

    #region Variable

    //private PhotonView PV;

    // ü��, ���ݷ�, ����, �̼�, �ൿ ����, ���� ��ȯ�� ���� �Ÿ� �� �ݰ�, ���� �Ÿ� �� �ݰ�
    [SerializeField] protected NormalEnemyStats stats;
    [SerializeField] protected NormalEnemyAnimator animator;    // �Ϲ� ���� �ִϸ�����
    [SerializeField] protected NormalState state;               // ���� ����(Idle, tracking, Attack, Damaged, Dead )
    [SerializeField] protected CharacterController cc;          // ĳ���� ��Ʈ�ѷ�
    [SerializeField] protected NavMeshAgent agent;              // ��ã�� AI
    [SerializeField] protected Transform targetTrans;           // Ÿ�� ��ġ
    [SerializeField] protected string playerTag;                // �� �±�
    [SerializeField] protected LayerMask targetMask;            // �� ���̾� 
    [SerializeField] protected LayerMask obstacleMask;          // �ǹ� ���̾�

    protected int currentHp;                                    // ���� ü��
    protected bool isDelay;                                     // ����, �ǰ� ��� �ð�

    protected Vector3 lookDir;                                  // ����� ����
    protected WaitForSeconds waitTime;                          // �Ź� ���� X

#if InUnity
    // Ȯ�ο� ����
    float lookingAngle;
    Vector3 leftDir;
    Vector3 rightDir;
    Vector3 lookDir;
    Vector3 leftatkDir;
    Vector3 rightatkDir;
#endif

    #endregion Variable

    #region Unity Method

    /// <summary>
    /// ������Ʈ �ʱ�ȭ
    /// </summary>
    protected void Awake()
    {
        //PV = GetComponent<PhotonView>();

        // ĳ���� ��Ʈ�ѷ� �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �̵� ai �޾ƿ���
        agent = GetComponent<NavMeshAgent>();

        // �ִϸ����� ������ �޾ƿ���
        if (animator == null) 
        { 
            animator = GetComponent<NormalEnemyAnimator>();
        }

        // �ൿ Ÿ�� 0 ���ϸ� 0.2�� �����ع�����
        if (stats.actionTime <= 0) 
        {
            stats.actionTime = 0.2f;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        state = NormalState.Idle;
        currentHp = stats.maxHp;
        agent.speed = stats.moveSpd;

        // �ൿ ���� ����
        waitTime = new WaitForSeconds(stats.actionTime); 

        // ���� �� �ൿ üũ ����
        StartCoroutine(Action());
    }


    #endregion Unity Method

    #region �浹 ����
    /// <summary>
    /// ���� ��ä�� ������ �浹�ϴ� ��ü ã��
    /// </summary>
    /// <param name="Radius">�Ÿ�</param>
    /// <param name="Angle">����</param>
    /// <returns>������ true, ������ false</returns>
    private bool FindTarget(float Radius, float Angle) 
    {

        // ���� targetMask�� �������� ã�´�
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, Radius, targetMask);

        // ��� 1�� �̻� ã�� ���
        if (cols.Length > 0) 
        {

            // �Ѱ��� �˻�
            foreach (var item in cols) 
            {

                // ���� ��ü�� ���� ��ü���� Ȯ�� �� true�� ��� �н��Ѵ�.
                if (item.gameObject == gameObject) 
                {
                    continue; 
                }

                // �ʹ� �����
                lookDir = item.gameObject.transform.position - transform.position; 

                // ���� �ȿ� �ִ��� Ȯ��
                if (Vector3.Angle(lookDir, transform.forward) < 0.5f * Angle) 
                {

                    // �߰��� ��ֹ� �ִ��� Ȯ��
                    RaycastHit hit;
                    Physics.Raycast(cc.center + transform.position, lookDir, out hit, Radius, targetMask 
                        | obstacleMask
                        );

                    // �÷��̾�� �浹���� �� ����
                    if (hit.transform?.tag == playerTag)
                    {
                        targetTrans = item.gameObject.transform;

                        // �ٶ� �������� ����
                        lookDir.y = 0;
                        lookDir = lookDir.normalized;

                        // �����ϴ� �ְ� �־� true�� Ż��
                        return true; 
                    }
                }
            }
        }

        // �����ϴ� �ְ� ���� ��� ����� ���⿡ Ÿ�� ��ǥ X �� ��ȯ�� false
        targetTrans = null;
        return false; 
    }
    #endregion  �浹 ����

    #region FSM
    /// <summary>
    /// �ൿ �޼ҵ�
    /// </summary>
    /// <returns>�տ��� ������ �ð���ŭ ���!</returns>
    private IEnumerator Action()
    {
        while (true)
        {

            // ���� Ȯ��
            ChkAction();

            yield return waitTime;
        }
    }

    /// <summary>
    /// FSM �޼ҵ�
    /// ���� ���¿� ���� �ൿ �޼ҵ� ����
    /// </summary>
    private void ChkAction()
    {

        // ���� ���¸� ����
        switch (state)
        {

            // ��� ���� �ȿ� �� �� ���� ������ �ִ´�
            case NormalState.Idle: 
                StateIdle();
                break;

            // ����� �Ѿư��� ���� ���� ����
            case NormalState.Tracking: 
                                       
                StateTracking();       
                break;

            // ���� ��� �� ���� ���� ����
            case NormalState.Attack: 
                StateAttack();
                break;

            // �ǰ� �� ��� ���¿����� �ƹ��͵� ���Ѵ�
            case NormalState.Damaged: 
            case NormalState.Dead: 
                break;
        }
    }

    /// <summary>
    /// ��� ���¿��� �ൿ �ϴ� �޼ҵ�
    /// </summary>
    private void StateIdle() 
    {

        // ��� ���� ����
        if (FindTarget(stats.trackingRadius, stats.trackingAngle)) 
        {

            // ���� ���� ���� �� �̵� ���� ����
            state = NormalState.Tracking; 
            animator.IdleToMove(); 
        }
    }

    /// <summary>
    /// ���� ���¿��� �ൿ �ϴ� �޼ҵ�
    /// </summary>
    private void StateTracking() 
    {

        // �ǰ� �̳� ���� ���� Ż�� �� ����Ǵ� ����
        // 1�ϰ� ���
        if (isDelay) 
        {

            isDelay = false;
        }
        // �̿� ���
        else
        {

            // agent �ϴ� Ű�� ����
            if (!agent.enabled) 
            {

                agent.enabled = true;
            }

            // ����� �ִ� ��� �̵�
            if (targetTrans != null) 
            {

                agent.destination = targetTrans.position;
            }

            // ���� ���·� ���ؾ� �ϴ��� üũ
            if (FindTarget(stats.atkRadius, stats.atkAngle)) 
            {

                state = NormalState.Attack; 
                animator.MoveToAttack(); 

                return;
            }


            // ��� ���·� ���ؾ� �ϴ��� üũ
            if (!FindTarget(stats.trackingRadius, stats.trackingAngle)) 
            {

                state = NormalState.Idle; 
                agent.enabled = false; 
                animator.MoveToIdle(); 

                return;
            }
        }
    }

    /// <summary>
    /// ���� ���¿��� �ൿ �ϴ� �޼ҵ�
    /// </summary>
    private void StateAttack()
    {

        // agent�� ����
        if (agent.enabled)
        {
            agent.enabled = false;
        }

        // ���� ���� ������ üũ
        if (!FindTarget(stats.atkRadius, stats.atkAngle))
        {

            // ����� ��� �� �ش�
            isDelay = true; 
            state = NormalState.Tracking;
            animator.AttackToMove();

            return;
        }

        // ���� ���¿����� ����� �ٶ󺸰� �Ѵ�
        transform.LookAt(transform.position + lookDir); 
    }

    #endregion

    #region �ִϸ����� �̺�Ʈ �޼ҵ�

    /// <summary>
    /// 1�� Ÿ�ٰ����� ���� ����
    /// </summary>
    public void AttackRay() 
    {
        // ������ ���� �ִ��� �Ǻ�
        RaycastHit hit;
        if (Physics.Raycast(transform.position + cc.center, lookDir, out hit, stats.atkRadius, targetMask)) 
        {
            // Ÿ�ٿ� �ǰ� �Լ� ������ �ȴ�
            hit.transform.gameObject.GetComponent<StatusController>()?.DecreaseHP(stats.atk); 
        }
    }

    /// <summary>
    /// ��ä�� ���� ���� �޼ҵ�
    /// </summary>
    public void AttackSector()
    {

        // targetMask�� ������ ���� ���� �ݰ�ȿ� ���Դ��� Ȯ��
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, stats.atkRadius, targetMask);


        // ���� �ְ� �ִ��� Ȯ��
        if (cols.Length > 0) 
        {
            foreach (var item in cols)
            {

                // �ڱ��ڽ��̳� ĸ�� �ݶ��̴��� �ִ��� üũ
                if (item as CapsuleCollider == null || item.gameObject == gameObject)
                {                                                                    
                    continue;
                }

                // ���� �ݰ� �ȿ� �ִ� ���
                if (Vector3.Angle(item.transform.position - transform.position, transform.forward) < stats.atkAngle * 0.5f) 
                {
                    // ���� �� ���̿� ��ֹ��� �ִ��� üũ
                    // ������ ������ ����!
                    // ���� ��� �÷��̾�� �� ���̿� ���� �ִµ� ������ ���� ��� ���� 
                    RaycastHit hit;

                    // ���� ��ġ���� �÷��̾� �������� �������� ���
                    if (Physics.Raycast(transform.position + cc.center, item.transform.position - transform.position, out hit, stats.atkRadius, targetMask | obstacleMask)) 
                    {

                        // hit�� �±׸� ��
                        if (hit.transform.tag == playerTag) 
                        {

                            // �������� �ִ� ����
                            item.gameObject.GetComponent<StatusController>()?.DecreaseHP(stats.atk); 
                        }
                    }
                }
            }
        }
    }



    /// <summary>
    /// �ǰ� ���� Ż�⿡ �� �޼ҵ�
    /// ���� ���·� ����
    /// </summary>
    public void EscapeDamaged()
    {

        state = NormalState.Tracking;
    }

    /// <summary>
    /// ��� ���� Ż�� �޼ҵ�
    /// ����� ��Ȱ��ȭ�� ������ - �ı��Ϸ��� Destroy!
    /// </summary>
    public void EscapeDie()
    {

        gameObject.SetActive(false);
    }
    #endregion

    #region �ܺο� ��ȣ�ۿ��� �޼ҵ�
    /// <summary>
    /// ������ ��� �� �ǰ� or ��� ����
    /// </summary>
    /// <param name="attack">���� ����� ���ݷ�</param>
    public void OnDamagedProcess(int attack = 0) 
    {

        // ü�� ����
        currentHp -= CalcDmg(attack);

        // hp�� �������� ������� �ǰ����� �Ǻ�
        if (currentHp > 0) 
        {
            ChkDamaged(); 
        }
        else 
        {
            ChkDie(); 
        }

        return;
    }

    /// <summary>
    /// ������ ��� �� �ּ� ������ 1 ����
    /// ���ݷ� - ���� = ������  
    /// </summary>
    /// <param name="attack">���� ���ݷ�</param>
    /// <returns>1 �̻��� ������</returns>
    private int CalcDmg(int attack)
    {

        int dmg = attack - stats.def; 
        if (dmg < 1) 
        {
            dmg = 1;
        }

        return dmg;  
    }

    /// <summary>
    /// �ǰ� �� ���ϴ� �޼ҵ�
    /// </summary>
    private void ChkDamaged() 
    {
        // �ǰ� ��� ���� ����
        // ���� ���°� �ƴϰ� �ǰ� ���°� �ƴϰ� �� Ż���� ���°� �ƴ� �� ����
        if (!isDelay
            && state != NormalState.Damaged 
            && state != NormalState.Attack) 
        {

            isDelay = true; 
            state = NormalState.Damaged; 
            animator.SetDamaged(); 
        }
    }

    /// <summary>
    /// ��� �� ���ϴ� �޼ҵ�
    /// </summary>
    private void ChkDie()
    {

        // ������ ������
        currentHp = 0;

        // ��� ��� 1ȸ��!
        if (state != NormalState.Dead)
        {
            state = NormalState.Dead; 
            animator.SetDead();
        }
    }

    /// <summary>
    /// ���� ���� �� ������ �޼ҵ�
    /// Ȥ�� ���� �ϴ� ����� ����
    /// </summary>
    private void GameOver() 
    {

        // ��� �ൿ ���� 
        StopAllCoroutines(); 
    }
    #endregion
}


