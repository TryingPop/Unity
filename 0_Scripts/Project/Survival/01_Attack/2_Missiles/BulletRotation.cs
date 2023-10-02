using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotation : MonoBehaviour
{

    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Vector3 powOffset;
    [SerializeField] private Vector3 rotOffset;

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


    public void Init()
    {

        Quaternion forward = Quaternion.LookRotation(transform.forward);
        Debug.Log(forward * powOffset);
        // 방향과 세기 조절... 필요!
        myRigid.AddForce(forward * powOffset, ForceMode.Impulse);
        myRigid.AddTorque(forward * rotOffset, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {

        myRigid.velocity = Vector3.zero;
        PoolManager.instance.UsedPrefab(gameObject, PoolIdx);
    }
}