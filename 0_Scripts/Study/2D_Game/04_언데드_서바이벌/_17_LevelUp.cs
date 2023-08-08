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

        // 1. 모든 아이템 비활성화
        foreach (_14_Item item in items)
        {

            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템 활성화
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


            // 3. 만렙인 아이템은 소비 아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {

                // 랜덤 아이템 시작 번호부터 
                items[Random.Range(4, items.Length)].gameObject.SetActive(true);
            }
            else
            {

                // 만렙이 아닌 경우
                ranItem.gameObject.SetActive(true);
            }
        }

    }
}
