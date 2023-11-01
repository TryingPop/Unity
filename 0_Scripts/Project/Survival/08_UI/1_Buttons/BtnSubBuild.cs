using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 건설 버튼 건물과 매칭 시켜줘야한다
/// </summary>
[CreateAssetMenu(fileName = "Build", menuName = "Button/Sub/Build")]
public class BtnSubBuild : ButtonInfo
{

    [SerializeField] protected ushort selectIdx;
    [SerializeField] protected short prepareIdx = -1;

    public override void OnEnter(InputManager _inputManager)
    {

        BuildManager buildManager = _inputManager.buildManager;

        if (prepareIdx == -1)
        {

            prepareIdx = buildManager.ChkIdx(selectIdx);
        }

        var building = buildManager.GetPrepareBuilding(prepareIdx);
        
        if (building)
        {

            building.Init();
        }
        else
        {

            OnExit(_inputManager);
        }
    }

    public override void Action(InputManager _inputManager)
    {

        // OnEnter에서 걸러지기에 null 체크 안한다
        var prebuilding = _inputManager.buildManager.GetPrepareBuilding(prepareIdx);
        var building = prebuilding.Build();
        if (building)
        {

            _inputManager.GiveCmd(building.transform.position, building);
            _inputManager.buildManager.UsedPrepareBuilding();
            _inputManager.ActionDone();
        }
    }

    public override void OnExit(InputManager _inputManager, TYPE_INPUT _nextKey = TYPE_INPUT.NONE)
    {

        // OnEnter에서 걸러지기에 null 체크 안한다!
        _inputManager.buildManager.UsedPrepareBuilding();
        base.OnExit(_inputManager, _nextKey);
    }
}
