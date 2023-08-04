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
    private List<Character> selectedChr;

    private bool isRun;
    /// <summary>
    /// 달리기 상태 반환
    /// </summary>
    public bool IsRun { get
        {

            isRun = !isRun;
            return isRun;
        } }


    /// <summary>
    /// 한번에 부대지정 가능한 최대 인원
    /// </summary>
    private static readonly int MAX_UNIT = 12;

    /// <summary>
    /// 생성자
    /// </summary>
    public SelectedTable()
    {

        selectedChr = new List<Character>(MAX_UNIT);
    }

    /// <summary>
    /// 선택된 유닛들 비우기
    /// </summary>
    public void Clear()
    {

        selectedChr.Clear();
        isRun = true;
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
            
            isRun = unit.isRun && isRun;
            selectedChr.Add(unit);
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
    public List<Character> Get()
    {

        return selectedChr;
    }

    /// <summary>
    /// 테스트용 몇 마리 있는지 확인
    /// </summary>
    public int GetSize()
    {

        return selectedChr.Count;
    }

    /*
    // 이 방법은 좋지 않은거 같다
    // 너무 중구난방으로 튄다
    // 이 경우 destination을 방향으로 바꿔야 한다
    /// <summary>
    /// 선택된 전체 n마리 중 n / 2 번째 몬스터의 위치를 나타낸다
    /// </summary>
    /// <returns></returns>
    public Vector3 midPos()
    {

        return selectedChr.ToArray()[selectedChr.Count / 2].transform.position;
    }
    */
}