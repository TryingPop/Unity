using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    [Tooltip("�ı� �ð�")]
    [SerializeField]
    private float destroyTime;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
