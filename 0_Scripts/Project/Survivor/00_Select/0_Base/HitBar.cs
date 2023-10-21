using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ���� ü�¹� UI
/// </summary>
[RequireComponent(typeof(Slider)),
    RequireComponent(typeof(RectTransform)),
    RequireComponent(typeof(Image[]))]
public class HitBar : MonoBehaviour
{

    
    [SerializeField] private Slider mySlider;               // ���� ���� ü�� �����̴�
    [SerializeField] private Transform target;              // ���� Ÿ��
    [SerializeField] private RectTransform myTrans;         // ���� ��ġ
    [SerializeField] private Image[] myImgs;                // ���� �� �Ⱥ��� �̹���
    private Vector3 offset;                                 // �ʱ� ĳ�����κ��� ��ŭ ������ ��������


    /// <summary>
    /// �������� �̵�, LateUpdate���� �̷������
    /// </summary>
    public void SetPos()
    {

        myTrans.position = target.position + offset;
    }

    /// <summary>
    /// �ʱ�ȭ, ���������� Ǯ��
    /// </summary>
    public void Init(Transform _target, int _maxHp, int _size)
    {

        myImgs[0].enabled = true;
        myImgs[1].enabled = true;
        target = _target;
        SetMaxHp(_maxHp);
        offset = Vector3.up * (2f + _size);
    }

    /// <summary>
    /// Hp ����
    /// </summary>
    public void SetHp(int _curHp)
    {

        mySlider.value = _curHp;
    }

    /// <summary>
    /// ��� �Ǹ� �̹��� �Ⱥ��̱�
    /// </summary>
    public void Used()
    {

        target = null;
        myImgs[0].enabled = false;
        myImgs[1].enabled = false;
    }

    /// <summary>
    /// �ִ� Hp ����
    /// </summary>
    public void SetMaxHp(int _maxHp)
    {

        mySlider.maxValue = _maxHp;
    }
}
