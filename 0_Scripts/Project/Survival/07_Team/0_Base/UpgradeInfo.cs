using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���׷��̵� ����
/// ���׷��̵� �ܰ踦 �����ϴ� Ŭ����
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    // ���׷��̵� ����
    [Header("����")]
    public UpgradeData unitAtk;
    public UpgradeData unitDef;
    public UpgradeData unitHp;

    public UpgradeData unitEvade;

    [Header("�ǹ�")]
    public UpgradeData buildingDef;
    public UpgradeData buildingHp;

    [Header("�ڿ�")]
    
    [Tooltip("�ϴ� �߰� �ڿ� ȹ�淮, 1�� : 10��")]
    public UpgradeResourceData turnGold;    // ȹ�� ��� �߰�
    [Tooltip("�ִ� �α�")]
    public UpgradeResourceData maxSupply;   // �߰� �α�

    public void Init()
    {

        unitAtk.Init();
        unitDef.Init();
        unitHp.Init();

        unitEvade.Init();

        buildingDef.Init();
        buildingHp.Init();

        turnGold.Init();
        maxSupply.Init();
    }
}
