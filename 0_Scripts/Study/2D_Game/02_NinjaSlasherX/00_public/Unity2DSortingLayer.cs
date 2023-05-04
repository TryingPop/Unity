using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("현재 버전에서는 renderer 항목에 SortingLayer ID, Order in Layer로 수정가능하다", true)]
public class Unity2DSortingLayer : MonoBehaviour
{

    public string sortingLayerName = "Front";
    public int sortingOrder = 0;

    void Awake()
    {

        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingLayerName = sortingLayerName;
        renderer.sortingOrder = sortingOrder;
    }
}
