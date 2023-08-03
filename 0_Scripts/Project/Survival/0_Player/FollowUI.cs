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
    /// 유닛 선택
    /// </summary>
    /// <param name="target">대상이 되는 유닛</param>
    public void SetTarget(Transform target)
    {

        targetTrans = target;
        myMesh.enabled = true;
        isSelected = true;
    }

    /// <summary>
    /// 유닛 해제
    /// </summary>
    public void ResetTarget()
    {

        isSelected = false;
        myMesh.enabled = false;
        targetTrans = null;
    }

    /// <summary>
    /// 화면에 사각형 만들기 ! 즉, 드래그
    /// </summary>
    /// <param name="screenPosition1"></param>
    /// <param name="screenPosition2"></param>
    /// <returns></returns>
    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {

        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    /// <summary>
    /// 그림에 사용되는 텍스쳐2D
    /// </summary>
    private static Texture2D whiteTexture;


    /// <summary>
    /// 중앙 부분 그리기
    /// </summary>
    public static void DrawScreenRect(Rect rect, Color color)
    {

        if (whiteTexture == null)
        {

            whiteTexture = new Texture2D(1, 1);
            whiteTexture.SetPixel(0, 0, Color.white);
            whiteTexture.Apply();
        }

        GUI.color = color;
        GUI.DrawTexture(rect, whiteTexture);
        GUI.color = Color.white;
    }


    /// <summary>
    /// 테두리 그리기
    /// </summary>
    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {

        // Top
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);

        // Bottom
        DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);

        // Left
        DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);

        // Right
        DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }
}
