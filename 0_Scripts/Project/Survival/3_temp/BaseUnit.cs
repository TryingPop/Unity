using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

/// <summary>
/// 유닛의 기본이 되는 클래스
/// 이동이 가능하다!
/// </summary>
public class BaseUnit : Selectable, IMovable       //
{

    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    public float applySpeed;                    // 적용된 이동 속도

    protected Command cmd;                      // 명령
    protected Queue<Command> cmds;              // 예약 명령

    public static readonly int MAX_COMMANDS = 5;// 최대 명령 수

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
    /// 초기화 메서드
    /// </summary>
    protected override void Init()
    {
        base.Init();
        OnMoveStop();
        cmds.Clear();
        cmd = null;
    }

    /// <summary>
    /// Fixedupdate에서 매번 확인할 예정
    /// </summary>
    public virtual void Action()
    {

        // 타겟이 있으면 타겟만 쫓는다
        if (cmd.target != null)
        {

            // 타겟이 살아 있을 경우 타겟만 쫓는다
            if (cmd.target.gameObject.activeSelf) OnMove(cmd.target.position);
            else
            {

                // 죽은 경우
                cmd.target = null;
                myAgent.destination = transform.position;
            }
        }


        if (myAgent.remainingDistance < 0.1f)
        {

            // 목적지와 거리가 0.1이면 멈추고 다음 목적지로 이동
            if (cmds.Count == 0)
            {

                // 다음 목적지가 없으면 대기모션을 취한다
                myAnimator.SetFloat("Move", 0f);
                return;
            }
            else
            {

                // 다음 커맨더 읽기
            }
        }
    }

    /// <summary>
    /// 이동 중지
    /// </summary>
    public virtual void OnMoveStop()
    {

        myAgent.destination = transform.position;
        myAnimator.SetFloat("Move", 0f);
    }

    /// <summary>
    /// 해당 좌표로 이동
    /// </summary>
    /// <param name="_destination">이동할 장소</param>
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

        // 명령이 없는 경우
        if (cmd == null && cmds.Count == 0) return;
        else if (cmd == null && cmds.Count > 0) cmd = cmds.Dequeue();

        // 읽기가 문제네 .. 이거 하드코딩 각인가?
    }
    #endregion
}
