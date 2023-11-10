using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        ActionManager.instance.DeadUnit += ChkMission;
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
    public override void ChkMission(Selectable _target)
    {

        if (_target.MyStat.SelectIdx == target.MyStat.SelectIdx
            && _target.MyTeam.TeamLayerNumber == targetLayer)
        {

            curNum++;
            if (curNum == targetNum) 
            { 
                
                EndMission();
            }
        }
    }

    // ���� ����
    public override string GetMissionObjectText()
    {

        return string.Format("{0} {1} {2}{3}{4}",
            targetLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "��" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "�߸�" : "�Ʊ�",
            targetNum == 1 ? $"{target.MyStat.MyName}" : $"{target.MyStat.MyName}�� {targetNum}����",
            isWin ? "óġ" : "��� �� ����",
            curNum == 0 || curNum == targetNum ? "" : $"({curNum}/{targetNum} ���)",
            IsSuccess ? isWin ? "[�Ϸ�]" : "[����]" : "");
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadUnit -= ChkMission;
        IsMissionComplete();
    }
}
