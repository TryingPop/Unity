using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

/// <summary>
/// ������ �⺻�� �Ǵ� Ŭ����
/// �̵��� �����ϴ�!
/// </summary>
public class BaseUnit : Selectable, IMovable       //
{

    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    public float applySpeed;                    // ����� �̵� �ӵ�

    protected Command cmd;                      // ���
    protected Queue<Command> cmds;              // ���� ���

    public static readonly int MAX_COMMANDS = 5;// �ִ� ��� ��

    protected virtual void Awake()
    {

        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();

        cmds = new Queue<Command>(MAX_COMMANDS);
    }

    protected virtual void OnEnable()
    {

        Init();
    }

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected override void Init()
    {
        base.Init();
        OnMoveStop();
        cmds.Clear();
        cmd = null;
    }

    /// <summary>
    /// Fixedupdate���� �Ź� Ȯ���� ����
    /// </summary>
    public virtual void Action()
    {

        // Ÿ���� ������ Ÿ�ٸ� �Ѵ´�
        if (cmd.target != null)
        {

            // Ÿ���� ��� ���� ��� Ÿ�ٸ� �Ѵ´�
            if (cmd.target.gameObject.activeSelf) OnMove(cmd.target.position);
            else
            {

                // ���� ���
                cmd.target = null;
                myAgent.destination = transform.position;
            }
        }


        if (myAgent.remainingDistance < 0.1f)
        {

            // �������� �Ÿ��� 0.1�̸� ���߰� ���� �������� �̵�
            if (cmds.Count == 0)
            {

                // ���� �������� ������ ������� ���Ѵ�
                myAnimator.SetFloat("Move", 0f);
                return;
            }
            else
            {

                // ���� Ŀ�Ǵ� �б�
            }
        }
    }

    /// <summary>
    /// �̵� ����
    /// </summary>
    public virtual void OnMoveStop()
    {

        myAgent.destination = transform.position;
        myAnimator.SetFloat("Move", 0f);
    }

    /// <summary>
    /// �ش� ��ǥ�� �̵�
    /// </summary>
    /// <param name="_destination">�̵��� ���</param>
    public virtual void OnMove(Vector3 _destination)
    {

        myAgent.SetDestination(_destination);
        myAnimator.SetFloat("Move", 1f);
    }

    #region command

    public override void AddCommand(Command _cmd)
    {

        if (cmds.Count < MAX_COMMANDS) cmds.Enqueue(_cmd);
    }

    public override void SetCommand(Command.TYPE _type, Vector3 _pos, Transform _target = null)
    {

        cmds.Clear();
        cmd.type = _type;
        cmd.Set(_pos, _target);
    }
    protected virtual void ReadCommand()
    {

        // ����� ���� ���
        if (cmd == null && cmds.Count == 0) return;
        else if (cmd == null && cmds.Count > 0) cmd = cmds.Dequeue();

        // �бⰡ ������ .. �̰� �ϵ��ڵ� ���ΰ�?
    }
    #endregion
}
