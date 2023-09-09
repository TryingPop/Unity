using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectedGroup
{

    private List<Selectable> selected;
    
    public static readonly int MAX_SELECT = 30;

    public int actionNum;

    public bool IsEmpty { get { return selected.Count == 0 ? true : false; } }
    
    public SelectedGroup()
    {

        selected = new List<Selectable>(MAX_SELECT);
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
        else if (selected.Count < MAX_SELECT)
        {

            if (!selected.Contains(_select))
            {

                selected.Add(_select);

                if (selected.Count == 1)
                {

                    actionNum = _select.myActionNum;
                }
                else if (selected.Count == 2)
                {

                    actionNum = 43028;

                    /*
                    actionNum = 0;

                    // LM, S, LP, H, LA 
                    for (int i = 1; i <= 5; i++)
                    {

                        if (i % 2 == 1)
                        {

                            Debug.Log(i);
                            actionNum += 1 << (i + InputManager.MOUSE_L);
                        }
                        else
                        {

                            actionNum += 1 << i;
                        }
                    }

                    Debug.Log(actionNum);       // 43028
                    */
                }

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

        // 대상이 있으면 대상을 쫓고 없으면 뭉치지 않게 배치를 한다
        bool posBatch = _trans == null;

        // MOUSE_R은 캐릭터쪽에서 해결!
        if (_type != InputManager.MOUSE_R) _type %= InputManager.MOUSE_L;


        // 명령 풀링
        Command cmd = Command.GetCommand((byte)selected.Count, _type, _pos, _trans);   // << 함께 참조하는게 문제였슴니다!

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

            Debug.Log("명령이 가득차서 보낼 수 없습니다.");
        }
    }

    public bool ChkCommand(int _type)
    {

        // 선택된 유닛이 없는 경우 실행 안함
        // if (selected.Count == 0) return false;

        // 행동할 수 없는 경우면 전달자체를 안한다!
        if (_type != InputManager.MOUSE_R && (1 << _type & actionNum) == 0) return false;
        return true;
    }

}
