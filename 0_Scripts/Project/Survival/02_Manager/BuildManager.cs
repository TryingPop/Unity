using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

    [SerializeField] private BuildGroup[] groups;

    public BuildGroup GetGroup(TYPE_BUTTON_OPTION buttonOpt)
    {

        switch (buttonOpt) 
        {

            case TYPE_BUTTON_OPTION.BUILD:
                return groups?[0];

            default:
                return null;
        }
    }
}
