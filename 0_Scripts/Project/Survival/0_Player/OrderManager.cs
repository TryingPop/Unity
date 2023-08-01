using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public SelectedTable select;

    private Camera cam;

    private void Awake()
    {
        
        select = new SelectedTable();
        cam = Camera.main;
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            if (!Input.GetKey(KeyCode.LeftShift))
            {

                select.Clear();
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Player")))
            {

                var go = hit.transform.gameObject;
                
                select.Selct(go);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            select.ShowSize();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {

            select.Clear();
        }
    }

}
