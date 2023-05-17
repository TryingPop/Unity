using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_ParticleChrDir : MonoBehaviour
{

    Rigidbody2D rootObject;

    void Start()
    {
        rootObject = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {

        float ra = (rootObject.transform.localScale.x < 0) ? +50 : -50;
        transform.transform.localRotation = Quaternion.Euler(270 + ra, -90, 0);
    }
}
