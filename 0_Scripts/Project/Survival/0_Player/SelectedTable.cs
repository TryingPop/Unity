using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectedTable
{

    /// <summary>
    /// 선택된 유닛      일단 보여주기!
    /// </summary>
    private HashSet<Character> selectedChr;
    
    
    /// <summary>
    /// 한번에 부대지정 가능한 최대 인원
    /// </summary>
    private static readonly int MAX_UNIT = 12;

    /// <summary>
    /// 생성자
    /// </summary>
    public SelectedTable()
    {

        selectedChr = new HashSet<Character>(MAX_UNIT);
    }

    /// <summary>
    /// 선택된 유닛들 비우기
    /// </summary>
    public void Clear()
    {

        selectedChr.Clear();
    }

    /// <summary>
    /// 최대 유닛수를 안넘을 경우 추가한다
    /// 기존에 있는 경우 제거
    /// </summary>
    /// <param name="unit"></param>
    public void Select(Character unit)
    {

        if (unit == null) return;
        if (selectedChr.Count < MAX_UNIT)
        {

            selectedChr.Add(unit);
            Debug.Log($"Add {unit.GetInstanceID()}");
        }
        else
        {

            Debug.Log("Group is fulled!");
        }
    }

    public void DeSelect(Character unit)
    {

        selectedChr.Remove(unit);
    }

    public bool IsContains(Character unit)
    {

        return selectedChr.Contains(unit);
    }

    /// <summary>
    /// 현재 선택된 유닛들 반환
    /// 없는 경우 널 반환
    /// </summary>
    /// <returns></returns>
    public Character[] Get()
    {

        if (selectedChr.Count == 0) return null;

        return selectedChr.ToArray();
    }

    /// <summary>
    /// 테스트용 몇 마리 있는지 확인
    /// </summary>
    public void ShowSize()
    {

        Debug.Log($"Size : {selectedChr.Count}");
    }
}