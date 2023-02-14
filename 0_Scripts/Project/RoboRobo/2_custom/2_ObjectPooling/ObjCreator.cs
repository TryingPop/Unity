using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjCreator : MonoBehaviour
{

    // 생성할 대상 프리팹의 AutoAttack 스크립트
    public AutoAttack obj;

    // 저장할 풀
    public ObjectPool<AutoAttack> objPool;

    [SerializeField] private int maxNum;    // 풀 최대 수

    private void Awake()
    {

        // 풀 설정
        objPool = new ObjectPool<AutoAttack>
            (

            // 생성 시 적용시킬 함수
            createFunc: () =>
            {

                // 대상 생성 및 풀에 추가
                var createObj = Instantiate(original: obj, parent: EnemySpawner.instance.poolTrans);
                createObj.poolToReturn = objPool;

                return createObj;
            },

            // 풀에 비활성화 대상 있으면 실행 시킬 함수 OnEnable 대신 여기에 추가해도 된다
            actionOnGet: (poolObj) =>
            {

                poolObj.gameObject.SetActive(true);
            },

            // 풀 용량에 여유분이 있고 유닛 사망 시 실행할 함수
            actionOnRelease: (poolObj) =>
            {

                poolObj.gameObject.SetActive(false);
                
            },

            // 풀 용량이 초과할 경우 유닛 사망 시 실행할 함수
            actionOnDestroy: (poolObj) =>
            {

                Destroy(poolObj.gameObject);
            },

            // 풀의 맥스 사이즈
            maxSize: maxNum
            );
    }

    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="position"></param>
    public void CreateObj(Vector3 position)
    {
        
        // 살아있는 유닛 수가 풀에 최대 수 보다 적은 경우만 생성
        if (objPool.CountActive < maxNum)
        {
     
            var obj = objPool.Get();

            obj.transform.position = position;
        }
    }
}
