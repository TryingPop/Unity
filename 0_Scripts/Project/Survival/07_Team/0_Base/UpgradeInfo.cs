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
    public int lvlUnitAtk;              // �߰� ���ݷ�
    public int lvlUnitDef;              // �߰� ����
    public int lvlUnitHp;               // �߰� ü��

    public int lvlUnitEvade;            // ȸ�� ����

    public int lvlBuildingDef;          // �ǹ� ����
    public int lvlBuildingHp;           // �ǹ�ü��

    // �ܺο��� �� �߰�
    public int lvlGetTurnGold;          // ȹ�� ��� �߰�
    public int lvlMaxSupply;            // �߰� �α�

    public int addTurnGold;
    public int addMaxSupply;
}
