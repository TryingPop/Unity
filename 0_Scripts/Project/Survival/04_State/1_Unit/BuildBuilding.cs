using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� ������ �Ǽ��� ���
/// </summary>
[CreateAssetMenu(fileName = "BuildBuilding", menuName = "Action/Unit/BuildBuilding")]
public class BuildBuilding : IUnitAction
{

    [SerializeField] private LayerMask chkLayer;
    private RaycastHit[] hit = new RaycastHit[2];       // �Ǽ� ���� ������Ʈ�� �Ǻ��ϹǷ�
                                                        // �ִ� ���� �� �ִ°� �Ǽ��ڿ� �Ǽ��� �̿� obj 2���� ���� �� ������ �ȴ�

    public override void Action(Unit _unit)
    {

        if (_unit.Target == null)
        {
         
            OnExit(_unit, STATE_SELECTABLE.NONE);
            return;
        }

        if (_unit.MyAgent.remainingDistance < _unit.Target.MyStat.MySize * 0.5f)
        {

            // ���� �Ÿ� �ȿ� ������ �̵� �ʱ�ȭ
            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // ��, �α� Ȯ��
            int supply = _unit.Target.MyStat.Supply;
            int gold = _unit.Target.MyStat.Cost;

            if (_unit.MyTeam.ChkGold(gold)
                && _unit.MyTeam.ChkSupply(supply))
            {

                // �ǹ� ��ġ�� �����뵵
                int chk = Physics.BoxCastNonAlloc(_unit.TargetPos, Vector3.one * (_unit.Target.MyStat.MySize * 0.5f), Vector3.up, hit, Quaternion.identity, 2f, chkLayer);

                // �浹�ϴ� ������ ���ų�, �浹�ϴ� ������ �Ǽ��� ���� ���
                if (chk == 0
                    || (chk == 1 
                    && hit[0].transform == _unit.transform))
                {

                    // ��� �Ҹ�
                    _unit.MyTeam.AddGold(-gold);

                    var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer, _unit.TargetPos);

                    if (go)
                    {

                        go.transform.position = _unit.TargetPos;
                        var _target = go.GetComponent<Selectable>();
                        
                        _target.AfterSettingLayer();
                        _target.ChkSupply();
                        _unit.Target = _target;
                        _unit.TargetPos = _target.transform.position;
                        OnExit(_unit, STATE_SELECTABLE.UNIT_REPAIR);
                    }
                }
                else
                {

                    // �Ǽ��� �ܿ� �ٸ� �� ������ �� ���´�
                    UIManager.instance.SetWarningText("�ش� ������ �Ǽ��� �� �����ϴ�", Color.yellow, 2f);
                    OnExit(_unit, STATE_SELECTABLE.NONE);
                }
            }
            else
            {

                // �Ǽ� ���� ���� �Ϲ� ���·� Ż��
                OnExit(_unit, STATE_SELECTABLE.NONE);
            }
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // Ÿ���� ���ų� �ǹ��� �ƴ� ��� Ż��!
        if (!_unit.Target) return;

        
        // Ÿ���� ��ҷ� �̵�!
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 1f);

        _unit.StateName = stateName;
    }
}
