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
    /// 유닛 추가하는 메서드
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
    /// 유닛이 선택될 때마다 마지막에 사망여부 확인
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
    /// 명령하기
    /// </summary>
    /// <param name="_type">명령 종류</param>
    /// <param name="_pos">좌표</param>
    /// <param name="_trans">대상</param>
    /// <param name="_add">예약 명령 여부</param>
    public void Command(int _type, Vector3 _pos, Transform _trans = null, bool _add = false)
    {

        // 선택된 유닛이 없는 경우 실행 안함
        if (selected.Count == 0) return;

        // 대상이 있으면 대상을 쫓고 없으면 뭉치지 않게 배치를 한다
        bool posBatch = _trans == null;
        Command cmd = new Command((byte)selected.Count, _type, _pos, _trans);   // << 함께 참조하는게 문제였슴니다!

        // 배치 유무 따지고 명령 하달
        for (int i = 0; i < selected.Count; i++)
        {

            // if (posBatch) SetNextPos(i, ref _pos);
            selected[i].GetCommand(cmd, _add);
        }
    }



}
