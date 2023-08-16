using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;
    
    public static readonly int MAX_SELECT = 16;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }
    
    public SelectedGroup()
    {

        selected = new List<Selectable>(MAX_SELECT);
    }

    public void Clear()
    {

        selected.Clear();
    }


    public void Select(Transform _target, bool _putLS)
    {

        if (_target == null) return;
        Selectable select = _target.GetComponent<Selectable>();
        if (select == null) return;

        if (_putLS)
        {

            if (IsContains(select)) DeSelect(select);
            else Add(select);
        }
        else
        {

            Clear();
            Add(select);
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
        else if (selected.Count < MAX_SELECT)
        {

            if (!selected.Contains(_select))
            {

                selected.Add(_select);
            }
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
                || selected[i].gameObject.layer == IDamagable.LAYER_DEAD)
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

    /// <summary>
    /// ����ϱ�
    /// </summary>
    /// <param name="_type">��� ����</param>
    /// <param name="_pos">��ǥ</param>
    /// <param name="_trans">���</param>
    /// <param name="_add">���� ��� ����</param>
    public void Command(int _type, Vector3 _pos, Transform _trans = null, bool _add = false)
    {

        // ���õ� ������ ���� ��� ���� ����
        if (selected.Count == 0) return;

        // ����� ������ ����� �Ѱ� ������ ��ġ�� �ʰ� ��ġ�� �Ѵ�
        bool posBatch = _trans == null;
        Command cmd = new Command((byte)selected.Count, _type, _pos, _trans);   // << �Բ� �����ϴ°� ���������ϴ�!

        // ��ġ ���� ������ ��� �ϴ�
        for (int i = 0; i < selected.Count; i++)
        {

            // if (posBatch) SetNextPos(i, ref _pos);
            selected[i].GetCommand(cmd, _add);
        }
    }



}
