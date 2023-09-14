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
    /// 유닛 추가하는 메서드
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
    /// 유닛이 선택될 때마다 마지막에 사망여부 확인
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
    /// 명령하기
    /// </summary>
    /// <param name="_type">명령 종류</param>
    /// <param name="_pos">좌표</param>
    /// <param name="_trans">대상</param>
    /// <param name="_add">예약 명령 여부</param>
    public void GiveCommand(int _type, Vector3 _pos, Selectable _trans = null, bool _add = false)
    {

        // 명령 풀링
        Command cmd = Command.GetCommand((byte)selected.Count, _type, _pos, _trans);

        // 명령 생성에서 최대 명령 수 초과하면 null을 생성하므로 
        if (cmd != null)
        {

            // 배치 유무 따지고 명령 하달
            for (int i = 0; i < selected.Count; i++)
            {

                selected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("최대 명령 수를 초과해서 명령할 수 없습니다.");
        }
    }

    /// <summary>
    /// 명령하기
    /// </summary>
    public void GiveCommand(int _type, bool _add)
    {

        // 명령 풀링
        Command cmd = Command.GetCommand((byte)selected.Count, _type);

        // 명령 생성에서 최대 명령 수 초과하면 null을 생성하므로 
        if (cmd != null)
        {

            // 배치 유무 따지고 명령 하달
            for (int i = 0; i < selected.Count; i++)
            {

                selected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("최대 명령 수를 초과해서 명령할 수 없습니다.");
        }
    }

    public bool ChkCommand(int _type)
    {

        // 행동할 수 없는 경우면 전달자체를 안한다!

        if (_type != VariableManager.MOUSE_R && (1 << _type & actionNum) == 0) return false;
        return true;
    }

    // Build 부분?

}
