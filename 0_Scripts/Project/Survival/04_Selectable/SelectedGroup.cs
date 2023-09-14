using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;

    public bool isBuilding = false;
    public int actionNum;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }
    
    public SelectedGroup()
    {

        selected = new List<Selectable>(VariableManager.MAX_SELECT);
    }

    public void Clear()
    {

        actionNum = 0;
        selected.Clear();
    }

    public void Select(Selectable _target, bool _putLS)
    {

        if (_target == null) return;

        if (_putLS)
        {

            if (IsContains(_target)) DeSelect(_target);
            else Add(_target);
        }
        else
        {

            Clear();
            Add(_target);
        }

        ChkDead();
    }

    /// <summary>
    /// ���� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="_select"></param>
    public void Add(Selectable _select)
    {

        if (_select == null) return;
        else if (selected.Count < VariableManager.MAX_SELECT)
        {

            if (!selected.Contains(_select))
            {

                selected.Add(_select);
            }
        }
    }

    public void SetActionNum()
    {

        actionNum = 0;

        if (selected.Count == 0) return; 

        actionNum = selected[0].myActionNum;

        for (int i = 1; i < selected.Count; i++)
        {

            actionNum = actionNum & selected[i].myActionNum;
        } 
    }

    /// <summary>
    /// ������ ���õ� ������ �������� ������� Ȯ��
    /// </summary>
    public void ChkDead()
    {

        for (int i = selected.Count - 1; i >= 0; i--)
        {

            if (!selected[i].gameObject.activeSelf 
                || selected[i].gameObject.layer == VariableManager.LAYER_DEAD)
            {

                selected.Remove(selected[i]);
            }
        }
    }

    public void DeSelect(Selectable _select)
    {

        selected.Remove(_select);
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
    /// <param name="_type">��� ����</param>
    /// <param name="_pos">��ǥ</param>
    /// <param name="_trans">���</param>
    /// <param name="_add">���� ��� ����</param>
    public void GiveCommand(int _type, Vector3 _pos, Selectable _trans = null, bool _add = false)
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
    public void GiveCommand(int _type, bool _add)
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

    public bool ChkCommand(int _type)
    {

        // �ൿ�� �� ���� ���� ������ü�� ���Ѵ�!

        if (_type != VariableManager.MOUSE_R && (1 << _type & actionNum) == 0) return false;
        return true;
    }

    // Build �κ�?

}
