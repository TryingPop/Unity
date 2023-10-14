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
    /// 유닛 추가하는 메서드
    /// </summary>
    public void Select(Selectable _select)
    {

        if (_select == null                                     // 선택 대상이 없거나
            || selected.Count >= VariableManager.MAX_SELECT     // 선택가능한 갯수를 넘거나
            || selected.Contains(_select)                       // 이미 포함되어져 있는 경우거나
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

            // UNIT_NONCOMBAT && UNIT_COMBAT인 경우 UNIT_NONCOMBAT으로 한다
            groupType = TYPE_SELECTABLE.UNIT_NONCOMBAT;
            return;
        }

        groupType = TYPE_SELECTABLE.NONE;
    }


    // 유닛이 죽으면 해제되게 한다!
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
    /// 명령하기
    /// </summary>
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
