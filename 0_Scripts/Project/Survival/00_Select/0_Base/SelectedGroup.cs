using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;

    public bool isOnlySelected = false;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }
    
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
                
        if (selected.Count == 1)
        {

            isOnlySelected = _select.IsOnlySelected;
        }
    }

    /*
    public void DragSelect(RaycastHit[] _hits, LayerMask _selectLayer, bool _add)
    {

        if (_hits == null) return;

        if (!_add) Clear();

        for (int i = 0; i < _hits.Length; i++)
        {

            if (((1 << _hits[i].transform.gameObject.layer) & _selectLayer) == 0) continue;

            Selectable select = _hits[i].transform.GetComponent<Selectable>();

            if (select.IsOnlySelected) continue;

            Select(select);
        }
    }

    public void DoubleClickSelect(RaycastHit[] _hits, LayerMask _selectLayer, ushort chkIdx, bool _add)
    {

        if (_hits == null) return;

        if (!_add) Clear();

        for (int i = 0; i < _hits.Length; i++)
        {

            if (((1 << _hits[i].transform.gameObject.layer) & _selectLayer) == 0) continue;

            Selectable select = _hits[i].transform.GetComponent<Selectable>();

            if (select == null
                || select.MyStat.SelectIdx != chkIdx
                ) continue;

            Select(select);
        }
    }
    */

    // ������ ������ �����ǰ� �Ѵ�!
    public void DeSelect(Selectable _select)
    {

        selected.Remove(_select);

        if (selected.Count == 0) isOnlySelected = false;
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
