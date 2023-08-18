using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public enum STATE_UNIT { DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, ATTACK = 3, PATROL = 4, HOLD = 5, SKILL1 = 6, SKILL2 = 7, SKILL3 = 8, ATTACKING = 11, HOLD_ATTACKING = 12 }


public class Unit : Selectable
{

    #region ����
    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 targetPos;
    [SerializeField] protected Vector3 patrolPos;

    [SerializeField] protected STATE_UNIT myState;
    [SerializeField] protected StateAction myStateAction;
    [SerializeField] protected Attack myAttack;  

    [SerializeField] public LayerMask atkLayers;

    [Header("�� ����")]
    [SerializeField] protected int atk;
    [SerializeField] protected float atkRange;
    [SerializeField] protected float atkTime;
    [SerializeField] protected float atkDoneTime;

    [SerializeField] protected float chaseRange;

    [SerializeField] protected bool stateChange;

    [SerializeField] protected float applySpeed;

    protected WaitForSeconds atkTimer;
    protected WaitForSeconds atkDoneTimer;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #endregion ����

    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public Attack MyAttack => myAttack;
    public StateAction MyStateAction => myStateAction;

    public int Atk => atk;
    public float AtkTime => atkTime;
    public float AtkRange => atkRange;
    public float ChaseRange => chaseRange;

    public float ApplySpeed => applySpeed;

    public Transform Target
    {

        get { return target; }
        set { target = value; }
    }

    public Vector3 TargetPos
    {

        get { return targetPos; }
        set { targetPos = value; }
    }

    public Vector3 PatrolPos
    {

        get { return patrolPos; }
        set { patrolPos = value; }
    }

    public int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_UNIT)value; }
    }



        

    /// <summary>
    /// ��� ���� �� �ִ� �������� üũ
    /// </summary>
    protected virtual bool atCommand
    {

        get
        {

            return myState == STATE_UNIT.NONE 
                && cmds.Count > 0;
        }
    }

    protected virtual bool atkReaction
    {

        get
        {

            return myState == STATE_UNIT.NONE
                || myState == STATE_UNIT.PATROL;
        }
    }

    #endregion ������Ƽ

    protected virtual void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        myAttack = GetComponent<Attack>();
        myStateAction = GetComponent<StateAction>();
        
        cmds = new Queue<Command>(MAX_COMMANDS);

        SetTimer();
    }
    protected virtual void OnEnable()
    {

        Init();
    }


    protected virtual void FixedUpdate()
    {

        Action();
    }


    protected override void Init()
    {
        
        base.Init();
        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        myAgent.speed = applySpeed;
        ActionDone();
        cmds.Clear();
    }

    /// <summary>
    /// Ÿ�̸� ����
    /// </summary>
    protected virtual void SetTimer()
    {

        atkTimer = new WaitForSeconds(atkTime);
        atkDoneTimer = new WaitForSeconds(atkDoneTime);
    }

    /// <summary>
    /// �ش� ������ �ൿ
    /// ���� �������� ���� �ִ�!
    /// </summary>
    protected virtual void Action()
    {

        if (myState == STATE_UNIT.DEAD) return;

        // ���� ��ȭ�� �ִ���
        else if (stateChange)
        {

            stateChange = false;

            // ����� �ְ� ���� �� �ִ��� Ȯ��
            if (atCommand) ReadCommand();
            myStateAction.Changed(this);
        }
        // �ൿ ����
        else myStateAction.Action(this);

    }

    /// <summary>
    /// �ൿ�� �Ϸ� Ȥ�� ������ �ʿ�
    /// </summary>
    /// <param name="_nextState">���� ����</param>
    public virtual void ActionDone(STATE_UNIT _nextState = STATE_UNIT.NONE)
    {

        myState = _nextState;
        stateChange = true;
    }

    /// <summary>
    /// ������ Ÿ�� ã��
    /// </summary>
    /// <param name="isChase">true�� ���� ����, false�� ���� ����</param>
    public virtual void FindTarget(bool isChase)
    {

        // �˻��ϴ� ������ �ڽ� �ݶ��̴��� ���� �־� hits�� �ּ� ũ�� 1�� ����ȴ�
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                   isChase ? chaseRange : atkRange, transform.forward, 0f, atkLayers);


        float minDis = isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        target = null;

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == transform)
            {

                continue;
            }

            // ���� ����� �� ����!
            float targetDis = Vector3.SqrMagnitude(transform.position - hits[i].transform.position);
            if (minDis > targetDis)
            {

                minDis = targetDis;
                target = hits[i].transform;
            }
        }

    }

    public virtual void OnAttack()
    {

        if (myAttack.IsAtk)
        {

            myAttack.ChkCoolTime(this);
        }
        else
        {

            myAttack.IsAtk = true;
            myAgent.ResetPath();
            transform.LookAt(target.position);
        }
    }

    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        OnDamageAction(_trans);
    }

    protected virtual void OnDamageAction(Transform _trans)
    {

        if (_trans != null || !atkReaction) return;

        else if (myAttack == null)
        {

            // ������ ���ϸ� �ݴ� �������� ����!
            Vector3 dir = (transform.position - _trans.position).normalized;
            targetPos = transform.position + dir * applySpeed * 0.5f;
            ActionDone(STATE_UNIT.MOVE);
        }
        else
        {

            // ������ �� ������ �¹޾�ģ��!
            target = _trans;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    protected virtual void KnockBack(Transform _trans)
    {

    }

    public override void Dead()
    {

        base.Dead();

        ActionDone(STATE_UNIT.DEAD);
        StopAllCoroutines();
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);
    }

    #region Command
    /// <summary>
    /// ��� �ޱ�
    /// ���� ����� �ƴ� ��� 
    /// ���� �� ���°� �ƴϸ� ���� �ൿ�� ����ϰ� 1�� �ڿ� ����� �����Ѵ�
    /// </summary>
    /// <param name="_cmd">���� ���</param>
    /// <param name="_add">���� ����ΰ�?</param>
    public override void GetCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        // ���� ����� �ƴ� ��� ������ ���� ��� �ʱ�ȭ��
        // ���� �Ͽ� ���� ����� ������ �� �ְ� NONE ���·� ����
        if (!_add)
        {

            ActionDone();
            cmds.Clear();
        }

        // ��� ���, ���� ����� ��� �ִ� �� Ȯ�� �Ѵ�
        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }

    /// <summary>
    /// ��� �б�
    /// </summary>
    public override void ReadCommand()
    {
        
        Command cmd = cmds.Dequeue();

        // ��ϵ� �ൿ�� �ִ��� Ȯ��
        // ���� �� ���� �ൿ�̸� ��ɸ� �������
        if (!ChkState(cmd.type)) return;
        myState = (STATE_UNIT)cmd.type;
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        cmd.Received(MySize);
    }

    /// <summary>
    /// �ൿ�� �� �ִ� �������� üũ
    /// </summary>
    /// <param name="_num">���� ��ȣ</param>
    /// <returns></returns>
    protected virtual bool ChkState(int _num)
    {

        if (myStateAction.ChkActions(_num))
        {

            return true;
        }

        return false;
    }
    #endregion Command
}
