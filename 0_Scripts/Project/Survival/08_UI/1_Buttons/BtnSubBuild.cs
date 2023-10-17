using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Build", menuName = "Button/Sub/Build")]
public class BtnSubBuild : ButtonInfo
{

    [SerializeField] protected ushort selectIdx;
    protected short prepareIdx = -1;

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

    public override void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        // OnEnter에서 걸러지기에 null 체크 안한다!
        _inputManager.buildManager.UsedPrepareBuilding();
        base.OnExit(_inputManager, _nextKey);
    }
}
