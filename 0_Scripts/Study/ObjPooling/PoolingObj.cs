using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class PoolingObj : MonoBehaviour
{

    // Pool Ȯ�ο�
    public IObjectPool<PoolingObj> poolToReturn;

    // Ʈ����
    public bool isTriggered { get; private set; }

    // �ε����� 3�� �� ��Ȱ��ȭ �ǰ� ����
    public void Reset()
    {

        isTriggered = false;
    }

    
    private void OnCollisionEnter(Collision collision)
    {

        // �̹� �ε����� ���
        if (isTriggered)
        {

            return;
        }

        // Ground Tag�� �ε����� ��� 3�� �� ��Ȱ��ȭ ����
        if (collision.collider.CompareTag("Ground"))
        {

            isTriggered = true;
            StartCoroutine(routine: DestroyPoolingObj());
        }
    }

    /// <summary>
    /// maxSize ���ϸ� ��Ȱ��ȭ ������ �ı�
    /// </summary>
    /// <returns>3�� ���</returns>
    private IEnumerator DestroyPoolingObj()
    {

        yield return new WaitForSeconds(3f);
        // Destroy(gameObject);

        poolToReturn.Release(element:this);
    }
}
