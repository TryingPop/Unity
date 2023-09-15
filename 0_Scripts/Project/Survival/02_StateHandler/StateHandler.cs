using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler<T, V> : MonoBehaviour
                                    where T : Selectable
                                    where V : IAction<T>
{

    public V[] actions;
    public int actionType;

    public abstract void Action(T _param);

    /// <summary>
    /// �ܺο��� �ൿ �߰�(��ų �ٲٰԵɶ�?
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_action"></param>
    public void AddActions(int _idx, V _action)
    {

        if (ChkActions(_idx)) actions[_idx] = _action;
    }

    /// <summary>
    /// �ش� �ൿ�� �ִ��� Ȯ��
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool ChkActions(int _type)
    {

        return (actionType & 1 << _type) != 0;
    }

    public void SetActionType()
    {

        actionType = 0;
        int add = InputManager.MOUSE_L;

        for (int i = 0; i < actions.Length; i++)
        {

            if (actions[i].needTarget)
            {

                actionType += 1 << (i + add);
            }
            else
            {

                actionType += 1 << i;
            }
        }
    }
}