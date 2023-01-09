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

    public void ChkAnimation(int i, bool start = true)
    {
        if (i >= stateList.Count)
        {

            return;
        }

        if (start)
        {

            myAnim.CrossFade(stateList[i], 0.1f);
        }
        else
        {

            myAnim.Stop(stateList[i]);
        }
    }
}
