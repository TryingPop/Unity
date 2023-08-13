using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IUnitAction : MonoBehaviour
{

    protected Image img;

    public abstract void Action(Unit _unit);

    public abstract void Changed(Unit _unit);
}
