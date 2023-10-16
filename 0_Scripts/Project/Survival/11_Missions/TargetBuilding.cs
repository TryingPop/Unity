using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetBuilding", menuName = "Mission/TargetBuilding")]
public class TargetBuilding : Mission 
{ 

    [SerializeField] protected Building[] objBuildings;
    [SerializeField] protected Vector3[] initPos;
    [SerializeField] protected int[] targetLayers;

    protected List<Building> targets;

    public override bool IsSucess => targets.Count == 0;

    /// <summary>
    /// 타겟 설정
    /// </summary>
    public override void Init(GameManager _gameManager)
    {

        // 유닛 생성 해야한다!
        if (targets == null) targets = new List<Building>(objBuildings.Length);

        for (int i = 0; i < objBuildings.Length; i++)
        {

            ushort selectIdx = objBuildings[i].MyStat.SelectIdx;
            short prefabIdx = PoolManager.instance.ChkIdx(selectIdx);

            Building building = null;

            if (prefabIdx != -1)
            {

                var go = PoolManager.instance.GetPrefabs(prefabIdx, targetLayers[i]);
                if (go) 
                {

                    building = go.GetComponent<Building>(); 
                }
            }

            if (building == null)
            {

                building = GameObject.Instantiate(objBuildings[i]);
                building.gameObject.layer = targetLayers[i];
            }

            building.transform.position = initPos[i];
            building.AfterSettingLayer();

            targets.Add(building);
        }
    }

    /// <summary>
    /// 특정 유닛이 죽었는지 판별
    /// </summary>
    public override void Chk(Unit _unit, Building _building)
    {

        if (_building == null) return;

        for (int i = 0; i < objBuildings.Length; i++)
        {

            if (targets[i] == _building)
            {

                targets.RemoveAt(i);
                return;
            }
        }
    }
}
