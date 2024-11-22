using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportUnit : Unit
{

    /// <summary>
    /// ���� Type�� hp, ���� ���
    /// </summary>
    /// <param name="_txt"></param>
    public override void SetInfo(Text _txt)
    {

        int temp = myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP);

        string strHp = MaxHp == VarianceManager.INFINITE ? "Infinity"
            : temp == 0 ? $"{curHp} / {MaxHp}" : $"{curHp} / {MaxHp}(+{temp})";

        temp = myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF);
        string strDef = temp == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{temp})";
        _txt.text = $"ü�� : {strHp}\n���� : {strDef}\n{myStateAction.GetStateName(myState)} ��";
    }

    /// <summary>
    /// ��Ÿ�� ������ �̵��� 
    /// </summary>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        STATE_SELECTABLE type = _cmd.type;

        // ���콺 R�� ��� ����� �ٲ۴�!
        if (type == STATE_SELECTABLE.MOUSE_R)
            type = STATE_SELECTABLE.UNIT_MOVE;

        myState = type;
        target = _cmd.target != transform ? _cmd.target : null;
        targetPos = _cmd.pos;
    }

    /// <summary>
    /// ����� ������ �ǰݽ� ����
    /// </summary>
    protected override void OnDamagedAction(Transform _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        // ������ ���ϸ� �ݴ� �������� ����!
        Vector3 dir = (transform.position - _trans.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }
}