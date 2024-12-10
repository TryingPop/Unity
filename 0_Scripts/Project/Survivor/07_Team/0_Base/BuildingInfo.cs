using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� ���� Ȯ��
/// ���忡 ���ֿ��� ������ ����!
/// 
/// Dictionary�� �ϸ� ������ ���� �����ؼ�
/// int�迭 2���� �Ѵ�
/// </summary>
[System.Serializable]
public class BuildingInfo
{

    [SerializeField] private LimitObject[] buildings;
    private Dictionary<MY_TYPE.GAMEOBJECT, LimitData> dic;

    public void Init()
    {

        dic = new();
        for (int i = 0; i < buildings.Length; i++)
        {

            buildings[i].Init();
            dic[buildings[i].MyType] = buildings[i];
        }
    }

    public bool Contains(MY_TYPE.GAMEOBJECT _type) => dic.ContainsKey(_type);

    /// <summary>
    /// �ǹ� �߰�
    /// </summary>
    public void AddBuilding(MY_TYPE.GAMEOBJECT _type, bool _add)
    {

        if (_add) dic[_type].AddVal(1);
        else dic[_type].RemoveVal(1);
    }

    /// <summary>
    /// ���� �ø���
    /// </summary>
    public void AddMaxBuilding(MY_TYPE.GAMEOBJECT _type, int _add = 1)
    {

        if (dic.ContainsKey(_type)) dic[_type].AddMax(_add);
    }

    public bool ChkAdd(MY_TYPE.GAMEOBJECT _type)
    {

        return dic[_type].ChkAdd();
    }
}