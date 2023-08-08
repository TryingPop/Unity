using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _17_LevelUp : MonoBehaviour
{

    private RectTransform rect;
    private _14_Item[] items;

    private void Awake()
    {
        
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<_14_Item>(true);
    }

    public void Show()
    {

        Next();
        rect.localScale = Vector3.one;
        _3_GameManager.instance.Stop();
        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.LevelUp);
        _21_AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {

        rect.localScale = Vector3.zero;
        _3_GameManager.instance.Resume();
        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Select);
        _21_AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {

        items[index].OnClick();
    }

    private void Next()
    {

        // 1. ��� ������ ��Ȱ��ȭ
        foreach (_14_Item item in items)
        {

            item.gameObject.SetActive(false);
        }

        // 2. �� �߿��� ���� 3�� ������ Ȱ��ȭ
        int[] ran = new int[3];

        while (true)
        {

            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);


            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
            {

                break;
            }
        }

        for (int index = 0; index < ran.Length; index++)
        {

            _14_Item ranItem = items[ran[index]];


            // 3. ������ �������� �Һ� ���������� ��ü
            if (ranItem.level == ranItem.data.damages.Length)
            {

                // ���� ������ ���� ��ȣ���� 
                items[Random.Range(4, items.Length)].gameObject.SetActive(true);
            }
            else
            {

                // ������ �ƴ� ���
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
