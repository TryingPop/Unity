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
    [SerializeField] protected Vector3 initPos;   // ���� �� ������ ��ġ
    [SerializeField] protected int targetNum;       // Ÿ�� ��
    
    protected int curNum;

    [SerializeField] protected bool generateTarget;   // �����Ұ��� Ȯ��
    

    public override bool IsSuccess => curNum == targetNum;

    /// <summary>
    /// �̼� ���� -> ��ǥ ���� 0
    /// </summary>
    public override void Init()
    {

        typeNum = (int)myType;

        curNum = 0;
        if (generateTarget) SetTargets();

        ActionManager.instance.DeadUnit += ChkMission;

        if (startScripts != null) UIManager.instance.SetScripts(startScripts.Scripts);
        if (startEvent != null)
        {

            for (int i = 0; i < startEvent.Length; i++)
            {

                startEvent[i].InitalizeEvent();
            }
        }
    }

    /// <summary>
    /// Ÿ�� ���� ���� �޼���
    /// </summary>
    protected void SetTargets()
    {

        // ���� ���� �ؾ��Ѵ�!
        int prefabIdx = PoolManager.instance.ChkIdx(target.MyStat.SelectIdx);

        Vector3 pos = initPos;
        // Ÿ�� �� ��ŭ ���� ����
        for (int i = 1; i <= targetNum; i++)
        {


            Selectable genTarget = null;

            if (prefabIdx != -1)
            {


                var go = PoolManager.instance.GetPrefabs(prefabIdx, targetLayer);
                if (go) genTarget = go.GetComponent<Selectable>();
            }

            if (genTarget == null)
            {

                genTarget = Instantiate(target);
            }
            
            Command.SetNextPos(genTarget.MyStat.MySize, i, ref pos);
            genTarget.transform.position = pos;
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
                IsMissionComplete();
            }
        }
    }

    // ���� ����
    public override string GetMissionObjectText()
    {

        return string.Format("[{0}] {1} {2} {3}{4}{5}",
            missionName,
            targetLayer == VarianceManager.LAYER_PLAYER ? "�÷��̾�" :
                    targetLayer == VarianceManager.LAYER_ENEMY ? "��" :
                    targetLayer == VarianceManager.LAYER_NEUTRAL ? "�߸�" : "�Ʊ�",
            targetNum == 1 ? $"{target.MyStat.MyName}" : $"{target.MyStat.MyName}�� {targetNum}����",
            IsWin ? "óġ" : "��� �� ����",
            curNum == 0 || curNum == targetNum ? "" : $"({curNum}/{targetNum} ���)",
            IsSuccess ? IsWin ? "[�Ϸ�]" : "[����]" : "");
    }

    protected override void EndMission()
    {

        ActionManager.instance.DeadUnit -= ChkMission;
    }
}
