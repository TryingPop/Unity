using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건물 건설 전 지을 수 있는지 확인용 스크립트
/// </summary>
public class PrepareBuilding : MonoBehaviour, Follower
{

    [SerializeField] protected MeshRenderer[] myMeshs;         // 색상 변경용

    [SerializeField] protected bool isBuild = true;         // 건설가능 상태?

    [SerializeField] public int selectIdx;               // 생성할 건물 idx
    [SerializeField] protected int prefabIdx = -1;

    [SerializeField] protected Transform[] chkGround;       // 현재 사용 X
    [SerializeField] protected LayerMask groundLayer;       // 현재 사용 X
    [SerializeField] protected int interval;                // 단위 배치용도 0.2 0.4f 자리 안가게 설정

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

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {

        gameObject.SetActive(true);
        ActionManager.instance.AddFollowMouse(this);        // ActionManager에서 마우스 쫓아가기 활성화
    }

    /// <summary>
    /// 충돌 판정
    /// </summary>
    private void OnTriggerStay(Collider other)
    {

        if (!other.CompareTag("Ground") && isBuild)
        {

            isBuild = false;
            SetColor();
        }
    }

    /// <summary>
    /// 탈출 충돌 판정
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        
        if (!other.CompareTag("Ground"))
        {

            isBuild = true;
            SetColor();
        }
    }

    /// <summary>
    /// 색상 설정
    /// </summary>
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

        for (int i = 0; i < myMeshs.Length; i++)
        {

            myMeshs[i].material.color = color;
        }
    }


    /// <summary>
    /// 지면 확인, 현재는 사용 X
    /// </summary>
    protected bool ChkGround()
    {

        for(int i = 0; i < chkGround.Length; i++)
        {

            if (!Physics.Raycast(chkGround[i].position, Vector3.down, 0.2f, groundLayer)) return false;
        }

        return true;
    }


    /// <summary>
    /// 건물 지을 수 잇는 공간이면 Target으로 넘겨준다 
    /// </summary>
    public Building Build()
    {

        if (!isBuild) return null;

        var go = PoolManager.instance.GetPrefabs(PrefabIdx, gameObject.layer, transform.position);
        Building building = go.GetComponent<Building>();
        building?.DisableSelectable();

        return building;
    }

    /// <summary>
    /// 사용 완료
    /// </summary>
    public void Used()
    {

        
        ActionManager.instance.RemoveFollowMouse(this);
        gameObject.SetActive(false);
    }

    
    /// <summary>
    /// 마우스 쫓아가기
    /// </summary>
    public void SetPos()
    {

        PlayerManager.instance.MouseToWorldPos(Input.mousePosition, out Vector3 pos);

        if (pos.y > -90f)
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
    /// <summary>
    /// 배치 간격
    /// </summary>
    protected int Calc(float _num, int _interval, float _div)
    {

        return Mathf.FloorToInt(_num * _div) * _interval;
    }
}