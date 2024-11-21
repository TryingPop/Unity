using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 선택 유닛을 알려주는 ui ( 파티클이다!)
/// </summary>
public class SelectedUI : MonoBehaviour
{

    [SerializeField] private GameObject selectedUIobj;
    private TargetUI[] selectedUIs;


    private List<GameEntity> curGroup;
    public List<GameEntity> CurGroup { set { curGroup = value; } }

    private int selectedNums;

    private void Awake()
    {

        selectedNums = 0;

        selectedUIs = new TargetUI[VarianceManager.MAX_SELECT];

        for (int i = 0; i < VarianceManager.MAX_SELECT; i++)
        {

            // 유닛이 선택됨을 알리는 UI 생성
            selectedUIs[i] = Instantiate(selectedUIobj, transform).GetComponent<TargetUI>();
            selectedUIs[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 해당 유닛들에게 선택되었다는 UI 배치
    /// </summary>
    public void ResetGroup()
    {

        selectedNums = curGroup.Count;
        for (int i = 0; i < curGroup.Count; i++)
        {

            selectedUIs[i].Init(curGroup[i]);
            selectedUIs[i].gameObject.SetActive(true);
        }

        for (int i = curGroup.Count; i < selectedUIs.Length; i++)
        {

            selectedUIs[i].gameObject.SetActive(false);
        }
    }

    public void SetPos()
    {
        
        for (int i = 0; i < selectedNums; i++)
        {

            selectedUIs[i].SetPos();
        }
    }
}