using JetBrains.Annotations;
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
        
        // ���콺 Ŭ��
        if (Input.GetMouseButtonDown(0))
        {

            // ���̸� ���
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // ����� �ִ� ���
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Player")))
            {

                // leftShift�� ������ �߰��� �ֱ� ����
                if (!Input.GetKey(KeyCode.LeftShift))
                {

                    select.Clear();
                }

                var go = hit.transform.gameObject;
                
                select.Selct(go);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Ground")))
            {

                var units = select.Get();

                if (units != null)
                {

                    for (int i = 0; i < units.Length; i++)
                    {

                        units[i].SetDestination(hit.point);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

            foreach(var unit in select.Get())
            {

                unit.MoveStop();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            select.ShowSize();
        }
    }

}
