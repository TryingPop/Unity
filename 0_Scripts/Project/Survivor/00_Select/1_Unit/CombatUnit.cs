using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnit : Unit
{

    [SerializeField] protected Attack myAttack;                     // 공격 방법 및 공격 정보

    public override Attack MyAttack => myAttack;

#if UNITY_EDITOR

    protected override void Init()
    {

        if (myAttack is null) Debug.LogError($"{myStat.MyType}에 공격이 없습니다!");

        base.Init();
    }
#endif

    public override void SetInfo(Text _txt)
    {

        int temp = myTeam == null ? 0 : myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_HP);
        int maxHp = myStat.GetMaxHp(temp);
        string strHp = maxHp == VarianceManager.INFINITE ? "Infinity"
            : temp == 0 ? $"{curHp} / {maxHp}" : $"{curHp} / {maxHp}(+{temp})";

        temp = myTeam == null ? 0 : myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_ATK);
        string strAtk = temp == 0 ? myAttack.GetAtk(0).ToString() : $"{myAttack.GetAtk(temp)}(+{temp})";

        temp = myTeam == null ? 0 : myTeam.GetLvl(TYPE_SELECTABLE.UP_UNIT_DEF);
        string strDef = temp == 0 ? myStat.GetDef(0).ToString() : $"{myStat.GetDef(temp)}(+{temp})";

        _txt.text = $"체력 : {strHp}\n공격력 : {strAtk}   방어력 : {strDef}\n{myStateAction.GetStateName(myState)} 중";
    }
    /// <summary>
    /// 피격 후 액션
    /// </summary>
    protected override void OnDamagedAction(Transform _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        BaseObj atker = _trans.GetComponent<BaseObj>();
        if (atker == null) return;
        if (((1 << atker.gameObject.layer) & myTeam.AllyLayer) == 0)
        {

            // 공격할 수 없거나 공격한 대상이 아군일 경우 반대 방향으로 도주
            Vector3 dir = (transform.position - _trans.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_SELECTABLE.UNIT_MOVE);
        }
        else
        {

            // 공격할 수 있고 적이 때렸을 경우 맞받아친다!
            target = atker;
            targetPos = _trans.position;
            ActionDone(STATE_SELECTABLE.UNIT_ATTACK);
        }
    }

    /// <summary>
    /// 공격 가능하면 공격한다.
    /// </summary>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        STATE_SELECTABLE type = _cmd.type;

        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // 마우스 R버튼을 누른 경우 이동이나 공격 타입으로 바꾼다
            if (_cmd.target == null)
                // 대상이 없거나 공격이 없을 경우
                type = STATE_SELECTABLE.UNIT_MOVE;
            else if ((myTeam.EnemyLayer & (1 << _cmd.target.gameObject.layer)) != 0)
                // 대상이 공격 해야할 대상이면 공격
                type = STATE_SELECTABLE.UNIT_ATTACK;
            else
                // 공격 대상이 아니면 따라간다
                type = STATE_SELECTABLE.UNIT_MOVE;
        }

        myState = type;
        // 자기 자신은 대상이 될 수 없다!
        target = _cmd.target != this ? _cmd.target : null;
        targetPos = _cmd.pos;
    }
}
