using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBuilding : MonoBehaviour
{

    [SerializeField] protected MeshRenderer myMesh;

    [SerializeField] protected bool isBuild = true;

    [SerializeField] public ushort selectIdx;
    [SerializeField] protected short prefabIdx = -1;

    [SerializeField] protected Transform[] chkGround;
    [SerializeField] protected LayerMask groundLayer;

    // [SerializeField] private ResourceManager.TYPE_RESOURCE costType;
    // [SerializeField] private int cost;

    // public ResourceManager.TYPE_RESOURCE CostType => costType;
    // public int Cost => cost;
    public int PrefabIdx 
    { 
        get 
        { 

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(selectIdx);
            }

            return prefabIdx;
        } 
    }

    private void OnTriggerStay(Collider other)
    {

        if (!other.CompareTag("Ground") && isBuild)
        {

            isBuild = false;
            SetColor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (!other.CompareTag("Ground"))
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
        }
        else
        {

            color = Color.red;
        }

        myMesh.material.color = color;
    }


    protected bool ChkGround()
    {

        for(int i = 0; i < chkGround.Length; i++)
        {

            if (!Physics.Raycast(chkGround[i].position, Vector3.down, 0.2f, groundLayer)) return false;
        }

        return true;
    }


    public GameObject Build()
    {

        if (!isBuild) return null;

        var go = PoolManager.instance.GetPrefabs(PrefabIdx, TeamManager.instance.ChkTeamNumber(gameObject.layer));

        if (go) 
        {

            go.transform.position = transform.position;
        }

        return go;
    }
}