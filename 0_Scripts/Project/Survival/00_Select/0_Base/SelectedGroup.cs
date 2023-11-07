using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> curSelected;                          // 현재 선택된 그룹

    // ctrl + 1 ~ 3 
    private List<List<Selectable>> saved;

    public bool isOnlySelected = false;                         // 혼자 선택할 수 있는 유닛?
    private TYPE_SELECTABLE groupType;                          // 그룹 타입 > 버튼 정보 받아오기!

    public bool IsEmpty { get { return curSelected.Count == 0 ? true : false; } }  // 비었는지 확인

    public TYPE_SELECTABLE GroupType => groupType;              // 외부는 읽기 전용

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
    /// 모든 유닛이 건설안된 건물인지 체크
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
    /// 취소 버튼 필요한지 확인
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
    /// 생성자
    /// </summary>
    public SelectedGroup()
    {

        curSelected = new List<Selectable>(VarianceManager.MAX_SELECT);

        saved = new List<List<Selectable>>(3) { new List<Selectable>(VarianceManager.MAX_SELECT),
                                                new List<Selectable>(VarianceManager.MAX_SELECT),
                                                new List<Selectable>(VarianceManager.MAX_SELECT)};
    }

    /// <summary>
    /// 선택 그룹 초기화
    /// </summary>
    public void Clear()
    {

        isOnlySelected = false;
        curSelected.Clear();
    }

    /// <summary>
    /// 유닛 추가하는 메서드
    /// </summary>
    public void Select(Selectable _select)
    {

        if (_select == null                                     // 선택 대상이 없거나
            || curSelected.Count >= VarianceManager.MAX_SELECT     // 선택가능한 갯수를 넘거나
            || curSelected.Contains(_select)                       // 이미 포함되어져 있는 경우거나
            ) return;


        curSelected.Add(_select);
        
        // 그룹 타입 설정
        if (curSelected.Count == 1)
        {

            isOnlySelected = _select.IsOnlySelected;
        }
    }

    /// <summary>
    /// 외부에서 타입 체크!
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

            // 그룹 타입이 같은지 확인
            if (groupNum == typeNum) groupType = (TYPE_SELECTABLE)groupNum;
                
            // 전투 유닛과 비전투 유닛이 섞인 그룹이면 비전투 유닛 버튼을 준다
            else if ((groupNum == (int)TYPE_SELECTABLE.UNIT && typeNum == (int)TYPE_SELECTABLE.NONCOMBAT)
                || (groupNum == (int)TYPE_SELECTABLE.NONCOMBAT && typeNum == (int)TYPE_SELECTABLE.UNIT)) groupType = TYPE_SELECTABLE.NONCOMBAT;

            else
            {

                // 건물과 유닛이 섞여있는 경우 NONE 이고 바로 탈출한다
                groupType = TYPE_SELECTABLE.NONE;
                return;
            }
        }

        // 마지막으로 건물이면 미완성 타입인지 확인
        if (IsBuildingType && IsUnfinishedbuildType) groupType = TYPE_SELECTABLE.UNFINISHED_BUILDING;
    }


    private int GetBaseTypeNum(TYPE_SELECTABLE _type)
    {

        // 100 미만의 숫자들은 각 타입의 Base 타입을 나타내는데, 해당 타입을 찾아준다
        int type = (int)_type;
        if (type >= VarianceManager.TYPE_SELECTABLE_INTERVAL) return type /= VarianceManager.TYPE_SELECTABLE_INTERVAL;
        else return type;
    }

    // 유닛이 죽으면 해제되게 한다!
    public void DeSelect(Selectable _select)
    {

        curSelected.Remove(_select);

        if (curSelected.Count == 0) 
        { 
            
            isOnlySelected = false;
        }
    }


    /// <summary>
    /// 해당 유닛 포함 여부
    /// </summary>
    public bool IsContains(Selectable _select)
    {

        return curSelected.Contains(_select);
    }


    /// <summary>
    /// 선택 유닛 정보 넘긴다 유닛 슬롯쪽에서 활용
    /// </summary>
    public List<Selectable> Get()
    {

        return curSelected;
    }

    /// <summary>
    /// 부대 지정 용도
    /// </summary>
    public void SetSaveGroup(int _idx)
    {

        if (_idx < 0 
            || _idx >= saved.Count
            || curSelected.Count == 0) return;

        // 새로 채워 넣는다
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
    /// 선택 그룹 사이즈 가져온다
    /// </summary>
    public int GetSize()
    {

        return curSelected.Count;
    }

    /// <summary>
    /// 명령하기
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, Selectable _trans = null, bool _add = false, int _num = -1)
    {

        // 선택된 유닛이 없는 경우 명령 생성 안한다!
        if (curSelected.Count == 0) return;

        if (_num >= 0) _num = (_num > curSelected.Count ? curSelected.Count : _num);
        else _num = curSelected.Count;

        // 명령 풀링
        Command cmd = Command.GetCommand(_num, _type, _pos, _trans);

        // 명령 생성에서 최대 명령 수 초과하면 null을 생성하므로 
        if (cmd != null)
        {

            // 배치 유무 따지고 명령 하달
            for (int i = 0; i < curSelected.Count; i++)
            {

                curSelected[i].GetCommand(cmd, _add);
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
    public void GiveCommand(STATE_SELECTABLE _type, bool _add, int _num = -1)
    {

        // 선택된 유닛이 없는 경우 명령 생성을 안한다!
        if (curSelected.Count == 0) return;

        if (_num >= 0) _num = (_num > curSelected.Count ? curSelected.Count : _num);
        else _num = curSelected.Count;

        // 명령 풀링
        Command cmd = Command.GetCommand(_num, _type);

        // 명령 생성에서 최대 명령 수 초과하면 null을 생성하므로 
        if (cmd != null)
        {

            // 배치 유무 따지고 명령 하달
            for (int i = 0; i < curSelected.Count; i++)
            {

                curSelected[i].GetCommand(cmd, _add);
            }
        }
        else
        {

            Debug.Log("최대 명령 수를 초과해서 명령할 수 없습니다.");
        }
    }
}
