using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UI;

public class UnitSlots : MonoBehaviour
{

    [SerializeField] protected GameObject unitSlotObj;

    [SerializeField] protected RectTransform slotUIRectTrans;

    protected List<Slot> slots;                             // 크기가 변하므로 리스트로!

    [SerializeField] protected Vector2 spacing;             // 공간

    [Tooltip("x : left, y : right, z : top, w : bottom")]
    [SerializeField] protected Vector4 padding;             // 초기 띄울 간격

    protected Vector2 uiSize;                               
    protected Vector2 slotSize;

    // protected List<Selectable> units;

    // 유닛 슬롯 
    [SerializeField] protected Image nextBtn;
    [SerializeField] protected Image prevBtn;
    [SerializeField] protected Text curPageText;

    protected int activeSlotNum;      // 활성화된 슬롯 개수
    protected int curPage;
    protected int maxPage;

    /// <summary>
    /// 0 : maxRow, 1 : maxColumn
    /// </summary>
    protected int[] matrixSize = new int[2];

    protected List<BaseObj> curGroup;
    // 현재 선택된 유닛 정보
    public List<BaseObj> CurGroup
    {

        set
        {

            curGroup = value;
        }
    }

    private bool isChange;
    public bool IsChanged 
    { 
    
        get 
        { 
            
            if (isChange)
            {

                isChange = false;
                return true;
            }

            return false; 
        } 
        set { isChange = value; }
    }


    private void Start()
    {

        slots = new List<Slot>();
        SetScreenSize();
    }

    /// <summary>
    /// 화면 크기 조정 시 불러올 함수
    /// </summary>
    public void SetScreenSize()
    {

        SetCalcSize();
        SetMaxSlots();
        BatchSlot();
    }

    /// <summary>
    /// 유닛 재설정 시 실행하는 함수
    /// </summary>
    public void Init()
    {

        ChkMaxPage();
        NextPage(int.MinValue);
    }

    /// <summary>
    /// 연산에 필요한 변수 계산
    /// </summary>
    private void SetCalcSize()
    {

        uiSize = slotUIRectTrans.rect.size;
        slotSize = unitSlotObj.GetComponent<RectTransform>().sizeDelta;

        // spacing + slotSize로 나누면 들어갈 수 있는 숫자로 깔끔하게 만들기 위해 스케일링!
        uiSize.x += spacing.x - (padding.x + padding.y);
        uiSize.y += spacing.y - (padding.z + padding.w);

        int row = Mathf.FloorToInt(uiSize.x / (slotSize.x + spacing.x));
        int column = Mathf.FloorToInt(uiSize.y / (slotSize.y + spacing.y));

        if (row < 1) row = 1;
        if (column < 1) column = 1;

        matrixSize[0] = row;
        matrixSize[1] = column;
    }

    /// <summary>
    /// 사이즈에 맞게 슬롯 수 조정
    /// </summary>
    private void SetMaxSlots()
    {

        int num = matrixSize[0] * matrixSize[1];
        if (num > VarianceManager.MAX_SELECT) num = VarianceManager.MAX_SELECT;

        // 생성과 파괴부분 수정할 필요가 있다!
        if (num < slots.Count)
        {

            // 노는 슬롯들 파괴
            for (int i = slots.Count - 1; i >= num ; i--)
            {

                var go = slots[i].gameObject;
                slots.RemoveAt(i);
                Destroy(go);
            }
        }
        else
        {

            // 부족한 슬롯 생성
            for (int i = slots.Count; i < num; i++)
            {

                var go = Instantiate(unitSlotObj, slotUIRectTrans);
                slots.Add(go.GetComponent<Slot>());
                go.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 슬롯 위치 선정
    /// </summary>
    private void BatchSlot()
    {

        // 라인에 배치할 숫자 구하기
        float halfSlotSizeX = slotSize.x * 0.5f;
        float halfSlotSizeY = slotSize.y * 0.5f;

        float batchX = slotSize.x + spacing.x;
        float batchY = -(slotSize.y + spacing.y);

        for (int i = 0; i < matrixSize[1]; i++)
        {

            for (int j = 0; j < matrixSize[0]; j++)
            {

                float posX = j * (batchX) + padding.x + (halfSlotSizeX);
                float posY = i * (batchY) - padding.z - (halfSlotSizeY);

                if (j + (i * matrixSize[0]) >= slots.Count) break;
                slots[j + (i * matrixSize[0])].myRectTrans.anchoredPosition = new Vector2(posX, posY);
            }
        }
    }

    private void ChkMaxPage()
    {

        maxPage = ((curGroup.Count - 1) / (matrixSize[0] * matrixSize[1]));
    }

    /// <summary>
    /// 현재 페이지에서 step만큼 페이지 넘기기
    /// </summary>
    public void NextPage(int _step)
    {

        int page = curPage + _step;
        ChkBtn(page);

        int len = matrixSize[0] * matrixSize[1];
        activeSlotNum = 0;
        // 버튼에 유닛 할당
        for (int i = 0; i < len; i++)
        {

            if (i >= slots.Count) break;
            int unitIdx = (len * curPage) + i;
            if (curGroup.Count > unitIdx)
            {

                activeSlotNum++;
                ActiveSlot(i, curGroup[unitIdx], true);
            }
            else
            {

                ActiveSlot(i, null, false);
            }
        }
    }


    /// <summary>
    /// 슬롯 활성화 이벤트
    /// </summary>
    private void ActiveSlot(int _slotIdx, BaseObj _selectable, bool _active)
    {

        slots[_slotIdx].gameObject.SetActive(_active);

        if (_active)
        {

            // select 정보 넘겨주기
            slots[_slotIdx].Init(_selectable);
        }
    }

    /// <summary>
    /// 다음 버튼과 이전 버튼 활성화 여부
    /// </summary>
    private void ChkBtn(int _num)
    {

        if (maxPage == 0)
        {

            curPage = 0;
            nextBtn.enabled = false;
            prevBtn.enabled = false;
            if (curGroup.Count != 0) curPageText.text = $"{curGroup.Count}";
            else curPageText.text = "";
            return;
        }


        if (_num >= maxPage)
        {

            _num = maxPage;
            nextBtn.enabled = false;
        }
        else
        {

            nextBtn.enabled = true;
        }


        if (_num <= 0)
        {

            _num = 0;
            prevBtn.enabled = false;
        }
        else
        {

            prevBtn.enabled = true;
        }

        curPage = _num;

        curPageText.enabled = true;
        curPageText.text = $"{curGroup.Count}\n{curPage + 1}/{maxPage + 1}";
    }


    public void SetHp()
    {

        for (int i = 0; i < activeSlotNum; i++)
        {

            slots[i].SetHp();
        }
    }
}



// 해당 사이트 참고해서 스크롤 뷰 풀링 하는거 고려해보기!
// https://wonjuri.tistory.com/entry/Unity-UI-%EC%9E%AC%EC%82%AC%EC%9A%A9-%EC%8A%A4%ED%81%AC%EB%A1%A4%EB%B7%B0-%EC%A0%9C%EC%9E%91