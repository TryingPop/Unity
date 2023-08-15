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
        selectedUIs = new TargetUI[SelectedGroup.MAX_SELECT];

        for (int i = 0; i < selectedUIs.Length; i++)
        {

            selectedUIs[i] = Instantiate(selectedUIobj, transform).GetComponent<TargetUI>();
            selectedUIs[i].gameObject.SetActive(false);
        }
    }

    public void SetTargets(List<Selectable> targets)
    {


        selectedNums = targets.Count;
        for (int i = 0; i < targets.Count; i++)
        {

            selectedUIs[i].Target = targets[i].transform;
            selectedUIs[i].SetSize(targets[i].GetComponent<Selectable>().MySize);
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

            selectedUIs[i].Batch();
        }
    }
}