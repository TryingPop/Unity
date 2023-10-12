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

        // ���콺 R�� ��� ����� �ٲ۴�!
        if (idx == VariableManager.MOUSE_R)
        {

            // ���콺 R��ư�� ���� ��� �̵��̳� ���� Ÿ������ �ٲ۴�
            if (myAttack == null)
            {

                // ������ �� ���� ���
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target == null)
            {

                // ����� ���� ���
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
            }
            else if (_cmd.target.MyStat.MyType == TYPE_SELECTABLE.BUILDING
                && (myAlliance.GetLayer(true) & (1 << _cmd.target.gameObject.layer)) != 0)
            {

                // ����� �ǹ��� ��� �����Ϸ� ����!
                _cmd.type = STATE_SELECTABLE.UNIT_ATTACK;
            }
            else
            {

                // ����� ���� ��� �ƴϰ�, �Ǽ� ��� �ƴϹǷ� �׳� ����ٴѴ�
                _cmd.type = STATE_SELECTABLE.UNIT_MOVE;
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
        else if (myState == STATE_SELECTABLE.NONE)
        {

            stateChange = true;
        }

        // ��� ���, ���� ����� ��� �ִ� �� Ȯ�� �Ѵ�
        if (cmds.Count < VariableManager.MAX_RESERVE_COMMANDS) cmds.Enqueue(_cmd);
        else Debug.Log($"{gameObject.name}�� ��ɾ ���� á���ϴ�.");
    }


    protected override void OnDamageAction(Selectable _trans)
    {

        if (_trans == null || !atkReaction) return;

        // ������ ���ϸ� �ݴ� �������� ����!
        Vector3 dir = (transform.position - _trans.transform.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }
}