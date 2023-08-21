using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// -1 ~ 5 �������� �Ϲ� ������ ���� ��ȣ
/// 6�����ʹ� Ư��!
/// </summary>
public enum STATE_UNIT { DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, PATROL = 3, HOLD = 4, ATTACK = 5, REPAIR = 5, HEAL = 5,
    SKILL0 = 5, SKILL1 = 6, SKILL2 = 7, SKILL3 = 8 }


public class Unit : Selectable
{

    #region ����
    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Rigidbody myRigid;

    [SerializeField] protected STATE_UNIT myState;
    [SerializeField] protected StateAction myStateAction;
    [SerializeField] protected Attack[] myAttacks;  

    [SerializeField] protected Vector3 patrolPos;
    

    [Header("�� ����")]

    [SerializeField] protected bool stateChange;

    [SerializeField] protected float applySpeed;

    [SerializeField] protected byte maxMp;
    protected byte curMp;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #endregion ����

    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public Attack[] MyAttacks => myAttacks;

    public StateAction MyStateAction => myStateAction;

    public float ApplySpeed => applySpeed;

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

    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == ISkillAction.INFINITE_MP) return;
            if (value > maxMp) value = maxMp;
            curMp = (byte)value;
        }
    }

    protected virtual bool usingSkill
    {

        get
        {

            return myState == STATE_UNIT.SKILL1
                || myState == STATE_UNIT.SKILL2
                || myState == STATE_UNIT.SKILL3;
        }
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
        myAttacks = GetComponents<Attack>();
        myStateAction = GetComponent<StateAction>();
        
        cmds = new Queue<Command>(MAX_COMMANDS);
    }
    
    protected virtual void OnEnable()
    {

        Init();
    }

    protected virtual void OnDisable()
    {

        ActionManager.instance.RemoveUnit(this);
    }

    protected override void Init()
    {
        
        base.Init();
        curMp = maxMp;

        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        myAgent.speed = applySpeed;
        ActionDone();
        cmds.Clear();

        ActionManager.instance.AddUnit(this);
    }



    /// <summary>
    /// �ش� ������ �ൿ
    /// </summary>
    public void Action()
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

        if (!myAgent.updateRotation) myAgent.updateRotation = true;
    }



    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        OnDamageAction(_trans);
    }

    protected virtual void OnDamageAction(Transform _trans)
    {

        if (_trans == null || !atkReaction) return;

        else if (myAttacks == null)
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
            targetPos = _trans.position;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    public override void Dead()
    {

        base.Dead();

        ActionDone(STATE_UNIT.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);
        // ActionManager.instance.RemoveUnit(this);
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

            cmds.Clear();

            // ��ų ��� �߿��� ��ɵ� �������ϰ� ��� ������ �ȵǰ� Ż��!
            if (usingSkill) return;     
            
            // ������ٵ� �ٷ�� ��쵵 �ֱ⿡
            ActionDone();
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
    protected bool ChkState(int _num)
    {

        if (myStateAction.ChkActions(_num))
        {

            return true;
        }

        return false;
    }
    #endregion Command
}
