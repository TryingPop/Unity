// #define ExampleUnity

using Photon.Pun;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviourPun, EnemyDamaged
{
    /// <summary>
    /// ���� ���� ����
    /// ���¿� ���� �ൿ �޼ҵ� ����
    /// </summary>
    public enum EnemyState { Idle,                        // ��� ����
                            Move,                         // �̵� ����
                            Attack,                       // ���� ����
                            Damaged,                      // ���� ����
                            Dead                          // ��� ����
                            }           

    /// <summary>
    /// 1������ ���� ���� 2��
    /// 2������ ���� ���� 4�� ������ ��
    /// </summary>
    public enum EnemyPhase 
    {
        first,                                            // 1������
        second                                            // 2������
    }

    [Header("�� ����")]
    [SerializeField] private EnemyState enemyState;       // ���� ���¸� ǥ���� ����
    [SerializeField] private EnemyPhase enemyPhase;       // ���� ����� ǥ���� ����
    [SerializeField] public int maxHp;                    // �ִ� ü��
    [SerializeField] public int atk;                      // ���ݷ�
    [SerializeField] public int def;                      // ����
    [SerializeField] private float moveSpeed;             // �ȴ� �ӵ�
    [SerializeField] private float runSpeed;              // �޸��� �ӵ�
    [SerializeField] private float sightRadius;           // �þ� �Ÿ�
    [SerializeField] private float sightAngle;            // �þ� �ݰ�
    [SerializeField] private float atkRadius;             // ���� �Ÿ�
    [SerializeField] private float atkAngle;              // ���� �ݰ�
    [SerializeField] private float actionTime = 0.2f;     // �ൿ ����
    [SerializeField] private CharacterController cc;      // ��� ��� ���� �� �̿�Ǵ� ������Ʈ
    [SerializeField] private Animator anim;               // ��� ������Ʈ
    [SerializeField] private NavMeshAgent agent;          // �� ã���? ������Ʈ
    [SerializeField] private MeshRenderer weaponMesh;     // ���� ����
    [SerializeField] private Transform targetTrans;       // Ÿ�� ��ġ
    [SerializeField] private string playerTag = "Player"; // ������ �ν��� �±�
    [SerializeField] private LayerMask targetMask;        // ������ �ν��� ���̾�
    [SerializeField] private LayerMask obstacleMask;      // ��ֹ��� �ν��� ���̾� 


    [SerializeField] [Range(1, 5)] private int phaseNum1; // 1������ ���� ���� ����
    [SerializeField] [Range(1, 5)] private int phaseNum2; // 2������ ���� ���� ����

    private WaitForSeconds waitTime;
    private Vector3 _dir;                                 // ���� ����
                                                          // �÷��̾� ��ġ - �� ��ġ 
    private int currentHp;                                // ���� ü��
    private Vector3 dir;                                  // �þ� ����
                                                          // dir���� y �� = 0�� ���̰� 1�� ����
    private bool isDelay;                                 // ���� ��� 


#if ExampleUnity
    // Ȯ�ο� ����
    float lookingAngle;
    Vector3 leftDir;
    Vector3 rightDir;
    Vector3 lookDir;
    Vector3 leftatkDir;
    Vector3 rightatkDir;
#endif

    // �� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    [PunRPC]
    public void Setup(int maxHp, int atk, int def, float moveSpeed)
    {
        Debug.Log("Setup �޼��� ����");

        // ü�� ����
        this.maxHp = maxHp;
        
        SetHp(); // currentHp <= 0 �̸� �ٷ� ���� ����
        
        weaponMesh.material.color = Color.white; // ���� ���� ����

        // ����޽� ������Ʈ�� �̵� �ӵ� ����
        // this.runSpeed = runSpeed;
        this.moveSpeed = moveSpeed;

        // ������
        this.atk = atk;

        // ����
        this.def = def;

        if (currentHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// cc�� agent 1���� �ҷ��͵� ����ϴ� �����ؼ� ����� ������
    /// </summary>
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        weaponMesh = GetComponentInChildren<MeshRenderer>();
    }

    /// <summary>
    /// ��Ȱ �� �� �ൿ hp �ʱ�ȭ, cc, agent üũ, �̵� �ӵ� ���� �׸��� �ൿ ����
    /// </summary>
    private void OnEnable() // ������Ʈ Ǯ�� ��� ����ؼ� OnEnable 
    {
        SetHp();

        // �̵� ����� ���� ��� ��Ȱ��ȭ
        if (cc == null || agent == null) 
        {

            gameObject.SetActive(false); 
        }

        // �ִϸ����� ��������
        anim = GetComponent<Animator>(); 
        anim?.SetBool("runBool", false);
        SetAtkNum();

        // �ʱ�� �ȴ� �ӵ�
        agent.speed = moveSpeed;

        // �ൿ ���� ����
        if (waitTime == null)
        {
            waitTime = new WaitForSeconds(actionTime); 
        }

        // �ൿ ���� !!
        StartCoroutine(Action()); 
    }

    /// <summary>
    /// hp ���� 0���ϸ� ��� ���·� ����� ��
    /// </summary>
    private void SetHp()
    {

        // 0���� ũ�� ��� ����, ������ ��� ���� �� 1������
        currentHp = maxHp;
        enemyState = currentHp > 0 ? EnemyState.Idle : EnemyState.Dead; 
        enemyPhase = EnemyPhase.first; 
        
    }


    private void OnDisable() 
    {
        // ������ ����?
    }

    /// <summary>
    /// FSM �˰���
    /// </summary>
    /// <returns>����� �ð�</returns>
    private IEnumerator Action() 
    {
        while (true)
        {
#if ExampleUnity
            // Gizmos�� Ȯ��
            lookingAngle = transform.eulerAngles.y;
            rightDir = AngleToDir(lookingAngle + sightAngle * 0.5f);
            leftDir = AngleToDir(lookingAngle - sightAngle * 0.5f);
            lookDir = AngleToDir(lookingAngle);
            rightatkDir = AngleToDir(lookingAngle + atkAngle * 0.5f);
            leftatkDir = AngleToDir(lookingAngle - atkAngle * 0.5f);
#endif


            // ���¿� ���� �ൿ 
            ChkAction(); 
     
            if (enemyState == EnemyState.Dead)
            {
                Debug.Log("�ڷ�ƾ ����");
                yield break;
            }

            // �տ��� ������ �ð���ŭ ���
            yield return waitTime; 
        }
    }

#if ExampleUnity
    /// <summary>
    /// �ð� �������� ������ŭ ȸ���� xz���� ������ ���� ���͸� ��ȯ
    /// </summary>
    /// <param name="angle">����( ���� : �� )</param>
    /// <returns>����</returns>
    Vector3 AngleToDir(float angle) // ���� ���� �޼ҵ�
    {
        float radian = angle * Mathf.Deg2Rad; // ���� ������ ��ȯ
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian)); // xz ���� ȸ�� ����
    }

    /// <summary>
    /// �þ� ǥ�� �޼ҵ�
    /// </summary>
    private void OnDrawGizmos()
    {
        // �þ� ����
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Debug.DrawRay(transform.position, rightDir * sightRadius, Color.blue);
        Debug.DrawRay(transform.position, leftDir * sightRadius, Color.blue);
        Debug.DrawRay(transform.position, lookDir * sightRadius, Color.cyan);
        
        // ���� ����
        Gizmos.DrawWireSphere(transform.position, atkRadius);
        Debug.DrawRay(transform.position, rightatkDir * atkRadius, Color.red);
        Debug.DrawRay(transform.position, leftatkDir * atkRadius, Color.red);
    }
#endif


    #region ���� ���� �޼ҵ�
    /// <summary>
    /// FSM ��� ���¸� ��� �ൿ �޼ҵ� ����̸� ��� ���� �޼ҵ� ����
    /// </summary>
    private void ChkAction() 
    {
        switch (enemyState) 
        {

            // ��� ���� �ȿ� �� ������ ������ ���� 
            case EnemyState.Idle: 
                StateIdle(); 
                break;

            // ��� �������� ������� Ȥ�� ���� ������ ���Դ��� üũ�ϰ� �̵�
            case EnemyState.Move: 
                StateMove(); 
                break;

            // ���� �ൿ ���� Ư�� Ÿ�ֿ̹� ���� ���� �ȿ� ������ �����Ѵ�
            case EnemyState.Attack: 
                StateAttack(); 
                              
                break;

            // �°ų� ������¸� �ƹ��͵� ����
            case EnemyState.Damaged: 
            case EnemyState.Dead:    
                break;
        }

        return;
    }

    /// <summary>
    /// ��� �ൿ �޼ҵ� - ��踸 ����.
    /// </summary>
    private void StateIdle() 
    {

        // ��� ������ ��������
        if (FindTarget(sightRadius, sightAngle)) 
        {

            // �̵� ���� ��ȯ �ִϸ��̼� ������ �̵� ����!
            enemyState = EnemyState.Move; 
            anim?.SetBool("walkBool", true); 

            return;
        }
    }

    /// <summary>
    /// �̵� �ൿ �޼ҵ� - ���� ���ư��� ����üũ�ϰ� Ÿ������ �̵��ϴ� �޼ҵ�
    /// </summary>
    private void StateMove() 
    {

        // ������ ������ ��
        if (isDelay) 
        {
            isDelay = false;
        }
        // ������ ���°� �ƴ� ��
        else
        {
            // �̵� �̿��� ��κ��� ���¿����� agent�� ���⿡ ���⼭ �����ִ°� üũ
            if (!agent.enabled) 
            {
                agent.enabled = true;
            }

            if (targetTrans != null)
            {
                // �������� Ÿ���� ���
                agent.destination = targetTrans.position;
                // Debug.Log("�̵� ��...");
            }

            // ���� ���� ���̸� ���ݻ���        
            if (FindTarget(atkRadius, atkAngle)) 
            {
                enemyState = EnemyState.Attack; 
                anim?.SetBool("runBool", false);
                anim?.SetBool("atkBool", true); 

                Debug.Log("����");

                return;
            }

            // ��� ���� ���� ������ üũ ����� ��� ����
            if (!FindTarget(sightRadius, sightAngle)) 
            {
                enemyState = EnemyState.Idle; 
                targetTrans = null; 
                agent.enabled = false; 
                anim?.SetBool("walkBool", false); 
                Debug.Log("��� ���� ����");

                return;
            }
        }

    }

    /// <summary>
    /// ���� �ൿ - ���� ��� ���ϰ�, Ÿ���� �������� ������ �ٶ󺻴�.
    /// </summary>
    public void StateAttack() 
    {

        // agent ���� ���ڸ� ���� �̶� ���� �̵��� ���� �̸� �Ȳ���
        if (agent.enabled)
        {

            agent.enabled = false;
        }

        // ���� ���� ���� �ȿ� �ִ��� üũ ������ �̵� ���� ����
        if (!FindTarget(atkRadius, atkAngle)) 
        {
            isDelay = true;
            enemyState = EnemyState.Move; 
            anim?.SetBool("atkBool", false); 
            anim?.SetBool("walkBool", true); 

            Debug.Log("�̵� ���� ����");

            return;
        }

        transform.LookAt(targetTrans);
    }

    #endregion


    #region Aniamtor Event �޼ҵ�
    /// <summary>
    /// ��ä�� ���� ���� �޼ҵ�
    /// </summary>
    public void AttackSector()
    {

        // targetMask�� ������ ���� ���� �ݰ�ȿ� ���Դ��� Ȯ��
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, atkRadius, targetMask);

        // ���� �ְ� �ִ��� Ȯ��
        if (cols.Length > 0) 
        {

            foreach (var item in cols)
            {

                // cc, capsuleCollider 2�� �Ǻ��ؼ� �ϳ��� �Ǻ��ϰ� ĳ������
                if (item as CapsuleCollider == null || item.gameObject == gameObject) 
                {                                                                       

                    continue;
                }

                _dir = (item.gameObject.transform.position - transform.position).normalized;
                dir = _dir;
                dir.y = 0;
                dir = dir.normalized;

                // ���� �ݰ� �ȿ� �ִ� ���
                if (Vector3.Angle(_dir, transform.forward) < atkAngle * 0.5f) 
                {

                    // ���� �� ���̿� ��ֹ��� �ִ��� üũ
                    // ������ ������ ����!
                    // ���� ��� �÷��̾�� �� ���̿� ���� �ִµ� ������ ���� ��� ���� 
                    RaycastHit hit;

                    // ���� ��ġ���� �÷��̾� �������� �������� ���
                    if (Physics.Raycast(transform.position + cc.center, _dir, out hit, atkRadius, targetMask | obstacleMask)) 
                    {

                        // hit�� �±׸� ��
                        if (hit.transform.tag == playerTag) 
                        {
                            // �� �߰�! ������ 2�� �������� �߰�
                            int dmg = enemyPhase == EnemyPhase.second ? 5 + atk : atk ; 

                            // ������ �ִ� ����
                            item.gameObject.GetComponent<StatusController>()?.DecreaseHP(dmg); 
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// ������� ���ݿ� �� ������ ������
    /// </summary>
    public void AttackRay()
    {

        // ������ ���� �ִ��� �Ǻ�
        RaycastHit hit;
        if(Physics.Raycast(transform.position + cc.center, dir, out hit, atkRadius, targetMask))
        {

            // ����� ���� �� ����
            int dmg = enemyPhase == EnemyPhase.second ? 5 + atk : atk; 

            hit.transform.gameObject.GetComponent<StatusController>()?.DecreaseHP(dmg);
        }
    }

    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    public void SetAtkNum()
    {
        // ������ 2���� ȸ���� 3�޺� ���� ����
        if (enemyPhase == EnemyPhase.first) 
        {
            anim?.SetInteger("atkInt", UnityEngine.Random.Range(0, phaseNum1));
        }
        else
        {
            anim?.SetInteger("atkInt", UnityEngine.Random.Range(0, phaseNum2));
        }
    }

    /// <summary>
    /// �´� ��� Ż�� �޼ҵ�
    /// </summary>
    public void EscapeDamaged()
    {

        enemyState = EnemyState.Move; 
    }

    /// <summary>
    /// ��� ��� Ż�� �޼ҵ�
    /// </summary>
    public void EscapeDie()
    {
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// �����ȿ� �ִ��� �Ǻ��ϴ� �޼ҵ�
    /// </summary>
    /// <param name="Radius">�Ÿ�</param>
    /// <param name="Angle">����</param>
    /// <returns>������ true,  ������ false�̰� Ÿ���� transform�� ��´�</returns>
    private bool FindTarget(float Radius, float Angle)
    {

        // ���� ���� �ȿ� ���� �ִ��� üũ
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, Radius, targetMask);

        // �ʱ�ȭ
        dir = Vector3.zero; 

        if (cols.Length > 0)
        {
            foreach(var item in cols)
            {

                // �ڱ��ڽŸ� �ƴϰ� Ȯ��
                if (item.gameObject == gameObject) 
                    continue;

                _dir = (item.gameObject.transform.position - transform.position);
                dir = _dir;
                dir.y = 0;
                dir = dir.normalized;

                // �����ȿ� ���� �մ°�?
                if (Vector3.Angle(_dir, transform.forward) < Angle * 0.5f)
                {

                    // ��� ���̿� ���̳� ��ֹ��� �ִ��� ���� X �ڵ�
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + cc.center, _dir, out hit, Radius, targetMask | obstacleMask))
                    {

                        // Player �±׸� ���� ���� �����ϸ�
                        if (hit.transform.tag == playerTag)
                        {

                            // ��� ���¿����� ������ transform�� ��´�
                            if (enemyState == EnemyState.Idle) 
                            {

                                // ����� transform ����
                                targetTrans = item.gameObject.transform; 
                            }

                            return true;
                        }
                    }
                }
            }
        }


        // ��ã�����Ƿ� �׾��� ���� �־� null �� ��ȯ�� false
        targetTrans = null;
        return false;
    }


    #region �ܺο� ��ȣ�ۿ��� �޼ҵ�
    /// <summary>
    /// �ǰ� �޼ҵ�
    /// </summary>
    /// <param name="attack">������ ����� ���ݷ�!</param>
    public void OnDamagedProcess(int attack = 0)
    {
        // �ּ� ������ 1 ���� ������ ���� 
        // ���ݷ� - ���¸�ŭ ��´�
        currentHp -= CalcDmg(attack);

        // ������ üũ
        if (enemyPhase == EnemyPhase.first)
        {
            ChkPhase(); 
        }

        // hp�� ���� ��� ����
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
    /// ������ ���� �ּҵ����� 1 ����!
    /// </summary>
    /// <param name="attack">���� ���ݷ�</param>
    /// <returns>���ݷ� - ����, �ּ� ������ 1����</returns>
    private int CalcDmg(int attack)
    {

        int dmg = attack - this.def;
        if (dmg < 1) dmg = 1;

        return dmg;
    }

    /// <summary>
    /// 70% �̸��̸� ������ ���� 
    /// </summary>
    private void ChkPhase() 
    {

        // ���� ������ �޸��� ��� ����
        if (((10 *currentHp) / maxHp )  <= 6) 
        {

            enemyPhase = EnemyPhase.second; 
            weaponMesh.material.color = Color.red; 
            anim?.SetBool("runBool", true);
            agent.speed = runSpeed;
        }
    }

    /// <summary>
    /// �ǰ� ��� ���ϰ� �ϴ� �޼ҵ�
    /// </summary>
    private void ChkDamaged()
    {
        // �ǰ� ��� ������ �������� �Ǻ�
        if (enemyState != EnemyState.Damaged
            && enemyState != EnemyState.Attack
            && !isDelay)
        {
            isDelay = true;
            enemyState = EnemyState.Damaged;
            anim?.SetTrigger("dmgTrigger");
        }
        return;
    }


    /// <summary>
    /// ��� ��� ���ϴ� �޼ҵ�
    /// </summary>
    private void ChkDie()
    {

        // ���� �� ���� ��
        currentHp = 0; 
        if (enemyState != EnemyState.Dead)
        {
            enemyState = EnemyState.Dead; 
            anim?.SetTrigger("dieTrigger");
        }

        return;
    }
    #endregion
}
