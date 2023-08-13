using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum STATE_UNIT { DEAD = -1, NONE = 0, MOVE = 1, STOP = 2, ATTACK = 3, PATROL = 4, HOLD = 5, REPAIR = 6, SKILL1 = 7, SKILL2 = 8, SKILL3 = 9, ATTACKING = 11, HOLD_ATTACKING = 12 }

public class Unit : Selectable
{

    [Header("���� ����")]
    [SerializeField] protected Animator myAnimator;
    [SerializeField] protected Collider myCollider;
    [SerializeField] protected NavMeshAgent myAgent;
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 targetPos;
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

    protected WaitForSeconds atkTimer;
    protected WaitForSeconds atkDoneTimer;

    protected Queue<Command> cmds;
    public static readonly int MAX_COMMANDS = 5;

    #region ������Ƽ
    public Animator MyAnimator => myAnimator;
    public Collider MyCollider => myCollider;
    public NavMeshAgent MyAgent => myAgent;

    public Attack MyAttack => myAttack;
    public StateAction MyStateAction => myStateAction;

    public int Atk => atk;
    public float AtkRange => atkRange;
    public float ChaseRange => chaseRange;


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

    public int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_UNIT)value; }
    }

    public bool StateChange
    {

        get
        {

            bool result = stateChange;
            if (result) stateChange = false;
            return result;
        }
    }
    #endregion ������Ƽ

    protected virtual void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();

        myAttack = GetComponent<Attack>();
        myStateAction = GetComponent<StateAction>();

        cmds = new Queue<Command>(MAX_COMMANDS);
        SetTimer();
    }
    
    protected virtual void SetTimer()
    {

        atkTimer = new WaitForSeconds(atkTime);
        atkDoneTimer = new WaitForSeconds(atkDoneTime);
    }

    /// <summary>
    /// �ൿ�� �Ϸ�Ǿ����� �Ǻ�!
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

        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                   isChase ? chaseRange : atkRange, transform.forward, 0f, atkLayers);

        if (hits.Length > 0)
        {

            float minDis = isChase ? chaseRange + 1f : atkRange + 1f;

            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].transform == transform)
                {

                    continue;
                }

                if (minDis > hits[i].distance)
                {

                    minDis = hits[i].distance;
                    Target = hits[i].transform;
                }
            }
        }
    }

    public virtual void OnAttack()
    {

        StartCoroutine(AttackCoroutine());
    }

    protected IEnumerator AttackCoroutine()
    {

        myState = myState == STATE_UNIT.HOLD ? STATE_UNIT.HOLD_ATTACKING : STATE_UNIT.ATTACKING;
        yield return atkTimer;


        myAttack.OnAttack(this);
        yield return atkDoneTimer;

        myAttack.AttackDone(this);
    }

    public override void DoCommand(Command _cmd, bool _add = false)
    {

        if (myState == STATE_UNIT.DEAD) return;

        if (!_add)
        {

            stateChange = true;
            cmds.Clear();
        }

        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }

    public override void ReadCommand()
    {

        Command cmd = cmds.Dequeue();

        if (ChkState(cmd.type)) return;
        myState = (STATE_UNIT)cmd.type;
        target = cmd.target != transform ? cmd.target : null;
        targetPos = cmd.pos;

        stateChange = true;
    }

    protected virtual bool ChkState(int _num)
    {

        if (myStateAction.ChkActions(_num))
        {

            return true;
        }

        return false;
    }
}
