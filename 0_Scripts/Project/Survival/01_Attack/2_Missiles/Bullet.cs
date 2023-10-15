using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private TrailRenderer myTrail;

    private void OnEnable()
    {

        myTrail.Clear();
        myTrail.enabled = true;
    }

    private void OnDisable()
    {

        myTrail.enabled = false;
    }
}
