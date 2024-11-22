using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportUnit : Unit
{

    /// <summary>
    /// 유닛 Type과 hp, 방어력 출력
    /// </summary>
    /// <param name="_txt"></param>
    public override void SetInfo(Text _txt)
    {

        int temp = myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP);

        string strHp = MaxHp == VarianceManager.INFINITE ? "Infinity"
            : temp == 0 ? $"{curHp} / {MaxHp}" : $"{curHp} / {MaxHp}(+{temp})";

        temp = myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF);
        string strDef = temp == 0 ? myStat.Def.ToString() : $"{myStat.Def}(+{temp})";
        _txt.text = $"체력 : {strHp}\n방어력 : {strDef}\n{myStateAction.GetStateName(myState)} 중";
    }

    /// <summary>
    /// 비타격 유닛은 이동이 
    /// </summary>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        STATE_SELECTABLE type = _cmd.type;

        // 마우스 R인 경우 명령을 바꾼다!
        if (type == STATE_SELECTABLE.MOUSE_R)
            type = STATE_SELECTABLE.UNIT_MOVE;

        myState = type;
        target = _cmd.target != transform ? _cmd.target : null;
        targetPos = _cmd.pos;
    }

    /// <summary>
    /// 비공격 유닛의 피격시 반응
    /// </summary>
    protected override void OnDamagedAction(Transform _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        // 공격을 못하면 반대 방향으로 도주!
        Vector3 dir = (transform.position - _trans.position).normalized;
        targetPos = transform.position + dir * myAgent.speed;
        ActionDone(STATE_SELECTABLE.UNIT_MOVE);
    }
}