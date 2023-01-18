using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class PoolingObj : MonoBehaviour
{

    // Pool 확인용
    public IObjectPool<PoolingObj> poolToReturn;

    // 트리거
    public bool isTriggered { get; private set; }

    // 부딪히면 3초 뒤 비활성화 되게 설정
    public void Reset()
    {

        isTriggered = false;
    }

    
    private void OnCollisionEnter(Collision collision)
    {

        // 이미 부딪혔을 경우
        if (isTriggered)
        {

            return;
        }

        // Ground Tag와 부딪혔을 경우 3초 뒤 비활성화 실행
        if (collision.collider.CompareTag("Ground"))
        {

            isTriggered = true;
            StartCoroutine(routine: DestroyPoolingObj());
        }
    }

    /// <summary>
    /// maxSize 이하면 비활성화 넘으면 파괴
    /// </summary>
    /// <returns>3초 대기</returns>
    private IEnumerator DestroyPoolingObj()
    {

        yield return new WaitForSeconds(3f);
        // Destroy(gameObject);

        poolToReturn.Release(element:this);
    }
}
