using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{

    [SerializeField] private GameObject m_goMissile = null;
    [SerializeField] private Transform m_tfMissileSpawn = null;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject t_missile = Instantiate(m_goMissile, m_tfMissileSpawn.position, Quaternion.identity);
            t_missile.GetComponent<Rigidbody>().velocity = Vector3.up * 6f;
        }
    }
}
