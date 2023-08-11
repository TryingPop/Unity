using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;

    private static readonly int MAX_SELECT = 16;

    public static float xBatchSize = 2f;
    public static float zBatchSize = 2f;

    public bool isUnit;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }
    public SelectedGroup()
    {

        selected = new List<Selectable>(MAX_SELECT);
        isUnit = true;
    }

    public void Clear()
    {

        selected.Clear();
        isUnit = true;
    }


    public void Select(Transform _target, bool _putLS)
    {

        if (_target == null) return;
        Selectable select = _target.GetComponent<Selectable>();
        if (select == null) return;

        if (_putLS)
        {

            if (IsContains(select)) DeSelect(select);
            else if (isUnit) Add(select);
        }
        else
        {

            Clear();
            Add(select);
            if (!_target.GetComponent<BaseUnit>()) isUnit = false;
        }

    }

    /// <summary>
    /// 유닛 추가하는 메서드
    /// </summary>
    /// <param name="_select"></param>
    public void Add(Selectable _select)
    {

        if (_select == null && !isUnit) return;

        else if(selected.Count < MAX_SELECT)
        {

            if (!selected.Contains(_select))
            {

                selected.Add(_select);
            }
        }
    }

    public void ChkDead()
    {

        for (int i = selected.Count - 1; i >= 0; i--)
        {

            if (!selected[i].gameObject.activeSelf)
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

    public void Command(int _type, Vector3 _pos, Transform _trans = null, bool _add = false)
    {

        for(int i = 0; i < selected.Count; i++)
        {

            selected[i].DoCommand(new Command(_type, _pos, _trans), _add);
        }
    }


    /// <summary>
    /// 회오리? 모양의 배치
    /// </summary>
    /// <param name="_num">몇 번째 유닛?</param>
    /// <param name="_pos">배치될 좌표</param>
    public void SetNextPos(int _num, ref Vector3 _pos)
    {

        if (_num == 0) return;
        int i = 0;

        int n = _num;
        while (n > 0)
        {

            i += 1;
            n -= 2 * i;
        }

        if (n + i <= 0)
        {

            if (i % 2 == 0)
            {

                _pos.x -= xBatchSize;
            }
            else
            {

                _pos.x += xBatchSize;
            }
        }
        else
        {

            if (i % 2 == 0)
            {

                _pos.z += zBatchSize;
            }
            else
            {

                _pos.z -= zBatchSize;
            }
        }
    }
}
