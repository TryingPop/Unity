using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    [SerializeField] private BuildGroup[] groups;

    public BuildGroup GetGroup(VariableManager.STATE_BUTTON_OPTION buttonOpt)
    {

        switch (buttonOpt) 
        {

            case VariableManager.STATE_BUTTON_OPTION.BUILD:
                return groups?[0];

            default:
                return null;
        }
    }
}
