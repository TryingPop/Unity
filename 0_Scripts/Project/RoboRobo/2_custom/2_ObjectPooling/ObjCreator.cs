using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjCreator : MonoBehaviour
{

    // ������ ��� �������� AutoAttack ��ũ��Ʈ
    public AutoAttack obj;

    // ������ Ǯ
    public ObjectPool<AutoAttack> objPool;

    [SerializeField] private int maxNum;    // Ǯ �ִ� ��

    private void Awake()
    {

        // Ǯ ����
        objPool = new ObjectPool<AutoAttack>
            (

            // ���� �� �����ų �Լ�
            createFunc: () =>
            {

                // ��� ���� �� Ǯ�� �߰�
                var createObj = Instantiate(original: obj, parent: EnemySpawner.instance.poolTrans);
                createObj.poolToReturn = objPool;

                return createObj;
            },

            // Ǯ�� ��Ȱ��ȭ ��� ������ ���� ��ų �Լ� OnEnable ��� ���⿡ �߰��ص� �ȴ�
            actionOnGet: (poolObj) =>
            {

                poolObj.gameObject.SetActive(true);
            },

            // Ǯ �뷮�� �������� �ְ� ���� ��� �� ������ �Լ�
            actionOnRelease: (poolObj) =>
            {

                poolObj.gameObject.SetActive(false);
                
            },

            // Ǯ �뷮�� �ʰ��� ��� ���� ��� �� ������ �Լ�
            actionOnDestroy: (poolObj) =>
            {

                Destroy(poolObj.gameObject);
            },

            // Ǯ�� �ƽ� ������
            maxSize: maxNum
            );
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="position"></param>
    public void CreateObj(Vector3 position)
    {
        
        // ����ִ� ���� ���� Ǯ�� �ִ� �� ���� ���� ��츸 ����
        if (objPool.CountActive < maxNum)
        {
     
            var obj = objPool.Get();

            obj.transform.position = position;
        }
    }
}
