using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    [SerializeField] protected UnitStateAction myStateAction;
    // [SerializeField] protected Attack[] myAttacks;
    [SerializeField] protected Attack myAttack;
    [SerializeField] protected SightMesh mySight;

    [SerializeField] protected Vector3 patrolPos;
    
    [Header("�� ����")]
    [SerializeField] protected bool stateChange;
    [SerializeField] protected float applySpeed;
    [SerializeField] protected short maxMp;
    protected short curMp;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #endregion ����

    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    // public Attack[] MyAttacks => myAttacks;
    public Attack MyAttack => myAttack;
    public UnitStateAction MyStateAction => myStateAction;

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

        // transform �� ���� �ٴϴ°� ���ٴ� ���Ǵ� ������ ĳ������!
        // ���Ĺ����� �ٸ� ������� Ȯ���� ��� ���� �ٴϴ°� ������ �� ���ٰ� �Ѵ�
        // https://forum.unity.com/threads/cache-transform-really-needed.356875/
        // https://geekcoders.tistory.com/56
        // transform = GetComponent<Transform>();

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        myStateAction = GetComponent<UnitStateAction>();
        mySight = GetComponentInChildren<SightMesh>();
        myAttack = GetComponent<Attack>();
        cmds = new Queue<Command>(MAX_COMMANDS);
    }
    
    protected virtual void OnEnable()
    {

        Init();
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
        if (mySight == null) { }
        else if (gameObject.layer == 17)
        {

            mySight.isStop = true;
            // mySight.SetSize(myAttacks?[0] == null ? 10 : myAttacks[0].chaseRange); 
            mySight.SetSize(myAttack == null ? 10 : myAttack.chaseRange);
        }
        else
        {

            // mySight.SetSight(0, 20);
            mySight.isStop = false;
        }
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
        myTurn = 0;
        if (!myAgent.updateRotation) myAgent.updateRotation = true;
    }


    public override void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (myState == STATE_UNIT.DEAD) return;

        base.OnDamaged(_dmg, _trans);

        Selectable select = null;
        if (_trans != null) select = _trans.GetComponent<Selectable>();
        OnDamageAction(select);
    }

    protected virtual void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !atkReaction) return;

        // else if (myAttacks == null || myAttacks[0] == null)
        if (myAttack == null)
        {

            // ������ ���ϸ� �ݴ� �������� ����!
            Vector3 dir = (transform.position - _trans.transform.position).normalized;
            targetPos = transform.position + dir * applySpeed * 0.5f;
            ActionDone(STATE_UNIT.MOVE);
        }
        else
        {

            // ������ �� ������ �¹޾�ģ��!
            target = _trans;
            targetPos = _trans.transform.position;
            ActionDone(STATE_UNIT.ATTACK);
        }
    }

    public override void Dead()
    {

        base.Dead();

        for (int i = cmds.Count; i > 0; i--)
        {

            cmds.Dequeue().Canceled();
        }

        ActionDone(STATE_UNIT.DEAD);
        myAgent.enabled = false;
        myAnimator.SetBool("Die", true);
        ActionManager.instance.RemoveUnit(this);
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

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

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
    public void ReadCommand()
    {
        
        Command cmd = cmds.Dequeue();

        ChkType(cmd);

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

    protected void ChkType(Command cmd)
    {

        if (cmd.type != 10) return;

        // if (myAttacks[0] == null)
        if (myAttack == null)
        {

            // ������ �� ���� ���
            cmd.type = 1;
        }
        else if (cmd.target == null)
        {

            // ����� ���� ���
            cmd.type = 1;
        }
        // else if ((myAttacks[0].atkLayers & (1 << cmd.target.gameObject.layer)) != 0)
        else if ((myAttack.atkLayers & (1 << cmd.target.gameObject.layer)) != 0)
        {

            // ����� ���̰� ���� ������ ���¸� ����� ����
            cmd.type = 5;
        }
        else
        {

            // ����� �Ʊ��� ���
            cmd.type = 1;
        }
    }
    #endregion Command
}
