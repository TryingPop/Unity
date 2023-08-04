using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{

    public SelectedTable select;

    private Camera cam;

    [SerializeField] private FollowUI[] follows;




    private Vector3 startPos;
    private Vector3 endPos;

    private bool isDrag;

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

            isDrag = true;
            SetVec();

            Select();
            SetFollowUI();
        }
        if (Input.GetMouseButtonUp(0))
        {

            isDrag = false;
            SetVec();

            MultiSelect();
            SetFollowUI();
        }

        if (Input.GetMouseButtonDown(1))
        {

            MoveUnits();
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


            Debug.Log($"selcted Units : {select.GetSize()}");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            int nums = select.GetSize();
            bool isRun = false;

            if (nums > 0)
            {

                isRun = select.IsRun;
            }
            foreach(var unit in select.Get())
            {

                unit.SetRun(isRun);
            }
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

    /// <summary>
    /// �巡�� ���
    /// </summary>
    private void MultiSelect()
    {

        if (endPos.y == float.PositiveInfinity || endPos.y == float.NegativeInfinity || Vector3.Distance(cam.WorldToScreenPoint(startPos), cam.WorldToScreenPoint(endPos)) < 10f) return;

        Vector3 center = (startPos + endPos) / 2f;
        Vector3 halfExtents = new Vector3(Mathf.Abs(center.x - startPos.x), 15f, Mathf.Abs(center.z - startPos.z));

        // ������ �ʾҴٸ� Ŭ����
        if (!Input.GetKey(KeyCode.LeftShift))
        {

            select.Clear();
        }

        var hits = Physics.BoxCastAll(center, halfExtents, Vector3.up, Quaternion.identity, 0f, LayerMask.GetMask("Player"));


        foreach (RaycastHit hit in hits)
        {

            
            var chr = hit.transform.gameObject.GetComponent<Character>();
            select.Select(chr);
        }
    }


    /// <summary>
    /// ���õ� ���� �����ֱ�
    /// </summary>
    private void SetFollowUI()
    {

        var arr = select.Get();

        if (arr != null)
        {

            // ���õ� �ֵ鸸 UI Ȱ��ȭ
            for (int i = 0; i < arr.Count; i++)
            {

                follows[i].SetTarget(arr[i].transform);
            }

            for (int i = arr.Count; i < follows.Length; i++)
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

    /// <summary>
    /// ���õ� ���ֵ� �̵�!
    /// </summary>
    private void MoveUnits()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Ground")))
        {

            var units = select.Get();
            for (int i = 0; i < units.Count; i++)
            {

                units[i].SetDestination(hit.point);
            }
        }
    }

    /// <summary>
    /// ���콺 Ŭ�� �������� �������� ����
    /// </summary>
    private void SetVec()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (isDrag)
        {

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Ground")))
            {

                startPos = hit.point;
            }
            else
            {

                startPos = Vector3.positiveInfinity;
            }

        }
        else
        {
            
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Ground")))
            {

                endPos = hit.point;
            }
            else
            {

                endPos = Vector3.positiveInfinity;
            }
        }
    }


    private void OnGUI()
    {
        
        if (isDrag 
            && startPos.y != float.PositiveInfinity && startPos.y != float.NegativeInfinity 
            && endPos.y != float.PositiveInfinity && endPos.y != float.NegativeInfinity)
        {

            Vector3 p1 = cam.WorldToScreenPoint(startPos);

            Rect rect = FollowUI.GetScreenRect(p1, Input.mousePosition);
            FollowUI.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            FollowUI.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}

// https://www.youtube.com/watch?v=vAVi04mzeKk ���� ��������!
// �غ��� ������ ��Ƽ� �Ѵ�...