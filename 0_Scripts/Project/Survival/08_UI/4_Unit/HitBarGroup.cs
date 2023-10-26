using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBarGroup : MonoBehaviour
{


    [SerializeField] private HitBar prefabHitBar;

    private List<HitBar> hitBars = new List<HitBar>(VariableManager.INIT_UNIT_LIST_NUM
            + VariableManager.INIT_BUILDING_LIST_NUM
            + VariableManager.INIT_NEUTRAL_LIST_NUM);

    private Stack<HitBar> usedHitBars = new Stack<HitBar>(VariableManager.INIT_UNIT_LIST_NUM
            + VariableManager.INIT_BUILDING_LIST_NUM
            + VariableManager.INIT_NEUTRAL_LIST_NUM);


    /// <summary>
    /// ü�� �� ����
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
    /// ü�¹� ����
    /// </summary>
    public void UsedHitBar(HitBar _hitBar)
    {

        hitBars.Remove(_hitBar);
        usedHitBars.Push(_hitBar);
        _hitBar.Used();
    }

    /// <summary>
    /// ��ǥ ��ġ
    /// </summary>
    public void SetPos()
    {

        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetPos();
        }
    }
}
