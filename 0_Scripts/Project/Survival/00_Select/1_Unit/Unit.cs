using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;




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
    [SerializeField] protected short maxMp;
    protected short curMp;
    protected int atk;
    #endregion ����


    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Rigidbody MyRigid => myRigid;

    public Attack MyAttack => myAttack;
    public UnitStateAction MyStateAction => myStateAction;

    public Vector3 PatrolPos
    {

        get { return patrolPos; }
        set { patrolPos = value; }
    }

    public override int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_UNIT)value; }
    }

    public int CurMp
    {

        get { return maxMp == 0 ? -1 : curMp; }
        set
        {

            if (maxMp == VariableManager.INFINITE) return;
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

    public int Atk => atk;

    #endregion ������Ƽ

    protected virtual void Awake()
    {

        // transform �� ���� �ٴϴ°� ���ٴ� ���Ǵ� ������ ĳ������!
        // ���Ĺ����� �ٸ� ������� Ȯ���� ��� ���� �ٴϴ°� ������ �� ���ٰ� �Ѵ�
        // https://forum.unity.com/threads/cache-transform-really-needed.356875/
        // https://geekcoders.tistory.com/56
        // transform = GetComponent<Transform>();
        cmds = new Queue<Command>(VariableManager.MAX_RESERVE_COMMANDS);
    }


    protected virtual void OnEnable()
    {

        Init();
    }

    public override void SetStat()
    {
        base.SetStat();
        atk = myAttack.atk;

        if (myUpgrades != null)
        {

            atk = myUpgrades.AddAtk;
        }
        
        myHitBar?.SetMaxHp(maxHp);
    }

    protected override void Init()
    {

        SetStat();
        base.Init();
        curMp = maxMp;
        
        myAnimator.SetBool("Die", false);
        myAgent.enabled = true;
        ActionDone();
        cmds.Clear();
    }

    protected void Start()
    {

        AfterSettingLayer();
    }

    /// <summary>
    /// ������ ������ ��� layer ���� ���Ŀ� �ٽ� �� �� �� �����Ѵ�!
    /// </summary>
    public override void AfterSettingLayer()
    {

        myAlliance = TeamManager.instance?.GetTeamInfo(gameObject.layer);
        if (mySight == null) { }
        else if (gameObject.layer == VariableManager.LAYER_PLAYER)
        {

            mySight.isStop = true;
            mySight.SetSize(myAttack == null ? 10 : myAttack.chaseRange);
        }
        else
        {

            mySight.isStop = false;
        }

        if (ActionManager.instance.ContainsUnit(this)) ActionManager.instance.RemoveUnit(this);
        if (myHitBar != null) ActionManager.instance.ClearHitBar(myHitBar);
        ActionManager.instance.AddUnit(this);
        MyHitBar = ActionManager.instance.GetHitBar();
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

        if (myAttack == null || ((1 << _trans.gameObject.layer) & myAlliance.GetLayer(false)) == 0)
        {

            // ������ �� ���ų� ������ ����� �Ʊ��� ��� �ݴ� �������� ����
            Vector3 dir = (transform.position - _trans.transform.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_UNIT.MOVE);
        }
        else
        {

            // ������ �� �ְ� ���� ������ ��� �¹޾�ģ��!
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
        ActionManager.instance.ClearHitBar(myHitBar);
    }

    public override void GiveButtonInfo(ButtonInfo[] _buttons)
    {

        myStateAction.GiveMyButtonInfos(_buttons, 0, 1);
    }

    public override void ChkButtons(ButtonInfo[] _buttons)
    {

        // ����!
        myStateAction.ChkButtons(_buttons, 5, 6, false);
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

        if (myState == STATE_UNIT.DEAD) 
        {

            _cmd.Canceled();
            return; 
        }

        // ���콺 R�� ��� ����� �ٲ۴�!
        if (_cmd.type == VariableManager.MOUSE_R)
        {

            // ���콺 R��ư�� ���� ��� �̵��̳� ���� Ÿ������ �ٲ۴�
            if (myAttack == null)
            {

                // ������ �� ���� ���
                _cmd.type = 1;
            }
            else if (_cmd.target == null)
            {

                // ����� ���� ���
                _cmd.type = 1;
            }
            else if ((myAlliance.GetLayer(false) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // ����� ���̰� ���� ������ ���¸� ����� ����
                _cmd.type = 5;
            }
            else
            {

                // ����� �Ʊ��� ���
                _cmd.type = 1;
            }
        }

        // ���� ����� �ƴ� ��� ������ ���� ��� �ʱ�ȭ��
        // ���� �Ͽ� ���� ����� ������ �� �ְ� NONE ���·� ����
        if (!_add)
        {

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

            // ��ų ��� �߿��� ��ɵ� �������ϰ� ��� ������ �ȵǰ� Ż��!
            // if (usingSkill) return;

            // ������ٵ� �ٷ�� ��쵵 �ֱ⿡
            ActionDone();
        } 
        else if (myState == STATE_UNIT.NONE)
        {

            stateChange = true;
        }

        // ��� ���, ���� ����� ��� �ִ� �� Ȯ�� �Ѵ�
        if (cmds.Count < VariableManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }

    /// <summary>
    /// ��� �б�
    /// </summary>
    public void ReadCommand()
    {
        
        Command cmd = cmds.Dequeue();

        // ��ϵ� �ൿ�� �ִ��� Ȯ��
        // ���� �� ���� �ൿ�̸� ��ɸ� �������
        if (!ChkState(cmd)) 
        {

            cmd.Canceled();
            return; 
        }

        myState = (STATE_UNIT)(cmd.type);       
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;
        cmd.Received(myStat.MySize);
    }

    /// <summary>
    /// �ൿ�� �� �ִ� �������� üũ
    /// </summary>
    /// <param name="_num">���� ��ȣ</param>
    /// <returns></returns>
    protected bool ChkState(Command _cmd)
    {

        // ���콺 ������ �ƴ� ��� �ൿ �������� Ȯ���Ѵ�
        return myStateAction.ChkAction(_cmd.type);
    }

    #endregion Command
}
