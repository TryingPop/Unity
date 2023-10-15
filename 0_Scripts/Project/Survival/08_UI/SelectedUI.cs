using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUI : MonoBehaviour
{

    [SerializeField] protected GameObject selectedUIobj;
    protected TargetUI[] selectedUIs;

    protected int selectedNums;

    private void Awake()
    {

        selectedNums = 0;

        selectedUIs = new TargetUI[VariableManager.MAX_SELECT];

        for (int i = 0; i < VariableManager.MAX_SELECT; i++)
        {

            // 유닛이 선택됨을 알리는 UI 생성
            selectedUIs[i] = Instantiate(selectedUIobj, transform).GetComponent<TargetUI>();
            selectedUIs[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 해당 유닛들에게 선택되었다는 UI 배치
    /// </summary>
    /// <param name="targets">선택된 그룹</param>
    public void SetTargets(List<Selectable> targets)
    {

        selectedNums = targets.Count;
        for (int i = 0; i < targets.Count; i++)
        {

            selectedUIs[i].Init(targets[i]);
            selectedUIs[i].gameObject.SetActive(true);
        }

        for (int i = targets.Count; i < selectedUIs.Length; i++)
        {

            selectedUIs[i].gameObject.SetActive(false);
        }
    }

    public void LateUpdate()
    {
        
        for (int i = 0; i < selectedNums; i++)
        {

            selectedUIs[i].SetPos();
        }
    }
}