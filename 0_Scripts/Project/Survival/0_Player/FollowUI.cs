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
    /// ¿Ø¥÷ º±≈√
    /// </summary>
    /// <param name="target">¥ÎªÛ¿Ã µ«¥¬ ¿Ø¥÷</param>
    public void SetTarget(Transform target)
    {

        targetTrans = target;
        myMesh.enabled = true;
        isSelected = true;
    }

    /// <summary>
    /// ¿Ø¥÷ «ÿ¡¶
    /// </summary>
    public void ResetTarget()
    {

        isSelected = false;
        myMesh.enabled = false;
        targetTrans = null;
    }
}
