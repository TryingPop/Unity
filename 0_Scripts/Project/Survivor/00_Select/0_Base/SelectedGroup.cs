using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<BaseObj> curSelected;                          // ���� ���õ� �׷�

    // ctrl + 1 ~ 3 
    private List<List<BaseObj>> saved;

    private TYPE_SELECTABLE groupType;                          // �׷� Ÿ�� > ��ư ���� �޾ƿ���!
    [SerializeField] private LayerMask commandLayer = 1 << VarianceManager.LAYER_PLAYER;

    private bool isCommandable;                                 // ��� ���� �Ǻ�
    public bool IsCommandable => isCommandable;
    
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

                if (curSelected[i].MyState != STATE_SELECTABLE.BUILDING_UNFINISHED)
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

                // if (curSelected[i].IsCancelBtn) return true;
            }

            return false;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public SelectedGroup(int _layer)
    {

        curSelected = new List<BaseObj>(VarianceManager.MAX_SELECT);

        saved = new List<List<BaseObj>>(3) { new(VarianceManager.MAX_SELECT),
                                                new(VarianceManager.MAX_SELECT),
                                                new(VarianceManager.MAX_SELECT)};

        commandLayer.value = 1 << _layer;
    }

    /// <summary>
    /// ���� �׷� �ʱ�ȭ
    /// </summary>
    public void Clear()
    {

        curSelected.Clear();
        isCommandable = true;
    }

    public void SelectOne(BaseObj _select)
    {

        // ���õȰ� ������ Ż���Ѵ�
        if (_select == null) return;

        // �ʱ�ȭ
        Clear();

        // 0 �� �׸� ����
        curSelected.Add(_select);

        // Ŀ�Ǵ� �������� �Ǻ�
        // if (((1 << _select.MyTeam.TeamLayerNumber) & (commandLayer)) == 0) isCommandable = false;
        // else isCommandable = true;
        isCommandable = ChkCommandable(_select);
    }

    public void AddSelect(BaseObj _select)
    {

        if (curSelected.Count == 0) SelectOne(_select);
        else if (curSelected.Contains(_select)) DeSelect(_select);
        else if (ChkCommandable(_select)) AppendSelect(_select);

    }

    public void AppendSelect(BaseObj _select)
    {

        if (curSelected.Count >= VarianceManager.MAX_SELECT      // ���ð����� ������ �Ѱų�
            || !isCommandable                                       // ��� �Ұ����� ������ �� ���� �̻� ���� �Ұ���!
            ) return;

        curSelected.Add(_select);
    }

    public void DragSelect(ref Vector3 _center, ref Vector3 _half, int _selectLayer)
    {

        int len = Physics.BoxCastNonAlloc(_center, _half, Vector3.up, VarianceManager.hits, Quaternion.identity, 0f, commandLayer);

        if (len > 0)
        {

            Clear();
            for (int i = 0; i < len; i++)
            {

                BaseObj select = VarianceManager.hits[i].transform.GetComponent<BaseObj>();
                curSelected.Add(select);
            }

            return;
        }

        len = Physics.BoxCastNonAlloc(_center, _half, Vector3.up, VarianceManager.hit, Quaternion.identity, 0f, _selectLayer);
        if (len > 0)
        {

            BaseObj select = VarianceManager.hit[0].transform.GetComponent<BaseObj>();
            SelectOne(select);

            return;
        }
    }

    /// <summary>
    /// LeftShift Ű�� ���� ��� ����ϴ� �巡�� ����
    /// </summary>
    public void AddDragSelect(ref Vector3 _center, ref Vector3 _half)
    {

        if (!isCommandable) return;

        int len = Physics.BoxCastNonAlloc(_center, _half, Vector3.up, VarianceManager.hits, Quaternion.identity, 0f, commandLayer);

        for (int i = 0; i < len; i++)
        {

            BaseObj select = VarianceManager.hits[i].transform.GetComponent<BaseObj>();

            if (curSelected.Count < VarianceManager.MAX_SELECT
                && !curSelected.Contains(select)) curSelected.Add(select);
            else if (curSelected.Count >= VarianceManager.MAX_SELECT) return;
        }
    }

    /// <summary>
    /// ���� Ŭ�� ����
    /// </summary>
    public void DoubleClickSelect(ref Vector3 _center, ref Vector3 _half, int _selectIdx)
    {

        if (!isCommandable
            || curSelected.Count >= VarianceManager.MAX_SELECT) return;

        RaycastHit[] hits = Physics.BoxCastAll(_center, _half, Vector3.up, Quaternion.identity, 0f, commandLayer);

        // ���� Ŭ�� ������ �տ��� ������ ����!
        for (int i = 0; i < hits.Length; i++)
        {

            BaseObj select = hits[i].transform.GetComponent<BaseObj>();
            if (select.MyStat.SelectIdx == _selectIdx
                && !curSelected.Contains(select)
                && curSelected.Count < VarianceManager.MAX_SELECT) curSelected.Add(select);
            else if (curSelected.Count >= VarianceManager.MAX_SELECT) return;
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
    public void DeSelect(BaseObj _select)
    {

        curSelected.Remove(_select);

        if (curSelected.Count == 0) isCommandable = true;
    }

    public void DeselectSavedGroup(BaseObj _select)
    {

        for (int i = 0; i < saved.Count; i++)
        {

            if (saved[i].Contains(_select)) saved[i].Remove(_select);
        }
    }

    public bool ContainsSavedGroup(BaseObj _select)
    {

        for (int i = 0; i < saved.Count; i++)
        {

            if (saved[i].Contains(_select)) return true;
        }

        return false;
    }


    /// <summary>
    /// �ش� ���� ���� ����
    /// </summary>
    public bool Contains(BaseObj _select)
    {

        return curSelected.Contains(_select);
    }

    public bool ChkCommandable(BaseObj _select) 
    {

        if (_select == null
            || _select.MyTeam == null) return false;

        int chk = 1 << _select.MyTeam.TeamLayerNumber;

        if ((chk & commandLayer) != 0) return true;

        return false;
    }

    /// <summary>
    /// ���� ���� ���� �ѱ�� ���� �����ʿ��� Ȱ��
    /// </summary>
    public List<BaseObj> Get(int groupNum = 0)
    {

        switch (groupNum)
        {


            case 1:
                return saved[0];

            case 2:
                return saved[1];

            case 3:
                return saved[2];

            default:
                return curSelected;
        }
    }

    /// <summary>
    /// �δ� ���� �뵵
    /// </summary>
    public void SetSaveGroup(int _idx)
    {

        if (_idx < 0                            // �ε��� üũ
            || _idx >= saved.Count
            || !isCommandable                   // ��� ������ �ֵ鸸 ���� ����
            || curSelected.Count == 0) return;  // ���� ������ 1���� �̻��� ���� ����

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

        Clear();
        
        for (int i = 0; i < saved[_idx].Count; i++)
        {

            curSelected.Add(saved[_idx][i]);
        }
    }

    /// <summary>
    /// ���� �׷� ������ �����´�
    /// </summary>
    public int GetSize(int groupNum = 0)
    {

        switch (groupNum)
        {


            case 1:
                return saved[0].Count;

            case 2:
                return saved[1].Count;

            case 3:
                return saved[2].Count;

            default:
                return curSelected.Count;
        }
    }

    /// <summary>
    /// ����ϱ�
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, BaseObj _trans = null, bool _add = false, int _num = -1)
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

#if UNITY_EDITOR
        else
        {

            Debug.Log("�ִ� ��� ���� �ʰ��ؼ� ����� �� �����ϴ�.");
        }
#endif
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

#if UNITY_EDITOR
        else
        {

            Debug.Log("�ִ� ��� ���� �ʰ��ؼ� ����� �� �����ϴ�.");
        }
#endif
    }
}
