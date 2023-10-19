using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 활성화
/// </summary>
public class ButtonSlots : MonoBehaviour
{

    [SerializeField] private Image[] btnImgs;
    [SerializeField] private GameObject[] btns;
    [SerializeField] private RectTransform[] btnRectTrans;

    /// <summary>
    /// 핸들러의 버튼 만큼 활성화
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
    /// 버튼 간격 설정 메서드 화면 크기 바꿔도 버튼칸은 크기 고정이므로 직접 값 대입
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
