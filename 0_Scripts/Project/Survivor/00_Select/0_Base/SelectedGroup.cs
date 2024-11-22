using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<BaseObj> curSelected;                          // 현재 선택된 그룹

    // ctrl + 1 ~ 3 
    private List<List<BaseObj>> saved;

    private TYPE_SELECTABLE groupType;                          // 그룹 타입 > 버튼 정보 받아오기!
    [SerializeField] private LayerMask commandLayer = 1 << VarianceManager.LAYER_PLAYER;

    private bool isCommandable;                                 // 명령 가능 판별
    public bool IsCommandable => isCommandable;
    
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

                if (curSelected[i].MyState != STATE_SELECTABLE.BUILDING_UNFINISHED)
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

                // if (curSelected[i].IsCancelBtn) return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 생성자
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
    /// 선택 그룹 초기화
    /// </summary>
    public void Clear()
    {

        curSelected.Clear();
        isCommandable = true;
    }

    public void SelectOne(BaseObj _select)
    {

        // 선택된게 없으면 탈출한다
        if (_select == null) return;

        // 초기화
        Clear();

        // 0 번 항목 선택
        curSelected.Add(_select);

        // 커맨더 가능인지 판별
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

        if (curSelected.Count >= VarianceManager.MAX_SELECT      // 선택가능한 갯수를 넘거나
            || !isCommandable                                       // 명령 불가능한 유닛은 두 마리 이상 선택 불가능!
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
    /// LeftShift 키를 누른 경우 사용하는 드래그 선택
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
    /// 더블 클릭 선택
    /// </summary>
    public void DoubleClickSelect(ref Vector3 _center, ref Vector3 _half, int _selectIdx)
    {

        if (!isCommandable
            || curSelected.Count >= VarianceManager.MAX_SELECT) return;

        RaycastHit[] hits = Physics.BoxCastAll(_center, _half, Vector3.up, Quaternion.identity, 0f, commandLayer);

        // 더블 클릭 선택은 앞에서 검증할 예정!
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
    /// 해당 유닛 포함 여부
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
    /// 선택 유닛 정보 넘긴다 유닛 슬롯쪽에서 활용
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
    /// 부대 지정 용도
    /// </summary>
    public void SetSaveGroup(int _idx)
    {

        if (_idx < 0                            // 인덱스 체크
            || _idx >= saved.Count
            || !isCommandable                   // 명령 가능한 애들만 저장 가능
            || curSelected.Count == 0) return;  // 현재 유닛이 1마리 이상일 때만 실행

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

        Clear();
        
        for (int i = 0; i < saved[_idx].Count; i++)
        {

            curSelected.Add(saved[_idx][i]);
        }
    }

    /// <summary>
    /// 선택 그룹 사이즈 가져온다
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
    /// 명령하기
    /// </summary>
    public void GiveCommand(STATE_SELECTABLE _type, Vector3 _pos, BaseObj _trans = null, bool _add = false, int _num = -1)
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

#if UNITY_EDITOR
        else
        {

            Debug.Log("최대 명령 수를 초과해서 명령할 수 없습니다.");
        }
#endif
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

#if UNITY_EDITOR
        else
        {

            Debug.Log("최대 명령 수를 초과해서 명령할 수 없습니다.");
        }
#endif
    }
}
