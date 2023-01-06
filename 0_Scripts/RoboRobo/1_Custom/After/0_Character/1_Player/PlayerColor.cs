using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{

    [SerializeField] private SkinnedMeshRenderer chrColor;

    private void Awake()
    {

        if (chrColor == null) 
        { 

            chrColor = GetComponentInChildren<SkinnedMeshRenderer>(); 
        }
    }

    public void ChangeColor(Color color)
    {

        chrColor.material.color = color;
    }

}
