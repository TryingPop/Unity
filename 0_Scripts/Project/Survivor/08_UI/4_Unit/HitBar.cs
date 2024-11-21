using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 유닛 체력바 UI
/// </summary>
[RequireComponent(typeof(Slider)),
    RequireComponent(typeof(RectTransform)),
    RequireComponent(typeof(Image[]))]
public class HitBar : MonoBehaviour
{
    
    [SerializeField] private Slider mySlider;               // 유닛 남은 체력 슬라이더
    [SerializeField] private GameEntity target;             // 타겟
    [SerializeField] private Transform targetTrans;         // 따라갈 타겟
    [SerializeField] private RectTransform myTrans;         // 나의 위치
    [SerializeField] private Image[] myImgs;                // 해제 시 안보일 이미지
    private Vector3 offset;                                 // 초기 캐릭으로부터 얼만큼 떨어져 있을건지


    /// <summary>
    /// 유닛으로 이동, LateUpdate에서 이루어진다
    /// </summary>
    public void SetPos()
    {

        myTrans.position = targetTrans.position + offset;
    }

    /// <summary>
    /// 초기화, 마찬가지로 풀링
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
    /// Hp 조절
    /// </summary>
    public void SetHp()
    {

        mySlider.value = target.CurHp;
    }

    /// <summary>
    /// 사용 되면 이미지 안보이기
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
    /// 최대 Hp 설정
    /// </summary>
    public void SetMaxHp()
    {

        mySlider.maxValue = target.MaxHp;
    }
}
