using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ش� Ÿ���� ���� �ı�
/// </summary>
public class TargetUnit : Mission
{

    [SerializeField] protected Selectable target;   // Ÿ�� ����
    [SerializeField] protected int targetLayer;     // Ÿ���� �� ����
    [SerializeField] protected Vector3[] initPos;   // ���� �� ������ ��ġ
    [SerializeField] protected int targetNum;       // Ÿ�� ��
    
    protected int curNum;

    [SerializeField] protected bool isSetTargets;   // �����Ұ��� Ȯ��
    

    public override bool IsSuccess => curNum == targetNum;





    /// <summary>
    /// �̼� ���� -> ��ǥ ���� 0
    /// </summary>
    public override void Init()
    {

        curNum = 0;
        if (isSetTargets) SetTargets();

        ActionManager.instance.DeadUnit += Chk;
    }

    /// <summary>
    /// Ÿ�� ���� ���� �޼���
    /// </summary>
    protected void SetTargets()
    {

        // ���� ���� �ؾ��Ѵ�!
        int prefabIdx = PoolManager.instance.ChkIdx(target.MyStat.SelectIdx);

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
        }
    }

    /// <summary>
    /// Ư�� ������ �׾����� �Ǻ�
    /// </summary>
    public override void Chk(Selectable _target)
    {

        if (_target.MyStat.SelectIdx == target.MyStat.SelectIdx
            && _target.MyTeam.TeamLayerNumber == targetLayer)
        {

            curNum++;
            if (curNum == targetNum) 
            { 
                
                GameManager.instance.ChkMissions();
                EndMission();
            }
        }
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadUnit -= Chk;
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
