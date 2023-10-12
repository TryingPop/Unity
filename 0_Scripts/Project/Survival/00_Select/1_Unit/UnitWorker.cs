using System.Collections;using System.Collections.Generic;
using UnityEngine;

public class UnitWorker : Unit
{

    public override void GetCommand(Command _cmd, bool _add = false)
    {
        
        if (myState == STATE_SELECTABLE.DEAD)
        {

            _cmd.Canceled();
            return;
        }

        int idx = (int)_cmd.type;

        // 마우스 R인 경우 명령을 바꾼다!
        if (idx == VariableManager.MOUSE_R)
        {

            // 마우스 R버튼을 누른 경우 이동이나 공격 타입으로 바꾼다
            if (myAttack == null)
            {

                // 공격할 수 없는 경우
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target == null)
            {

                // 대상이 없는 경우
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target.MyStat.MyType == TYPE_SELECTABLE.BUILDING
                && (myAlliance.GetLayer(true) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // 대상이 건물인 경우 수리하러 간다!
                _cmd.type = STATE_SELECTABLE.UNIT_ATTACK;
            }
            else
            {

                // 대상이 수리 대상도 아니고, 건설 대상도 아니므로 그냥 따라다닌다
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
            }
        }

        // 예약 명령이 아닌 경우 기존에 예약 명령 초기화와
        // 다음 턴에 예약 명령을 실행할 수 있게 NONE 상태로 변경
        if (!_add)
        {

            for (int i = cmds.Count; i > 0; i--)
            {

                cmds.Dequeue().Canceled();
            }

            // 스킬 사용 중에는 명령들 삭제만하고 명령 실행은 안되게 탈출!
            // if (usingSkill) return;

            // 리지드바디를 다루는 경우도 있기에
            ActionDone();
        }
        else if (myState == STATE_SELECTABLE.NONE)
        {

            stateChange = true;
        }

        // 명령 등록, 예약 명령인 경우 최대 수 확인 한다
        if (cmds.Count < VariableManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}의 명령어가 가득 찼습니다.");
    }


    protected override void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !atkReaction) return;

        // 공격을 못하면 반대 방향으로 도주!
        Vector3 dir = (transform.position - _trans.transform.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }
}