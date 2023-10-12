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
    /// 유닛 추가하는 메서드
    /// </summary>
    public void Select(Selectable _select)
    {

        if (_select == null                                     // 선택 대상이 없거나
            || selected.Count >= VariableManager.MAX_SELECT     // 선택가능한 갯수를 넘거나
            || selected.Contains(_select)                       // 이미 포함되어져 있는 경우거나
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

    // 유닛이 죽으면 해제되게 한다!
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
    /// 명령하기
    /// </summary>
    /// <param name="_type">명령 종류</param>
    /// <param name="_pos">좌표</param>
    /// <param name="_trans">대상</param>
    /// <param name="_add">예약 명령 여부</param>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, Selectable _trans = null, bool _add = false)
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
    public void GiveCommand(STATE_SELECTABLE _type, bool _add)
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
}
