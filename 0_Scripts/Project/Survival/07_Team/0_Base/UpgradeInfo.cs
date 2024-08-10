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
    public LvlData unitAtk;             // ���� ���ݷ�
    public LvlData unitDef;             // ���� ����
    public LvlData unitHp;              // ���� ü��

    public LvlData buildingDef;
    public LvlData buildingHp;


    public int lvlUnitEvade;            // ȸ�� ����

    // �ܺο��� �� �߰�
    public int lvlGetTurnGold;          // ȹ�� ��� �߰�
    public int lvlMaxSupply;            // �߰� �α�

    public int addTurnGold;
    public int addMaxSupply;
}
