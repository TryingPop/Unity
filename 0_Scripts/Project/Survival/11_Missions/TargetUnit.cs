using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �ı�
/// </summary>
[CreateAssetMenu(fileName = "TargetUnit", menuName = "Mission/TargetUnit")]
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;   // Ÿ�� ����
    [SerializeField] protected int targetLayer;     // Ÿ���� �� ����
    [SerializeField] protected Vector3[] initPos;   // ���� �� ������ ��ġ
    [SerializeField] protected int targetNum;       // Ÿ�� ��
    protected int curNum;


    public override bool IsSucess => curNum >= targetNum;

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    public override void Init(GameManager _gameManager)
    {
        
        // ���� ���� �ؾ��Ѵ�!
        int selectIdx = target.MyStat.SelectIdx;
        int prefabIdx = PoolManager.instance.ChkIdx(selectIdx);

        curNum = 0;

        // Ÿ�� �� ��ŭ ���� ����
        for (int i = 0; i < targetNum; i++)
        {


            Selectable genTarget = null;

            if (prefabIdx != -1)
            {

                var go = PoolManager.instance.GetPrefabs(prefabIdx, targetLayer);
                if (go)
                {

                    genTarget = go.GetComponent<Selectable>();
                }
            }

            if (genTarget == null)
            {

                genTarget = GameObject.Instantiate(target);
                genTarget.gameObject.layer = targetLayer;
            }

            genTarget.transform.position = initPos[i];
            genTarget.AfterSettingLayer();
            genTarget.ChkSupply(false);
        }
    }

    /// <summary>
    /// Ư�� ������ �׾����� �Ǻ�
    /// </summary>
    public override void Chk(Unit _unit, Building _building)
    {

        if (_unit == null) return;

        if (_unit.MyStat.SelectIdx == target.MyStat.SelectIdx
            && TeamManager.instance.CompareTeam(_unit.MyTeam, targetLayer))
        {

            curNum++;
        }
    }

    // ���� ����
    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin)
        {

            if (targetNum == 1) return $"{target.MyStat.MyName} óġ";
            
            return $"{target.MyStat.MyName} : {curNum} / {targetNum} óġ";
        }

        if (targetNum == 1) return $"{target.MyStat.MyName} ���";

        return $"{target.MyStat.MyName} : {curNum} / {targetNum} ���";
    }
}
