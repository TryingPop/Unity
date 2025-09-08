using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� �Ǽ� �� ���� �� �ִ��� Ȯ�ο� ��ũ��Ʈ
/// </summary>
public class PrepareBuilding : MonoBehaviour, Follower
{

    [SerializeField] protected MeshRenderer[] myMeshs;         // ���� �����

    [SerializeField] protected bool isBuild = true;         // �Ǽ����� ����?

    [SerializeField] public int selectIdx;               // ������ �ǹ� idx
    [SerializeField] protected int prefabIdx = -1;

    // [SerializeField] protected Transform[] chkGround;       // ���� ��� X
    // [SerializeField] protected LayerMask groundLayer;       // ���� ��� X
    [SerializeField] protected int interval;                // ���� ��ġ�뵵 0.2 0.4f �ڸ� �Ȱ��� ����

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
    /// �ʱ�ȭ
    /// </summary>
    public void Init()
    {

        gameObject.SetActive(true);
        ActionManager.instance.AddFollowMouse(this);        // ActionManager���� ���콺 �Ѿư��� Ȱ��ȭ
    }

    /// <summary>
    /// �浹 ����
    /// </summary>
    private void OnTriggerStay(Collider other)
    {

#if UNITY_EDITOR

        Debug.Log($"{other.name}�� stay");
#endif

        if (!other.CompareTag("Ground") && isBuild)
        {

            isBuild = false;
            SetColor();
        }
    }

    /// <summary>
    /// Ż�� �浹 ����
    /// </summary>
    private void OnTriggerExit(Collider other)
    {

#if UNITY_EDITOR

        Debug.Log($"{other.name}�� exit");
#endif
        if (!other.CompareTag("Ground"))
        {

            isBuild = true;
            SetColor();
        }
    }

#if UNITY_EDITOR
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log($"{other.name}�� enter");
    }
#endif

    /// <summary>
    /// ���� ����
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
    /// ���� Ȯ��, ����� ��� X
    /// </summary>
    /*
    protected bool ChkGround()
    {

        for(int i = 0; i < chkGround.Length; i++)
        {

            if (!Physics.Raycast(chkGround[i].position, Vector3.down, 0.2f, groundLayer)) return false;
        }

        return true;
    }
    */

    /// <summary>
    /// �ǹ� ���� �� �մ� �����̸� Target���� �Ѱ��ش� 
    /// </summary>
    public Building Build()
    {

        if (!isBuild) return null;

#if UNITY_EDITOR

        Debug.Log("�ϲ� ������ �÷��̾� �Դϴ�.\n�׷��� ������ �÷��̾� ���̾�� ��������,\n"
            + "���� Ȯ�忡���� ���̾ �ٸ� ������� �������ּž� �մϴ�.");
#endif

        // �ǹ� �Ǽ��ϴ� ���̾�� �÷��̾�� �����ȴ�.
        var go = PoolManager.instance.GetPrefabs(PrefabIdx, VarianceManager.LAYER_PLAYER, transform.position);
        Building building = go.GetComponent<Building>();
        building?.DisableSelectable();

        return building;
    }

    /// <summary>
    /// ��� �Ϸ�
    /// </summary>
    public void Used()
    {
        
        ActionManager.instance.RemoveFollowMouse(this);
        isBuild = true;
        SetColor();
        gameObject.SetActive(false);
    }

    
    /// <summary>
    /// ���콺 �Ѿư���
    /// </summary>
    public void SetPos()
    {

        PlayerManager.instance.UiPosToWorldPos(Input.mousePosition, out Vector3 pos);

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
    /// ��ġ ����
    /// </summary>
    protected int Calc(float _num, int _interval, float _div)
    {

        return Mathf.FloorToInt(_num * _div) * _interval;
    }
}