using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("���� ���������� renderer �׸� SortingLayer ID, Order in Layer�� ���������ϴ�", true)]
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
