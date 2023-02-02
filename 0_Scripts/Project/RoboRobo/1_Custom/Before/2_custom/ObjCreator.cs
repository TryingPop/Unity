using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjCreator : MonoBehaviour
{

    public AutoAttack obj;

    public ObjectPool<AutoAttack> objPool;

    private void Awake()
    {

        objPool = new ObjectPool<AutoAttack>
            (

            createFunc: () =>
            {

                var createObj = Instantiate(original: obj, parent: EnemySpawner.instance.poolTrans);
                createObj.poolToReturn = objPool;

                return createObj;
            },

            actionOnGet: (poolObj) =>
            {

                poolObj.gameObject.SetActive(true);
            },

            actionOnRelease: (poolObj) =>
            {

                poolObj.gameObject.SetActive(false);
                
            },

            actionOnDestroy: (poolObj) =>
            {

                // ÆÄ±«
                Destroy(poolObj.gameObject);
            },

            maxSize: EnemySpawner.instance.maxNum
            );
    }

    public void CreateObj(Vector3 position)
    {
        
        if (objPool.CountActive < EnemySpawner.instance.maxNum)
        {
     
            var obj = objPool.Get();

            obj.transform.position = position;
        }
    }
}
