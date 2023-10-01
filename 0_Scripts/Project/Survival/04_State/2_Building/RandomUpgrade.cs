using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RandomUpgrade", menuName = "Action/Building/RandomUpgrade")]
public class RandomUpgrade : BuildingAction
{

    [SerializeField] protected TYPE_UPGRADE[] types;
    [SerializeField] protected short[] amounts;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            int rand = Random.Range(0, types.Length);

            var alliance = _building.MyAlliance;
            _building.MyUpgrades.UpgradeStat(types[rand], amounts[rand]);
            ActionManager.instance.UpgradeChk(alliance);
            OnExit(_building);
        }
    }
}
