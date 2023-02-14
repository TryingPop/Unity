using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{

    [SerializeField] private int dmg;
    private static List<GameObject> objs = new List<GameObject>();


    private void OnTriggerStay(Collider other)
    {

        // �÷��̾ ���� �ƴϸ� Ż��
        if (other.tag != "Player" && other.tag != "Enemy") return;

        // ����� ����Ʈ�� ���� ���
        if (!objs.Contains(other.gameObject))
        {

            // ����� ����Ʈ�� �ִ´�
            objs.Add(other.gameObject);
            // ������ �ֱ�
            other.gameObject.GetComponent<Stat>()?.OnDamaged(dmg);
            // 1�� Ÿ�̸� üũ
            StartCoroutine(Timer(other.gameObject));
        }
    }

    /// <summary>
    /// 1�� �Ŀ� Ż��
    /// </summary>
    /// <param name="obj">����Ʈ�� ������ ���</param>
    /// <returns></returns>
    private IEnumerator Timer(GameObject obj)
    {

        yield return new WaitForSeconds(1f);
        objs.Remove(obj);
    }
}
