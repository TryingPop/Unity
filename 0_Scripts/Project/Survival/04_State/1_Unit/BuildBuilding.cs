using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 생성과 건설을 담당
/// </summary>
[CreateAssetMenu(fileName = "BuildBuilding", menuName = "Action/Unit/BuildBuilding")]
public class BuildBuilding : IUnitAction
{

    [SerializeField] private LayerMask chkLayer;
    private RaycastHit[] hit = new RaycastHit[2];       // 건설 전에 오브젝트를 판별하므로
                                                        // 최대 담을 수 있는게 건설자와 건설자 이외 obj 2개만 담을 수 있으면 된다

    public override void Action(Unit _unit)
    {

        if (_unit.Target == null)
        {
         
            OnExit(_unit, STATE_SELECTABLE.NONE);
            return;
        }

        if (_unit.MyAgent.remainingDistance < _unit.Target.MyStat.MySize * 0.5f)
        {

            // 일정 거리 안에 들어오면 이동 초기화
            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // 돈, 인구 확인
            int supply = _unit.Target.MyStat.Supply;
            int gold = _unit.Target.MyStat.Cost;

            if (_unit.MyTeam.ChkGold(gold)
                && _unit.MyTeam.ChkSupply(supply))
            {

                // 건물 겹치기 방지용도
                int chk = Physics.BoxCastNonAlloc(_unit.TargetPos, Vector3.one * (_unit.Target.MyStat.MySize * 0.5f), Vector3.up, hit, Quaternion.identity, 2f, chkLayer);

                // 충돌하는 유닛이 없거나, 충돌하는 유닛이 건설자 뿐일 경우
                if (chk == 0
                    || (chk == 1 
                    && hit[0].transform == _unit.transform))
                {

                    // 골드 소모
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

                    // 건설자 외에 다른 게 있으면 못 짓는다
                    UIManager.instance.SetWarningText("해당 지점에 건설할 수 없습니다", Color.yellow, 2f);
                    OnExit(_unit, STATE_SELECTABLE.NONE);
                }
            }
            else
            {

                // 건설 못한 경우는 일반 상태로 탈출
                OnExit(_unit, STATE_SELECTABLE.NONE);
            }
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // 타겟이 없거나 건물이 아닌 경우 탈출!
        if (!_unit.Target) return;

        
        // 타겟의 장소로 이동!
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 1f);

        _unit.StateName = stateName;
    }
}
