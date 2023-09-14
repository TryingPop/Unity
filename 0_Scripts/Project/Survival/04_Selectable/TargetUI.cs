using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : MonoBehaviour
{

    private Transform target;

    private ParticleSystem myParticle;


    public Transform Target 
    { 
        
        set { target = value; }
        get { return target; }
    }

    
    private void Awake()
    {

        myParticle = GetComponent<ParticleSystem>();

    }

    /// <summary>
    /// 사이즈 조절
    /// </summary>
    /// <param name="_size"></param>
    public void SetSize(int _size)
    {

        {

            var shape = myParticle.shape;
            shape.scale = new Vector3(_size, _size, 1);
        }

        {

            var emission = myParticle.emission;
            emission.rateOverTime = 15 * _size;
        }
    }

    /// <summary>
    /// 유닛 배치
    /// </summary>
    public void Batch()
    {

        if (target == null) return;
        transform.position = target.position;
    }
}