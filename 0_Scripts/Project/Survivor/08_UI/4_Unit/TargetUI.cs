using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 따라다니는 낱개
/// </summary>
public class TargetUI : MonoBehaviour
{

    [SerializeField] private Transform target;
    private BaseObj follow;
    private ParticleSystem myParticle;


    private void Awake()
    {

        myParticle = GetComponent<ParticleSystem>();
    }


    public void Init(BaseObj _follow)
    {

        follow = _follow;
        SetSize(follow.MyStat.MySize);
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
    public void SetPos()
    {

        if (follow == null) return;
        transform.position = follow.transform.position;

        if (follow.Target) target.position = follow.Target.transform.position;
        else target.position = follow.TargetPos;
    }
}