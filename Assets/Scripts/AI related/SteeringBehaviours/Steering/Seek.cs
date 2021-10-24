using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : ISteering
{
    private Transform _self;
    private Transform _target;

    public Seek(Transform self, Transform target)
    {
        _self = self;
        _target = target;
    }
    public Vector3 GetDir()
    {
        //A : Self
        //B: Target

        return (_target.position - _self.position).normalized;
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

}
