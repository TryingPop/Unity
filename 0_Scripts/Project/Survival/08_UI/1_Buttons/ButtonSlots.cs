using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư Ȱ��ȭ
/// </summary>
public class ButtonSlots : MonoBehaviour
{

    // [SerializeField] private Image[] btnImgs;
    // [SerializeField] private GameObject[] btns;
    // [SerializeField] private RectTransform[] btnRectTrans;
    [SerializeField] private UIBtn[] btns;

    /// <summary>
    /// �ڵ鷯�� ��ư ��ŭ Ȱ��ȭ
    /// </summary>
    public void Init(ButtonHandler _handler)
    {


        if (_handler == null)
        {

            for (int i = 0; i < btns.Length; i++)
            {

                btns[i].gameObject.SetActive(false);
            }

            return;
        }


        for (int i = 0; i < btns.Length; i++)
        {

            int idx = _handler.GetIdx(btns[i].Key);
            if (idx != -1)
            {

                btns[i].gameObject.SetActive(true);
                ButtonInfo info = _handler.actions[idx];

                btns[i].Init(info, SetPos(idx));
            }
            else
            {

                btns[i].gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// ��ư ���� ���� �޼��� ȭ�� ũ�� �ٲ㵵 ��ưĭ�� ũ�� �����̹Ƿ� ���� �� ����
    /// </summary>
    private Vector2 SetPos(int _idx)
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

        return pos;
    }
}
