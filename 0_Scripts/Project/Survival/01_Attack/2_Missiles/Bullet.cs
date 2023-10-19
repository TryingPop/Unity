using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private TrailRenderer myTrail;     // 이동을 표현

    private void OnEnable()
    {

        myTrail.Clear();                                // 재활용 될 때 어색한 효과 없애기 위해 만든 스크립트
        myTrail.enabled = true;
    }

    private void OnDisable()
    {

        myTrail.enabled = false;
    }
}
