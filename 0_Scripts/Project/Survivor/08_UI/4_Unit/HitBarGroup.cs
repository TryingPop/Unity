using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBarGroup : MonoBehaviour
{


    [SerializeField] private HitBar prefabHitBar;

    private List<HitBar> hitBars = new List<HitBar>(VarianceManager.INIT_UNIT_LIST_NUM
            + VarianceManager.INIT_BUILDING_LIST_NUM
            + VarianceManager.INIT_NEUTRAL_LIST_NUM);

    private Stack<HitBar> usedHitBars = new Stack<HitBar>(VarianceManager.INIT_UNIT_LIST_NUM
            + VarianceManager.INIT_BUILDING_LIST_NUM
            + VarianceManager.INIT_NEUTRAL_LIST_NUM);


    /// <summary>
    /// 체력 바 생성
    /// </summary>
    public HitBar GetHitBar()
    {

        if (!usedHitBars.TryPop(out HitBar hitbar))
        {

            var go = Instantiate(prefabHitBar, this.transform);
            hitbar = go.GetComponent<HitBar>();
        }

        hitBars.Add(hitbar);

        return hitbar;
    }

    /// <summary>
    /// 체력바 종료
    /// </summary>
    public void UsedHitBar(HitBar _hitBar)
    {

        hitBars.Remove(_hitBar);
        usedHitBars.Push(_hitBar);
        _hitBar.Used();
    }

    /// <summary>
    /// 좌표 배치
    /// </summary>
    public void SetPos()
    {

        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetPos();
        }
    }

    /// <summary>
    /// 현재는 모든 유닛들의 체력바의 max 값 수정
    /// 뒤에 hitbars가 구분되면 추후에는 그룹만 실행되게 수정해야한다!
    /// </summary>
    public void SetMaxHp()
    {

        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetMaxHp();
        }
    }
}
