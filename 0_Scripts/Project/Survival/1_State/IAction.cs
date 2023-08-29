using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAction<T> : MonoBehaviour 
                                    where T : Selectable
{


    public abstract void Action(T _param);
}
