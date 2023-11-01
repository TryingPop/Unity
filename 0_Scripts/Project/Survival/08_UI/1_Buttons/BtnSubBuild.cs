using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� �Ǽ� ��ư �ǹ��� ��Ī ��������Ѵ�
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

        // OnEnter���� �ɷ����⿡ null üũ ���Ѵ�
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

        // OnEnter���� �ɷ����⿡ null üũ ���Ѵ�!
        _inputManager.buildManager.UsedPrepareBuilding();
        base.OnExit(_inputManager, _nextKey);
    }
}
