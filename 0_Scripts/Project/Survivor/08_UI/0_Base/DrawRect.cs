using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 화면에 드래그할 때 영역 그리는 용도
/// OnGui아니면 에러 뜬다!
/// </summary>
public class DrawRect
{

    public static Color borderColor = new Color(0.8f, 0.8f, 0.95f);
    public static Color color = new Color(0.8f, 0.8f, 0.95f, 0.25f);

    protected static Texture2D whiteTexture;
    public static Texture2D WhiteTexture
    {

        get
        {

            if (whiteTexture == null)
            {

                whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
            }

            return whiteTexture;
        }
    }


    public static void DrawDragScreenRect(Vector3 _screenPos1, Vector3 _screenPos2)
    {

        Rect rect = GetScreenRect(_screenPos1, _screenPos2);
        DrawScreenRect(rect, color);
        DrawScreenRectBorder(rect, 2, borderColor);
    }

    protected static Rect GetScreenRect(Vector3 _screenPos1, Vector3 _screenPos2)
    {

        _screenPos1.y = Screen.height - _screenPos1.y;
        _screenPos2.y = Screen.height - _screenPos2.y;

        Vector3 topLeft = Vector3.Min(_screenPos1, _screenPos2);
        Vector3 bottomRight = Vector3.Max(_screenPos1, _screenPos2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    protected static void DrawScreenRect(Rect _rect, Color _color)
    {

        GUI.color = _color;
        GUI.DrawTexture(_rect, WhiteTexture);
        GUI.color = Color.white;
    }

    protected static void DrawScreenRectBorder(Rect _rect, float _thickness, Color color)
    {

        // 상하 좌우
        DrawScreenRect(new Rect(_rect.xMin, _rect.yMin, _rect.width, _thickness), color);
        DrawScreenRect(new Rect(_rect.xMin, _rect.yMax - _thickness, _rect.width, _thickness), color);
        DrawScreenRect(new Rect(_rect.xMin, _rect.yMin, _thickness, _rect.height), color);
        DrawScreenRect(new Rect(_rect.xMax - _thickness, _rect.yMin, _thickness, _rect.height), color);
    }
}
