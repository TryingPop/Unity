using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{

    protected List<Node> nodes = new List<Node>();

    public Selector(List<Node> nodes)
    {

        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {

        foreach (var node in nodes)
        {

            switch (node.Evaluate())
            {

                case NodeState.RUNNING:
                    nodeState = NodeState.RUNNING;
                    return nodeState;

                case NodeState.SUCCESS:
                    nodeState = NodeState.SUCCESS;
                    return nodeState;

                case NodeState.FAILURE:
                    break;
            }
        }

        nodeState = NodeState.FAILURE;
        return nodeState;
    }
}
