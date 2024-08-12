using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���׷��̵� �Ǹ��� �뵵
/// </summary>
[CreateAssetMenu(fileName ="RandomUpgrade", menuName = "Action/Building/RandomUpgrade")]
public class RandomUpgrade : BuildingAction
{

    [SerializeField] protected TYPE_MANAGEMENT[] types;
    [SerializeField] protected short[] amounts;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            int rand = Random.Range(0, types.Length);

            var alliance = _building.MyTeam;
            _building.MyTeam.UpgradeUnit(types[rand]);
            OnExit(_building);
        }
    }
}
