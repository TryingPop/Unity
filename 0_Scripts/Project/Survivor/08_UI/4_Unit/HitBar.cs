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
    [SerializeField] private GameEntity target;             // Ÿ��
    [SerializeField] private Transform targetTrans;         // ���� Ÿ��
    [SerializeField] private RectTransform myTrans;         // ���� ��ġ
    [SerializeField] private Image[] myImgs;                // ���� �� �Ⱥ��� �̹���
    private Vector3 offset;                                 // �ʱ� ĳ�����κ��� ��ŭ ������ ��������


    /// <summary>
    /// �������� �̵�, LateUpdate���� �̷������
    /// </summary>
    public void SetPos()
    {

        myTrans.position = targetTrans.position + offset;
    }

    /// <summary>
    /// �ʱ�ȭ, ���������� Ǯ��
    /// </summary>
    public void Init(GameEntity _target, int _maxHp, int _ups)
    {

        myImgs[0].enabled = true;
        myImgs[1].enabled = true;
        target = _target;
        targetTrans = _target.transform;
        SetMaxHp();
        offset = Vector3.up * _ups;
    }

    /// <summary>
    /// Hp ����
    /// </summary>
    public void SetHp()
    {

        mySlider.value = target.CurHp;
    }

    /// <summary>
    /// ��� �Ǹ� �̹��� �Ⱥ��̱�
    /// </summary>
    public void Used()
    {

        target = null;
        targetTrans = null;
        mySlider.value = 0;
        myImgs[0].enabled = false;
        myImgs[1].enabled = false;
    }

    /// <summary>
    /// �ִ� Hp ����
    /// </summary>
    public void SetMaxHp()
    {

        mySlider.maxValue = target.MaxHp;
    }
}
