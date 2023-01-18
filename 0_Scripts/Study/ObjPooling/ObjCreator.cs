using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjCreator : MonoBehaviour
{

    // ������ ������ 
    public PoolingObj objPrefab;

    // ������ Ǯ
    public ObjectPool<PoolingObj> ObjPool;

    // ������ �������� �θ� ������Ʈ
    public Transform PoolObjs;

    // �ڵ� ���� Ȱ��ȭ ��Ȱ��ȭ ������
    private bool isActive;
    
    // �ڷ�ƾ �������� �����Ҳ��̱⿡ �����ϰ� ����� ���� �ڷ�ƾ �޼ҵ�
    private IEnumerator method;


    private void Start()
    {

        // �ʱ�ȭ
        ObjPool = new ObjectPool<PoolingObj>
            (
            
            // ���� �� �θ� ������Ʈ ������ ���� �� ������ Ǯ�� ��´�
            createFunc: () =>
            {

                // return Instantiate(objPrefab);
                var createObj = Instantiate(original:objPrefab, parent:PoolObjs); ;
                createObj.poolToReturn = ObjPool;

                return createObj;
                
            },

            // Ȱ��ȭ �Ǹ� �����ϴ� �޼ҵ� Ȱ��ȭ �� ������ ���� �ʱ�ȭ
            actionOnGet: (poolObj) =>
            {

                poolObj.gameObject.SetActive(true);
                poolObj.Reset();
            },

            // Release�� �����ϴ� �޼ҵ�
            actionOnRelease: (poolObj) =>
            {

                poolObj.gameObject.SetActive(false);
            },

            // maxSize ������ ���� �����տ� ����Ǵ� �޼ҵ�
            actionOnDestroy: (poolObj) =>
            {

                Destroy(poolObj.gameObject);
            }, 

            // �ִ� ������
            maxSize: 100);

        // �޼ҵ� ���
        method = CreateObjs();
    }

    private void Update()
    {

        // ���콺 ���� ��ư ������ ���Ƿ� ����
        if (Input.GetMouseButtonDown(0))
        {

            // 5 ~ 19 �� ���� ����
            int count = Random.Range(5, 20);

            for (int i = 0; i <count; i++)
            {

                CreateObj();
            }
        }

        // A Ű ������ �ڵ� ���� ������ 0.5�� �ѹ��� 5 ~ 19��
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
    /// ������ ����
    /// </summary>
    private void CreateObj()
    {

        // ȸ�� ���� ���� 
        Vector3 position = Random.insideUnitSphere + new Vector3(x: 0, y: 5, z: 0);
        Quaternion rotation = Random.rotation;

        // Instantiate(objPrefab, position, rotation);
        
        // �갡 ����
        PoolingObj obj = ObjPool.Get();

        // ��ǥ�� ȸ�� ����
        obj.transform.position = position;
        obj.transform.rotation = rotation;

    }

    /// <summary>
    /// �ڵ� ����
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
