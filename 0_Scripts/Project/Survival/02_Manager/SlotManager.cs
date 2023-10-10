using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{

    [SerializeField] protected GameObject unitSlotObj;

    [SerializeField] protected RectTransform slotUIRectTrans;

    protected List<RectTransform> slots;                            // 크기가 변하므로 리스트로!

    [SerializeField] protected Vector2 spacing;

    [Tooltip("x : left, y : right, z : top, w : bottom")]
    [SerializeField] protected Vector4 padding;

    protected Vector2 uiSize;
    protected Vector2 slotSize;

    protected List<Selectable> units;

    [SerializeField] protected GameObject nextBtn;
    [SerializeField] protected GameObject prevBtn;

    protected byte curPage;
    protected byte maxPage;

    /// <summary>
    /// 0 : maxRow, 1 : maxColumn
    /// </summary>
    protected int[] matrixSize = new int[2];  



    private void Start()
    {

        slots = new List<RectTransform>();
        units = InputManager.instance.curGroup.Get();

        SetCalcSize();
        SetMaxSlots();
        BatchSlot();

        if (curPage < maxPage) NextPage(0);
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            SetCalcSize();
            SetMaxSlots();
            BatchSlot();
            if (curPage < maxPage) NextPage(0);
        }
    }

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

        curPage = 0;
        maxPage = (byte)((units.Count - 1) / (row + column));
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetMaxSlots()
    {

        int num = matrixSize[0] * matrixSize[1];
        if (num > VariableManager.MAX_SELECT) num = VariableManager.MAX_SELECT;

        // 생성과 파괴부분 수정할 필요가 있다!
        if (num < slots.Count)
        {

            for (int i = slots.Count - 1; i >= num ; i--)
            {

                var go = slots[i].gameObject;
                slots.RemoveAt(i);
                Destroy(go);
            }
        }
        else
        {

            for (int i = slots.Count; i < num; i++)
            {

                var go = Instantiate(unitSlotObj, slotUIRectTrans);
                slots.Add(go.GetComponent<RectTransform>());
            }
        }
    }

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
                slots[j + (i * matrixSize[0])].anchoredPosition = new Vector2(posX, posY);
            }
        }
    }


    private void ActiveSlot(int _slotIdx, int _unitIdx, bool _active)
    {

        slots[_slotIdx].gameObject.SetActive(_active);
    }

    private void SetBtn(int _num)
    {

        if (_num >= maxPage)
        {

            _num = maxPage;
            nextBtn.SetActive(false);
        }
        else
        {

            nextBtn.SetActive(true);
        }


        if (_num <= 0)
        {

            _num = 0;
            prevBtn.SetActive(false);
        }
        else
        {

            prevBtn.SetActive(true);
        }

        curPage = (byte)_num;
    }

    public void NextPage(int _step)
    {

        int page = curPage + _step;
        SetBtn(page);

        int len = matrixSize[0] * matrixSize[1];
        for (int i = 0; i < len; i++)
        {

            if (i >= slots.Count) break;
            int unitIdx = (len * curPage) + i;
            if (units.Count > unitIdx)
            {

                ActiveSlot(i, unitIdx, true);
            }
            else
            {

                ActiveSlot(i, unitIdx, false);
            }
        }
    }
}



// 해당 사이트 참고해서 스크롤 뷰 풀링 하는거 고려해보기!
// https://wonjuri.tistory.com/entry/Unity-UI-%EC%9E%AC%EC%82%AC%EC%9A%A9-%EC%8A%A4%ED%81%AC%EB%A1%A4%EB%B7%B0-%EC%A0%9C%EC%9E%91