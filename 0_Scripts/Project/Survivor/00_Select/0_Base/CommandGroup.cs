using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGroup : ActionGroup<Selectable>
{

    public CommandGroup(int _capacity) : base(_capacity) { }

    public void GetCommand(Command _cmd, bool _add = false)
    {

        Node node = head.Next;

        while (!IsLast(node))
        {

            if (node == null)
            {

#if UNITY_EDITOR

                Debug.LogError("ActionGroup의 Node 순서에 이상이 있어\n" +
                    "행동이 정상적으로 작동하지 않습니다.");
#endif

                return;
            }

            node.Val.GetCommand(_cmd, _add);
            node = node.Next;
        }
    }
}
