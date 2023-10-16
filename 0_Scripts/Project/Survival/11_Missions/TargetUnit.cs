using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetUnit", menuName = "Mission/TargetUnit")]
public class TargetUnit : Mission
{

    [SerializeField] protected Unit[] objUnits;
    [SerializeField] protected int[] targetLayers;
    [SerializeField] protected Vector3[] initPos;
    protected List<Unit> targets;

    public override bool IsSucess => targets.Count == 0;

    /// <summary>
    /// 타겟 설정
    /// </summary>
    public override void Init(GameManager _gameManager)
    {
        
        // 유닛 생성 해야한다!
        if (targets == null) targets = new List<Unit>(objUnits.Length);

        for (int i = 0; i < objUnits.Length; i++)
        {

            ushort selectIdx = objUnits[i].MyStat.SelectIdx;
            short prefabIdx = PoolManager.instance.ChkIdx(selectIdx);

            Unit building = null;

            if (prefabIdx != -1)
            {

                var go = PoolManager.instance.GetPrefabs(prefabIdx, targetLayers[i]);
                if (go)
                {

                    building = go.GetComponent<Unit>();
                }
            }

            if (building == null)
            {

                building = GameObject.Instantiate(objUnits[i]);
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

        if (_unit == null) return;

        for (int i = 0; i < targets.Count; i++)
        {

            if (targets[i] == _unit)
            {

                targets.RemoveAt(i);
                return;
            }
        }

    }
}
