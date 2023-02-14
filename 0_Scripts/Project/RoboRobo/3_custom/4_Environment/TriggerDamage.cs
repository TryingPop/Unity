using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{

    [SerializeField] private int dmg;
    private static List<GameObject> objs = new List<GameObject>();


    private void OnTriggerStay(Collider other)
    {

        // 플레이어나 적이 아니면 탈출
        if (other.tag != "Player" && other.tag != "Enemy") return;

        // 대상이 리스트에 없는 경우
        if (!objs.Contains(other.gameObject))
        {

            // 대상을 리스트에 넣는다
            objs.Add(other.gameObject);
            // 데미지 주기
            other.gameObject.GetComponent<Stat>()?.OnDamaged(dmg);
            // 1초 타이머 체크
            StartCoroutine(Timer(other.gameObject));
        }
    }

    /// <summary>
    /// 1초 후에 탈출
    /// </summary>
    /// <param name="obj">리스트에 제거할 대상</param>
    /// <returns></returns>
    private IEnumerator Timer(GameObject obj)
    {

        yield return new WaitForSeconds(1f);
        objs.Remove(obj);
    }
}
