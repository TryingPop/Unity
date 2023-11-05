using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 원하는 타이밍에 대사를 실행하게 해주는 클래스
/// </summary>
public class BasicScript : MonoBehaviour
{

    [SerializeField] protected Script[] scripts;
    protected IEnumerator StartTalking()
    {

        // 한 프레임 쉬고 다음 프레임부터 시작한다!
        yield return null;

        // 기본 글자 간격은 1초를 기준!
        for (int i = 0; i < scripts.Length; i++)
        {

            UIManager.instance.SetScript(scripts[i]);

            // 대기
            if (scripts[i].NextTime == 2f) yield return VarianceManager.BASE_WAITFORSECONDS;
            else yield return new WaitForSeconds(scripts[i].NextTime);
        }
    }

    public void Talk()
    {

        StartCoroutine(StartTalking());
    }
}