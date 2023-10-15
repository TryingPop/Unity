using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Build", menuName = "Button/Sub/Build")]
public class BtnSubBuild : BtnSub
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
        var building = _inputManager.buildManager.GetPrepareBuilding(prepareIdx);
        var go = building.Build();
        if (go)
        {

            var target = go.GetComponent<Building>();
            
            _inputManager.GiveCmd(target.transform.position, target);

            _inputManager.buildManager.GetPrepareBuilding(prepareIdx).Used(target);
            _inputManager.ActiveButtonUI(true, false, false);
            _inputManager.ActionDone();
        }
    }

    public override void OnExit(InputManager _inputManager, TYPE_KEY _nextKey = TYPE_KEY.NONE)
    {

        // OnEnter에서 걸러지기에 null 체크 안한다!
        _inputManager.buildManager.GetPrepareBuilding(prepareIdx).Used(null);
        base.OnExit(_inputManager, _nextKey);
    }
}
