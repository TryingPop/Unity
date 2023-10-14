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
        TYPE_SELECTABLE selectType = _select.MyStat.MyType;

        if (selected.Count == 1)
        {

            isOnlySelected = _select.IsOnlySelected;
            groupType = selectType;
        }
        else
        {

        }
    }

    private void SetType(TYPE_SELECTABLE _type)
    {

        if (groupType == _type 
            || groupType == TYPE_SELECTABLE.NONE)
        {

            return;
        }

        int type = (int)_type;
        int curType = (int)groupType;

        if (curType == type)
        {

            groupType = (TYPE_SELECTABLE)type;
            return;
        }

        if (curType <= 2 && type <= 2)
        {

            // UNIT_NONCOMBAT && UNIT_COMBAT�� ��� UNIT_NONCOMBAT���� �Ѵ�
            groupType = TYPE_SELECTABLE.UNIT_NONCOMBAT;
            return;
        }

        groupType = TYPE_SELECTABLE.NONE;
    }


    // ������ ������ �����ǰ� �Ѵ�!
    public void DeSelect(Selectable _select)
    {

        selected.Remove(_select);

        if (selected.Count == 0) 
        { 
            
            isOnlySelected = false;
            groupType = TYPE_SELECTABLE.NONE;
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
