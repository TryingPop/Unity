using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;

    public bool isOnlySelected = false;
    private TYPE_SELECTABLE groupType;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }

    public TYPE_SELECTABLE GroupType => groupType;

    public SelectedGroup()
    {

        selected = new List<Selectable>(VariableManager.MAX_SELECT);
    }

    public void Clear()
    {

        isOnlySelected = false;
        selected.Clear();
    }

    /// <summary>
    /// ���� �߰��ϴ� �޼���
    /// </summary>
    public void Select(Selectable _select)
    {

        if (_select == null                                     // ���� ����� ���ų�
            || selected.Count >= VariableManager.MAX_SELECT     // ���ð����� ������ �Ѱų�
            || selected.Contains(_select)                       // �̹� ���ԵǾ��� �ִ� ���ų�
            ) return;


        selected.Add(_select);
        
        // �׷� Ÿ�� ����
        if (selected.Count == 1)
        {

            isOnlySelected = _select.IsOnlySelected;
        }
    }

    /// <summary>
    /// �ܺο��� Ÿ�� üũ!
    /// </summary>
    public void ChkGroupType()
    {

        if (selected.Count == 0)
        {

            groupType = TYPE_SELECTABLE.NONE;
            return;
        }

        groupType = selected[0].MyStat.MyType;

        for (int i = 1; i < selected.Count; i++)
        {

            var type = selected[i].MyStat.MyType;
            if (groupType == type) continue;
            else
            {

                int groupNum = (int)groupType % VariableManager.TYPE_SELECTABLE_INTERVAL;
                int typeNum = (int)type % VariableManager.TYPE_SELECTABLE_INTERVAL;

                if (groupNum == typeNum) groupType = (TYPE_SELECTABLE)groupNum;
                else if ((groupNum == 2 && typeNum == 1)
                    || (groupNum == 1 && typeNum == 2)) groupType = (TYPE_SELECTABLE)1;
                else
                {

                    groupType = TYPE_SELECTABLE.NONE;
                    break;
                }
            }
        }
    }


    // ������ ������ �����ǰ� �Ѵ�!
    public void DeSelect(Selectable _select)
    {

        selected.Remove(_select);

        if (selected.Count == 0) 
        { 
            
            isOnlySelected = false;
        }
    }


    public bool IsContains(Selectable _select)
    {

        return selected.Contains(_select);
    }

    public List<Selectable> Get()
    {

        return selected;
    }

    public int GetSize()
    {

        return selected.Count;
    }

    /// <summary>
    /// ����ϱ�
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, Selectable _trans = null, bool _add = false)
    {

        // ���õ� ������ ���� ��� ��� ���� ���Ѵ�!
        if (selected.Count == 0) return;

        // ��� Ǯ��
        Command cmd = Command.GetCommand((byte)selected.Count, _type, _pos, _trans);

        // ��� �������� �ִ� ��� �� �ʰ��ϸ� null�� �����ϹǷ� 
        if (cmd != null)
        {

            // ��ġ ���� ������ ��� �ϴ�
            for (int i = 0; i < selected.Count; i++)
            {

                selected[i].GetCommand(cmd, _add);
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
    public void GiveCommand(STATE_SELECTABLE _type, bool _add)
    {

        // ���õ� ������ ���� ��� ��� ������ ���Ѵ�!
        if (selected.Count == 0) return;

        // ��� Ǯ��
        Command cmd = Command.GetCommand((byte)selected.Count, _type);

        // ��� �������� �ִ� ��� �� �ʰ��ϸ� null�� �����ϹǷ� 
        if (cmd != null)
        {

            // ��ġ ���� ������ ��� �ϴ�
            for (int i = 0; i < selected.Count; i++)
            {

                selected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("�ִ� ��� ���� �ʰ��ؼ� ����� �� �����ϴ�.");
        }
    }
}
