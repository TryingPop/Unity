using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class UnitSlots : MonoBehaviour
{

    [SerializeField] protected GameObject unitSlotObj;

    [SerializeField] protected RectTransform slotUIRectTrans;

    protected List<Slot> slots;                            // ũ�Ⱑ ���ϹǷ� ����Ʈ��!

    [SerializeField] protected Vector2 spacing;

    [Tooltip("x : left, y : right, z : top, w : bottom")]
    [SerializeField] protected Vector4 padding;

    protected Vector2 uiSize;
    protected Vector2 slotSize;

    // protected List<Selectable> units;

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
    public void Init(List<Selectable> _curGroup)
    {

        ChkMaxPage(_curGroup);
        NextPage(_curGroup, int.MinValue);
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
        if (num > VariableManager.MAX_SELECT) num = VariableManager.MAX_SELECT;

        // ������ �ı��κ� ������ �ʿ䰡 �ִ�!
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

    private void ChkMaxPage(List<Selectable> _curGroup)
    {

        maxPage = (byte)((_curGroup.Count - 1) / (matrixSize[0] + matrixSize[1]));
    }

    /// <summary>
    /// ���� ���������� step��ŭ ������ �ѱ��
    /// </summary>
    public void NextPage(List<Selectable> _curGroup, int _step)
    {

        int page = curPage + _step;
        ChkBtn(page);

        int len = matrixSize[0] * matrixSize[1];
        for (int i = 0; i < len; i++)
        {

            if (i >= slots.Count) break;
            int unitIdx = (len * curPage) + i;
            if (_curGroup.Count > unitIdx)
            {

                ActiveSlot(i, _curGroup[unitIdx], true);
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
    private void ActiveSlot(int _slotIdx, Selectable _selectable, bool _active)
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
            nextBtn.SetActive(false);
            prevBtn.SetActive(false);
            return;
        }


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
}



// �ش� ����Ʈ �����ؼ� ��ũ�� �� Ǯ�� �ϴ°� ����غ���!
// https://wonjuri.tistory.com/entry/Unity-UI-%EC%9E%AC%EC%82%AC%EC%9A%A9-%EC%8A%A4%ED%81%AC%EB%A1%A4%EB%B7%B0-%EC%A0%9C%EC%9E%91