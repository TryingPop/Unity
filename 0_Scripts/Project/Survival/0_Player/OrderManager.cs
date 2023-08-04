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

        // 마우스 클릭
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
    /// 유닛 선택
    /// </summary>
    private void Select()
    {


        // 레이를 쏜다
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // 대상이 있는 경우
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask("Player")))
        {

            // leftShift를 눌러야 추가로 넣기 가능
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
    /// 드래그 기능
    /// </summary>
    private void MultiSelect()
    {

        if (endPos.y == float.PositiveInfinity || endPos.y == float.NegativeInfinity || Vector3.Distance(cam.WorldToScreenPoint(startPos), cam.WorldToScreenPoint(endPos)) < 10f) return;

        Vector3 center = (startPos + endPos) / 2f;
        Vector3 halfExtents = new Vector3(Mathf.Abs(center.x - startPos.x), 15f, Mathf.Abs(center.z - startPos.z));

        // 누르지 않았다면 클리어
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
    /// 선택된 유닛 보여주기
    /// </summary>
    private void SetFollowUI()
    {

        var arr = select.Get();

        if (arr != null)
        {

            // 선택된 애들만 UI 활성화
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

            // UI 해제
            for (int i = 0; i < follows.Length; i++)
            {

                follows[i].ResetTarget();
            }
        }
    }

    /// <summary>
    /// 선택된 유닛들 이동!
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
    /// 마우스 클릭 시작지점 종료지점 설정
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

// https://www.youtube.com/watch?v=vAVi04mzeKk 여기 참고하자!
// 해보니 유닛을 담아서 한다...