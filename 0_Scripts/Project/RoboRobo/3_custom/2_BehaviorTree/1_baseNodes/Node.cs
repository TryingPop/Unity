using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

[Serializable]
public abstract class Node
{

    protected NodeState nodeState;

    public NodeState NodeState => nodeState;

    public abstract NodeState Evaluate();
}


public enum NodeState
{

    RUNNING, SUCCESS, FAILURE,
}