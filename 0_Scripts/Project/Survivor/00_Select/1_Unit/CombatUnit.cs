using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUnit : Unit
{

    [SerializeField] protected Attack myAttack;                     // ���� ��� �� ���� ����

    public override Attack MyAttack => myAttack;

#if UNITY_EDITOR

    protected override void Init()
    {

        if (myAttack is null) Debug.LogError($"{myStat.MyType}�� ������ �����ϴ�!");

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

        _txt.text = $"ü�� : {strHp}\n���ݷ� : {strAtk}   ���� : {strDef}\n{myStateAction.GetStateName(myState)} ��";
    }
    /// <summary>
    /// �ǰ� �� �׼�
    /// </summary>
    protected override void OnDamagedAction(Transform _trans)
    {

        if (_trans == null || !ChkDmgReaction()) return;

        BaseObj atker = _trans.GetComponent<BaseObj>();
        if (atker == null) return;
        if (((1 << atker.gameObject.layer) & myTeam.AllyLayer) == 0)
        {

            // ������ �� ���ų� ������ ����� �Ʊ��� ��� �ݴ� �������� ����
            Vector3 dir = (transform.position - _trans.position).normalized;
            targetPos = transform.position + dir * myAgent.speed;
            ActionDone(STATE_SELECTABLE.UNIT_MOVE);
        }
        else
        {

            // ������ �� �ְ� ���� ������ ��� �¹޾�ģ��!
            target = atker;
            targetPos = _trans.position;
            ActionDone(STATE_SELECTABLE.UNIT_ATTACK);
        }
    }

    /// <summary>
    /// ���� �����ϸ� �����Ѵ�.
    /// </summary>
    protected override void ReadCommand(Command _cmd)
    {

        if (_cmd.ChkUsedCommand(myStat.MySize)) return;

        STATE_SELECTABLE type = _cmd.type;

        if (type == STATE_SELECTABLE.MOUSE_R)
        {

            // ���콺 R��ư�� ���� ��� �̵��̳� ���� Ÿ������ �ٲ۴�
            if (_cmd.target == null)
                // ����� ���ų� ������ ���� ���
                type = STATE_SELECTABLE.UNIT_MOVE;
            else if ((myTeam.EnemyLayer & (1 << _cmd.target.gameObject.layer)) != 0)
                // ����� ���� �ؾ��� ����̸� ����
                type = STATE_SELECTABLE.UNIT_ATTACK;
            else
                // ���� ����� �ƴϸ� ���󰣴�
                type = STATE_SELECTABLE.UNIT_MOVE;
        }

        myState = type;
        // �ڱ� �ڽ��� ����� �� �� ����!
        target = _cmd.target != this ? _cmd.target : null;
        targetPos = _cmd.pos;
    }
}
