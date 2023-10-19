using System.Collections;using System.Collections.Generic;
using UnityEngine;

public class UnitWorker : Unit
{

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <param name="_trans"></param>
    protected override void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        // ������ ���ϸ� �ݴ� �������� ����!
        Vector3 dir = (transform.position - _trans.transform.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }

    /// <summary>
    /// ������ �����Ƿ� ���� ����
    /// </summary>
    /// <param name="_cmd"></param>
    protected override void ReadCommand(Command _cmd)
    {

        STATE_SELECTABLE type = _cmd.type;

        // ���콺 R�� ��� ����� �ٲ۴�!
        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // ���콺 R��ư�� ���� ��� �̵��̳� ���� Ÿ������ �ٲ۴�
            if (myAttack == null)
            {

                // ������ �� ���� ���
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target == null)
            {

                // ����� ���� ���
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if ((myAlliance.GetLayer(true) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // ����� ���� ��� �� or ����? �Ϸ� ����!
                type = STATE_SELECTABLE.UNIT_REPAIR;
            }
            else
            {

                // ����� ���� ��� �ƴϰ�, �Ǽ� ��� �ƴϹǷ� �׳� ����ٴѴ�
                type = STATE_SELECTABLE.UNIT_MOVE;
            }
        }

        myState = type;
        target = _cmd.target != transform ? _cmd.target : null;
        targetPos = _cmd.pos;
        _cmd.Received(myStat.MySize);
    }
}