using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class IdleNode : Node
{
    public override NodeState Evaluate()
    {

        Debug.Log("노는 중이에용");
        return NodeState.SUCCESS;
    }
}
