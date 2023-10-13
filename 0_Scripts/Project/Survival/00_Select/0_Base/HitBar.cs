using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBar : MonoBehaviour
{

    [SerializeField] private Slider mySlider;
    [SerializeField] private Transform target;
    [SerializeField] private RectTransform myTrans;
    [SerializeField] private Image[] myImgs;
    private Vector3 offset;


    public void SetPos()
    {

        myTrans.position = target.position + offset;
    }

    public void Init(Transform _target, int _maxHp, int _size)
    {

        myImgs[0].enabled = true;
        myImgs[1].enabled = true;
        target = _target;
        SetMaxHp(_maxHp);
        offset = Vector3.up * (2f + _size);
    }

    public void SetHp(int _curHp)
    {

        mySlider.value = _curHp;
    }

    public void Used()
    {

        myImgs[0].enabled = false;
        myImgs[1].enabled = false;
    }

    public void SetMaxHp(int _maxHp)
    {

        mySlider.maxValue = _maxHp;
    }
}
