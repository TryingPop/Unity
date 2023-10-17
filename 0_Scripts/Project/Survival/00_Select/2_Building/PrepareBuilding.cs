using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareBuilding : FollowMouse
{

    [SerializeField] protected MeshRenderer myMesh;

    [SerializeField] protected bool isBuild = true;

    [SerializeField] public ushort selectIdx;
    [SerializeField] protected short prefabIdx = -1;

    [SerializeField] protected Transform[] chkGround;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected int interval;

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

    public void Init()
    {

        gameObject.SetActive(true);
        ActionManager.instance.AddFollowMouse(this);
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


    public Building Build()
    {

        if (!isBuild) return null;

        var go = PoolManager.instance.GetPrefabs(PrefabIdx, gameObject.layer, transform.position);
        Building building = go.GetComponent<Building>();
        building?.DisableSelectable();

        return building;
    }

    public void Used()
    {

        
        ActionManager.instance.RemoveFollowMouse(this);
        gameObject.SetActive(false);
    }

    

    public override void SetPos()
    {

        InputManager.instance.MouseToWorldPosition(out Vector3 pos);

        if (pos.y < 100f)
        {

            if (interval > 0)
            {

                float div = 1.0f / interval;

                pos = new Vector3(

                    Calc(pos.x, interval, div),
                    Calc(pos.y, interval, div),
                    Calc(pos.z, interval, div)
                    );
            }

            transform.position = pos;
        }
    }

    protected int Calc(float _num, int _interval, float _div)
    {

        return Mathf.FloorToInt(_num * _div) * _interval;
    }
}