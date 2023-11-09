using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �ı�
/// </summary>
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;       // Ÿ�� ����
    [SerializeField] protected int targetLayer;     // Ÿ���� �� ����
    [SerializeField] protected Vector3[] initPos;   // ���� �� ������ ��ġ
    [SerializeField] protected int targetNum;       // Ÿ�� ��

    protected List<Selectable> targets;

    public override bool IsSucess => targets.Count == 0;


    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    public override void Init(GameManager _gameManager)
    {
        
        // ���� ���� �ؾ��Ѵ�!
        int prefabIdx = PoolManager.instance.ChkIdx(target.MyStat.SelectIdx);

        if (targets == null) targets = new List<Selectable>(targetNum);

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

                    // ������ �ʰ��� ��� ���� ��
                }
            }

            if (genTarget == null)
            {

                genTarget = Instantiate(target);
            }

            int rand = i >= initPos.Length ? Random.Range(0, initPos.Length) : i;
            genTarget.transform.position = initPos[rand];
            genTarget.AfterSettingLayer();
            genTarget.ChkSupply(false);
            targets.Add(genTarget);
        }
    }

    /// <summary>
    /// Ư�� ������ �׾����� �Ǻ�
    /// </summary>
    public override void Chk(Unit _unit, Building _building)
    {

        if (_unit == null) return;

        if (targets.Contains(_unit))
        {

            targets.Remove(_unit);
        }
    }

    // ���� ����
    public override string GetMissionObjectText(bool _isWin)
    {

        if (_isWin)
        {

            if (targetNum == 1) return $"{target.MyStat.MyName} óġ";
            return $"{target.MyStat.MyName} : {targetNum - targets.Count} / {targetNum} óġ";
        }

        if (targetNum == 1) return $"{target.MyStat.MyName} ���";

        return $"{target.MyStat.MyName} : {targetNum - targets.Count} / {targetNum} ���";
    }
}
