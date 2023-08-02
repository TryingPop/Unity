using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour
{

    private Transform targetTrans;
    private bool isSelected;
    private MeshRenderer myMesh;


    private void Awake()
    {
        
        myMesh = GetComponent<MeshRenderer>();
    }


    private void LateUpdate()
    {

        if (isSelected) transform.position = targetTrans.position;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="target">����� �Ǵ� ����</param>
    public void SetTarget(Transform target)
    {

        targetTrans = target;
        myMesh.enabled = true;
        isSelected = true;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void ResetTarget()
    {

        isSelected = false;
        myMesh.enabled = false;
        targetTrans = null;
    }
}
