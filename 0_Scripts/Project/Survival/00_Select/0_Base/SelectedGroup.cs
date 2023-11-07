using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> curSelected;                          // ���� ���õ� �׷�

    // ctrl + 1 ~ 3 
    private List<List<Selectable>> saved;

    public bool isOnlySelected = false;                         // ȥ�� ������ �� �ִ� ����?
    private TYPE_SELECTABLE groupType;                          // �׷� Ÿ�� > ��ư ���� �޾ƿ���!

    public bool IsEmpty { get { return curSelected.Count == 0 ? true : false; } }  // ������� Ȯ��

    public TYPE_SELECTABLE GroupType => groupType;              // �ܺδ� �б� ����

    private bool IsBuildingType
    {

        get
        {

            int typeNum = GetBaseTypeNum(groupType);

            return typeNum == (int)TYPE_SELECTABLE.BUILDING
                || typeNum == (int)TYPE_SELECTABLE.UNFINISHED_BUILDING;
        }
    }

    /// <summary>
    /// ��� ������ �Ǽ��ȵ� �ǹ����� üũ
    /// </summary>
    private bool IsUnfinishedbuildType
    {

        get
        {

            for (int i = 0; i < curSelected.Count; i++)
            {

                if (curSelected[i].MyState != (int)STATE_SELECTABLE.BUILDING_UNFINISHED)
                {

                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// ��� ��ư �ʿ����� Ȯ��
    /// </summary>
    public bool IsCancelBtn
    {

        get
        {

            for (int i = 0; i < curSelected.Count; i++)
            {

                if (curSelected[i].IsCancelBtn)
                {

                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public SelectedGroup()
    {

        curSelected = new List<Selectable>(VarianceManager.MAX_SELECT);

        saved = new List<List<Selectable>>(3) { new List<Selectable>(VarianceManager.MAX_SELECT),
                                                new List<Selectable>(VarianceManager.MAX_SELECT),
                                                new List<Selectable>(VarianceManager.MAX_SELECT)};
    }

    /// <summary>
    /// ���� �׷� �ʱ�ȭ
    /// </summary>
    public void Clear()
    {

        isOnlySelected = false;
        curSelected.Clear();
    }

    /// <summary>
    /// ���� �߰��ϴ� �޼���
    /// </summary>
    public void Select(Selectable _select)
    {

        if (_select == null                                     // ���� ����� ���ų�
            || curSelected.Count >= VarianceManager.MAX_SELECT     // ���ð����� ������ �Ѱų�
            || curSelected.Contains(_select)                       // �̹� ���ԵǾ��� �ִ� ���ų�
            ) return;


        curSelected.Add(_select);
        
        // �׷� Ÿ�� ����
        if (curSelected.Count == 1)
        {

            isOnlySelected = _select.IsOnlySelected;
        }
    }

    /// <summary>
    /// �ܺο��� Ÿ�� üũ!
    /// </summary>
    public void ChkGroupType()
    {

        if (curSelected.Count == 0)
        {

            groupType = TYPE_SELECTABLE.NONE;
            return;
        }
        
        groupType = curSelected[0].MyStat.MyType;

        for (int i = 1; i < curSelected.Count; i++)
        {


            TYPE_SELECTABLE type = curSelected[i].MyStat.MyType;
            
            int groupNum = GetBaseTypeNum(groupType);
            int typeNum = GetBaseTypeNum(type);

            if (groupType == type) continue;

            // �׷� Ÿ���� ������ Ȯ��
            if (groupNum == typeNum) groupType = (TYPE_SELECTABLE)groupNum;
                
            // ���� ���ְ� ������ ������ ���� �׷��̸� ������ ���� ��ư�� �ش�
            else if ((groupNum == (int)TYPE_SELECTABLE.UNIT && typeNum == (int)TYPE_SELECTABLE.NONCOMBAT)
                || (groupNum == (int)TYPE_SELECTABLE.NONCOMBAT && typeNum == (int)TYPE_SELECTABLE.UNIT)) groupType = TYPE_SELECTABLE.NONCOMBAT;

            else
            {

                // �ǹ��� ������ �����ִ� ��� NONE �̰� �ٷ� Ż���Ѵ�
                groupType = TYPE_SELECTABLE.NONE;
                return;
            }
        }

        // ���������� �ǹ��̸� �̿ϼ� Ÿ������ Ȯ��
        if (IsBuildingType && IsUnfinishedbuildType) groupType = TYPE_SELECTABLE.UNFINISHED_BUILDING;
    }


    private int GetBaseTypeNum(TYPE_SELECTABLE _type)
    {

        // 100 �̸��� ���ڵ��� �� Ÿ���� Base Ÿ���� ��Ÿ���µ�, �ش� Ÿ���� ã���ش�
        int type = (int)_type;
        if (type >= VarianceManager.TYPE_SELECTABLE_INTERVAL) return type /= VarianceManager.TYPE_SELECTABLE_INTERVAL;
        else return type;
    }

    // ������ ������ �����ǰ� �Ѵ�!
    public void DeSelect(Selectable _select)
    {

        curSelected.Remove(_select);

        if (curSelected.Count == 0) 
        { 
            
            isOnlySelected = false;
        }
    }


    /// <summary>
    /// �ش� ���� ���� ����
    /// </summary>
    public bool IsContains(Selectable _select)
    {

        return curSelected.Contains(_select);
    }


    /// <summary>
    /// ���� ���� ���� �ѱ�� ���� �����ʿ��� Ȱ��
    /// </summary>
    public List<Selectable> Get()
    {

        return curSelected;
    }

    /// <summary>
    /// �δ� ���� �뵵
    /// </summary>
    public void SetSaveGroup(int _idx)
    {

        if (_idx < 0 
            || _idx >= saved.Count
            || curSelected.Count == 0) return;

        // ���� ä�� �ִ´�
        saved[_idx].Clear();

        for (int i = 0; i < curSelected.Count; i++)
        {

            saved[_idx].Add(curSelected[i]);
        }
    }

    public void GetSaveGroup(int _idx)
    {

        if (_idx < 0
            || _idx >= saved.Count
            || saved[_idx].Count == 0) return;

        curSelected.Clear();

        for (int i = 0; i < saved[_idx].Count; i++)
        {

            curSelected.Add(saved[_idx][i]);
        }
    }

    /// <summary>
    /// ���� �׷� ������ �����´�
    /// </summary>
    public int GetSize()
    {

        return curSelected.Count;
    }

    /// <summary>
    /// ����ϱ�
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, Selectable _trans = null, bool _add = false, int _num = -1)
    {

        // ���õ� ������ ���� ��� ��� ���� ���Ѵ�!
        if (curSelected.Count == 0) return;

        if (_num >= 0) _num = (_num > curSelected.Count ? curSelected.Count : _num);
        else _num = curSelected.Count;

        // ��� Ǯ��
        Command cmd = Command.GetCommand(_num, _type, _pos, _trans);

        // ��� �������� �ִ� ��� �� �ʰ��ϸ� null�� �����ϹǷ� 
        if (cmd != null)
        {

            // ��ġ ���� ������ ��� �ϴ�
            for (int i = 0; i < curSelected.Count; i++)
            {

                curSelected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("�ִ� ��� ���� �ʰ��ؼ� ����� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// ����ϱ�
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, bool _add, int _num = -1)
    {

        // ���õ� ������ ���� ��� ��� ������ ���Ѵ�!
        if (curSelected.Count == 0) return;

        if (_num >= 0) _num = (_num > curSelected.Count ? curSelected.Count : _num);
        else _num = curSelected.Count;

        // ��� Ǯ��
        Command cmd = Command.GetCommand(_num, _type);

        // ��� �������� �ִ� ��� �� �ʰ��ϸ� null�� �����ϹǷ� 
        if (cmd != null)
        {

            // ��ġ ���� ������ ��� �ϴ�
            for (int i = 0; i < curSelected.Count; i++)
            {

                curSelected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("�ִ� ��� ���� �ʰ��ؼ� ����� �� �����ϴ�.");
        }
    }
}
