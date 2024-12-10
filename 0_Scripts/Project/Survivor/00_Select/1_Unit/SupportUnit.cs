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

        int temp = 0;
        if (myTeam != null) myTeam.GetLvl(MY_TYPE.UPGRADE.UNIT_HP);

        int maxHp = myStat.GetMaxHp(temp);
        string strHp = maxHp == VarianceManager.INFINITE ? "Infinity"
            : temp == 0 ? $"{curHp} / {maxHp}" : $"{curHp} / {maxHp}(+{temp})";

        if (myTeam != null) temp = myTeam.GetLvl(MY_TYPE.UPGRADE.UNIT_DEF);
        else temp = 0;

        string strDef = temp == 0 ? myStat.Def.ToString() : $"{myStat.GetDef(temp)}(+{temp})";
        _txt.text = $"ü�� : {strHp}\n���� : {strDef}\n{myStateAction.GetStateName(myState)} ��";
    }

    /// <summary>
    /// ��Ÿ�� ������ �̵��� 
    /// </summary>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        MY_STATE.GAMEOBJECT type = _cmd.type;

        // ���콺 R�� ��� ����� �ٲ۴�!
        if (type == MY_STATE.GAMEOBJECT.MOUSE_R)
            type = MY_STATE.GAMEOBJECT.UNIT_MOVE;

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
        ActionDone(MY_STATE.GAMEOBJECT.UNIT_MOVE);
    }
}