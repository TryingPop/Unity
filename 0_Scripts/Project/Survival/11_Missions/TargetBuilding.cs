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
    /// Ÿ�� ����
    /// </summary>
    public override void Init(GameManager _gameManager)
    {

        // ���� ���� �ؾ��Ѵ�!
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
    /// Ư�� ������ �׾����� �Ǻ�
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
