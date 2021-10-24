using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : ISteering
{
    private Transform _self;
    private Transform _target;

    public Flee(Transform self, Transform target)
    {
        _self = self;
        _target = target;
    }
    public Transform SetSelf
    {
        set
        {
            _self = value;
        }
    }

    public Transform SetTarget
    {
        set
        {
            _target = value;
        }
    }
    public Vector3 GetDir()
    {
        //A : Target
        //B: Self

        return (_self.position - _target.position).normalized;
    }

}
