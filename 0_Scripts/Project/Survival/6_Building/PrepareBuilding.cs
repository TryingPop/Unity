using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBuilding : MonoBehaviour
{

    [SerializeField] protected MeshRenderer myMesh;

    protected bool isBuild = true;

    [SerializeField] protected int prefab

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Ground") && isBuild)
        {

            isBuild = false;
            SetColor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Wall"))
        {

            isBuild = true;
            SetColor();
        }
    }

    protected void SetColor()
    {

        Color color;

        if (isBuild)
        {

            color = Color.green;
            color.a = 0.2f;
        }
        else
        {

            color = Color.red;
            color.a = 0.2f;
        }

        myMesh.material.color = color;
    }

    public void Build()
    {

        if (!isBuild) return;


    }
}