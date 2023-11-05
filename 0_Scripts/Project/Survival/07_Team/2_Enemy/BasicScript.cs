using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ϴ� Ÿ�ֿ̹� ��縦 �����ϰ� ���ִ� Ŭ����
/// </summary>
public class BasicScript : MonoBehaviour
{

    [SerializeField] protected Script[] scripts;
    protected IEnumerator StartTalking()
    {

        // �� ������ ���� ���� �����Ӻ��� �����Ѵ�!
        yield return null;

        // �⺻ ���� ������ 1�ʸ� ����!
        for (int i = 0; i < scripts.Length; i++)
        {

            UIManager.instance.SetScript(scripts[i]);

            // ���
            if (scripts[i].NextTime == 2f) yield return VarianceManager.BASE_WAITFORSECONDS;
            else yield return new WaitForSeconds(scripts[i].NextTime);
        }
    }

    public void Talk()
    {

        StartCoroutine(StartTalking());
    }
}