using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animation myAnim;

    [SerializeField] private List<string> stateList;

    private void Awake()
    {
        
        if (myAnim == null) { myAnim = GetComponent<Animation>(); }
        if (stateList.Count == 0) { stateList = new List<string>() { "0_idle", "1_walk", "2_attack", "3_attacked" }; }

        for (int i = 0; i < stateList.Count; i++)
        {

            myAnim[stateList[i]].layer = i;
        }

        myAnim.CrossFade(stateList[0], 0.1f);
    }

    /// <summary>
    /// 행동 실행
    /// 행동 번호 : 대기, 걷기, 공격, 피격
    /// </summary>
    /// <param name="i">행동 번호</param>
    /// <param name="start">행동 여부</param>
    public void ChkAnimation(int i, bool start = true)
    {

        // 갖고 있는 애니메이션보다 많은 경우면 종료
        if (i >= stateList.Count)
        {

            return;
        }

        // 번호에 맞는 행동 시작 
        if (start)
        {

            myAnim.CrossFade(stateList[i], 0.1f);
        }
        // 번호에 맞는 행동 종료
        // 문제 : 공격 중이면 막대기 내려간 상태로 이동한다
        else
        {

            myAnim.Stop(stateList[i]);
        }
    }
}
