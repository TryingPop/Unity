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

    /// <summary>
    /// ȭ�鿡 �簢�� ����� ! ��, �巡��
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
    /// �׸��� ���Ǵ� �ؽ���2D
    /// </summary>
    private static Texture2D whiteTexture;


    /// <summary>
    /// �߾� �κ� �׸���
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
    /// �׵θ� �׸���
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
