using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public SelectedTable select;

    private Camera cam;

    [SerializeField] private FollowUI[] follows;

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

            Select();
            SetFollowUI();
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

    /// <summary>
    /// ���� ����
    /// </summary>
    private void Select()
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

            var chr = hit.transform.gameObject.GetComponent<Character>();

            if (select.IsContains(chr))
            {

                select.DeSelect(chr);
            }
            else
            {

                select.Select(chr);
            }
        }
    }

    private void SetFollowUI()
    {

        var arr = select.Get();

        if (arr != null)
        {

            // ���õ� �ֵ鸸 UI Ȱ��ȭ
            for (int i = 0; i < arr.Length; i++)
            {

                follows[i].SetTarget(arr[i].transform);
            }

            for (int i = arr.Length; i < follows.Length; i++)
            {

                follows[i].ResetTarget();
            }
        }
        else
        {

            // UI ����
            for (int i = 0; i < follows.Length; i++)
            {

                follows[i].ResetTarget();
            }
        }
    }
}


