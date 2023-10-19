using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCase : MonoBehaviour
{


    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Vector3 powOffset;     // 튕기는 파워
    [SerializeField] private Vector3 rotOffset;     // 튕기는 회전력

    [SerializeField] private ushort prefabIdx;
    protected short poolIdx = -1;

    public short PoolIdx
    {

        get
        {

            if (poolIdx == -1)
            {

                poolIdx = PoolManager.instance.ChkIdx(prefabIdx);
            }

            return poolIdx;
        }
    }

    /// <summary>
    /// 시작에서만 한다
    /// </summary>
    public void Init()
    {

        Quaternion forward = Quaternion.LookRotation(transform.forward);

        myRigid.AddForce(forward * powOffset, ForceMode.Impulse);
        myRigid.AddTorque(forward * rotOffset, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {

        myRigid.velocity = Vector3.zero;
        PoolManager.instance.UsedPrefab(gameObject, PoolIdx);
    }
}