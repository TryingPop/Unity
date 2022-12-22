using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjCreator : MonoBehaviour
{

    // 생성할 프리팹 
    public PoolingObj objPrefab;

    // 보관할 풀
    public ObjectPool<PoolingObj> ObjPool;

    // 생성된 프리팹의 부모 오브젝트
    public Transform PoolObjs;

    // 자동 생성 활성화 비활성화 변수와
    private bool isActive;
    
    // 코루틴 연산으로 시작할꺼이기에 시작하고 멈출걸 담을 코루틴 메소드
    private IEnumerator method;


    private void Start()
    {

        // 초기화
        ObjPool = new ObjectPool<PoolingObj>
            (
            
            // 생성 시 부모 오브젝트 하위로 생성 및 보관할 풀에 담는다
            createFunc: () =>
            {

                // return Instantiate(objPrefab);
                var createObj = Instantiate(original:objPrefab, parent:PoolObjs); ;
                createObj.poolToReturn = ObjPool;

                return createObj;
                
            },

            // 활성화 되면 실행하는 메소드 활성화 및 프리팹 상태 초기화
            actionOnGet: (poolObj) =>
            {

                poolObj.gameObject.SetActive(true);
                poolObj.Reset();
            },

            // Release시 실행하는 메소드
            actionOnRelease: (poolObj) =>
            {

                poolObj.gameObject.SetActive(false);
            },

            // maxSize 넘으면 이후 프리팹에 실행되는 메소드
            actionOnDestroy: (poolObj) =>
            {

                Destroy(poolObj.gameObject);
            }, 

            // 최대 사이즈
            maxSize: 100);

        // 메소드 담기
        method = CreateObjs();
    }

    private void Update()
    {

        // 마우스 왼쪽 버튼 누르면 임의로 생성
        if (Input.GetMouseButtonDown(0))
        {

            // 5 ~ 19 개 랜덤 생성
            int count = Random.Range(5, 20);

            for (int i = 0; i <count; i++)
            {

                CreateObj();
            }
        }

        // A 키 누르면 자동 생성 간격은 0.5초 한번에 5 ~ 19개
        if (Input.GetKeyDown(KeyCode.A))
        {

            isActive = !isActive;
            
            if (isActive)
            {

                StartCoroutine(method);
            }
            else
            {
                StopCoroutine(method);
            }
           
        }
    }

    /// <summary>
    /// 프리팹 생성
    /// </summary>
    private void CreateObj()
    {

        // 회전 랜덤 설정 
        Vector3 position = Random.insideUnitSphere + new Vector3(x: 0, y: 5, z: 0);
        Quaternion rotation = Random.rotation;

        // Instantiate(objPrefab, position, rotation);
        
        // 얘가 생성
        PoolingObj obj = ObjPool.Get();

        // 좌표랑 회전 적용
        obj.transform.position = position;
        obj.transform.rotation = rotation;

    }

    /// <summary>
    /// 자동 생성
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateObjs()
    {

        while (true)
        {

            int count = Random.Range(5, 20);

            for (int i = 0; i < count; i++)
            {

                CreateObj();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
