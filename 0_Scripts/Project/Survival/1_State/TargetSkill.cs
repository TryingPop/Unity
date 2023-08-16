using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : IUnitAction
{

    private static TargetSkill instance;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(this);
        }
    }


    public override void Action(Unit _unit)
    {

        if (_unit.Target == null) return;
        if (!_unit.Target.gameObject.activeSelf
            || _unit.Target.gameObject.layer == IDamagable.LAYER_DEAD) return;

        // if (Vector3.Distance(_unit.transform.position, _unit.Target.transform.position) < _unit.SkillRange(_unit.MyState))
        {

            _unit.MyAgent.ResetPath();
            _unit.transform.LookAt(_unit.Target.position);
            // _unit.OnSkill(_unit.MyState);
        }
        // else
        {

            _unit.MyAgent.destination = _unit.Target.position;
        }
    }

    public override void Changed(Unit _unit)
    {

        if (_unit.Target == null) _unit.ActionDone();
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
