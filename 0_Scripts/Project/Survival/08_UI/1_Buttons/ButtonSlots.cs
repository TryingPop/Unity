using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư Ȱ��ȭ
/// </summary>
public class ButtonSlots : MonoBehaviour
{

    [SerializeField] private Image[] btnImgs;
    [SerializeField] private GameObject[] btns;
    [SerializeField] private RectTransform[] btnRectTrans;

    /// <summary>
    /// �ڵ鷯�� ��ư ��ŭ Ȱ��ȭ
    /// </summary>
    public void Init(ButtonHandler _handler)
    {


        if (_handler == null)
        {

            for (int i = 0; i < btnImgs.Length; i++)
            {

                btns[i].SetActive(false);
            }

            return;
        }


        for (int i = 0; i < btnImgs.Length; i++)
        {

            int idx = _handler.Idxs[i];
            if (idx != -1)
            {

                btns[i].SetActive(true);
                btnImgs[i].sprite = _handler.actions[idx].BtnSprite;
                SetPos(i, idx);
            }
            else
            {

                btns[i].SetActive(false);
            }
        }
    }


    /// <summary>
    /// ��ư ���� ���� �޼��� ȭ�� ũ�� �ٲ㵵 ��ưĭ�� ũ�� �����̹Ƿ� ���� �� ����
    /// </summary>
    private void SetPos(int _i, int _idx)
    {

        Vector2 pos;
        if (_idx <= 3)
        {

            pos.x = 30 + (_idx * 50);
            pos.y = -30;
        }
        else
        {

            pos.x = -170 + (_idx * 50);
            pos.y = -80;
        }

        btnRectTrans[_i].anchoredPosition = pos;
    }
}
