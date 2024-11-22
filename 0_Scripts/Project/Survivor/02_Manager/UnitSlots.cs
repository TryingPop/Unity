using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UI;

public class UnitSlots : MonoBehaviour
{

    [SerializeField] protected GameObject unitSlotObj;

    [SerializeField] protected RectTransform slotUIRectTrans;

    protected List<Slot> slots;                             // ũ�Ⱑ ���ϹǷ� ����Ʈ��!

    [SerializeField] protected Vector2 spacing;             // ����

    [Tooltip("x : left, y : right, z : top, w : bottom")]
    [SerializeField] protected Vector4 padding;             // �ʱ� ��� ����

    protected Vector2 uiSize;                               
    protected Vector2 slotSize;

    // protected List<Selectable> units;

    // ���� ���� 
    [SerializeField] protected Image nextBtn;
    [SerializeField] protected Image prevBtn;
    [SerializeField] protected Text curPageText;

    protected int activeSlotNum;      // Ȱ��ȭ�� ���� ����
    protected int curPage;
    protected int maxPage;

    /// <summary>
    /// 0 : maxRow, 1 : maxColumn
    /// </summary>
    protected int[] matrixSize = new int[2];

    protected List<BaseObj> curGroup;
    // ���� ���õ� ���� ����
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
    /// ȭ�� ũ�� ���� �� �ҷ��� �Լ�
    /// </summary>
    public void SetScreenSize()
    {

        SetCalcSize();
        SetMaxSlots();
        BatchSlot();
    }

    /// <summary>
    /// ���� �缳�� �� �����ϴ� �Լ�
    /// </summary>
    public void Init()
    {

        ChkMaxPage();
        NextPage(int.MinValue);
    }

    /// <summary>
    /// ���꿡 �ʿ��� ���� ���
    /// </summary>
    private void SetCalcSize()
    {

        uiSize = slotUIRectTrans.rect.size;
        slotSize = unitSlotObj.GetComponent<RectTransform>().sizeDelta;

        // spacing + slotSize�� ������ �� �� �ִ� ���ڷ� ����ϰ� ����� ���� �����ϸ�!
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
    /// ����� �°� ���� �� ����
    /// </summary>
    private void SetMaxSlots()
    {

        int num = matrixSize[0] * matrixSize[1];
        if (num > VarianceManager.MAX_SELECT) num = VarianceManager.MAX_SELECT;

        // ������ �ı��κ� ������ �ʿ䰡 �ִ�!
        if (num < slots.Count)
        {

            // ��� ���Ե� �ı�
            for (int i = slots.Count - 1; i >= num ; i--)
            {

                var go = slots[i].gameObject;
                slots.RemoveAt(i);
                Destroy(go);
            }
        }
        else
        {

            // ������ ���� ����
            for (int i = slots.Count; i < num; i++)
            {

                var go = Instantiate(unitSlotObj, slotUIRectTrans);
                slots.Add(go.GetComponent<Slot>());
                go.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ���� ��ġ ����
    /// </summary>
    private void BatchSlot()
    {

        // ���ο� ��ġ�� ���� ���ϱ�
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
    /// ���� ���������� step��ŭ ������ �ѱ��
    /// </summary>
    public void NextPage(int _step)
    {

        int page = curPage + _step;
        ChkBtn(page);

        int len = matrixSize[0] * matrixSize[1];
        activeSlotNum = 0;
        // ��ư�� ���� �Ҵ�
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
    /// ���� Ȱ��ȭ �̺�Ʈ
    /// </summary>
    private void ActiveSlot(int _slotIdx, BaseObj _selectable, bool _active)
    {

        slots[_slotIdx].gameObject.SetActive(_active);

        if (_active)
        {

            // select ���� �Ѱ��ֱ�
            slots[_slotIdx].Init(_selectable);
        }
    }

    /// <summary>
    /// ���� ��ư�� ���� ��ư Ȱ��ȭ ����
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



// �ش� ����Ʈ �����ؼ� ��ũ�� �� Ǯ�� �ϴ°� ����غ���!
// https://wonjuri.tistory.com/entry/Unity-UI-%EC%9E%AC%EC%82%AC%EC%9A%A9-%EC%8A%A4%ED%81%AC%EB%A1%A4%EB%B7%B0-%EC%A0%9C%EC%9E%91