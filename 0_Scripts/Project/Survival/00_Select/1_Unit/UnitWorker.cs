using System.Collections;using System.Collections.Generic;
using UnityEngine;

public class UnitWorker : Unit
{

    /// <summary>
    /// 데미지 반응
    /// </summary>
    /// <param name="_trans"></param>
    protected override void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        // 공격을 못하면 반대 방향으로 도주!
        Vector3 dir = (transform.position - _trans.transform.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }

    /// <summary>
    /// 공격이 수리므로 따로 설정
    /// </summary>
    /// <param name="_cmd"></param>
    protected override void ReadCommand(Command _cmd)
    {

        STATE_SELECTABLE type = _cmd.type;

        // 마우스 R인 경우 명령을 바꾼다!
        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // 마우스 R버튼을 누른 경우 이동이나 공격 타입으로 바꾼다
            if (myAttack == null)
            {

                // 공격할 수 없는 경우
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target == null)
            {

                // 대상이 없는 경우
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if ((myAlliance.GetLayer(true) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // 대상이 팀인 경우 힐 or 수리? 하러 간다!
                type = STATE_SELECTABLE.UNIT_REPAIR;
            }
            else
            {

                // 대상이 수리 대상도 아니고, 건설 대상도 아니므로 그냥 따라다닌다
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
        }

        myState = type;
        target = _cmd.target != transform ? _cmd.target : null;
        targetPos = _cmd.pos;
        _cmd.Received(myStat.MySize);
    }
}