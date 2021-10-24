using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evasion : ISteering
{
    private Transform _self;
    private Transform _target;
    private IVel _vel;
    private float _timePrediction;
    public Evasion(Transform self, Transform target, IVel vel, float timePrediction)
    {
        _self = self;
        _target = target;
        _vel = vel;
        _timePrediction = timePrediction;
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
        float directionMultiplier = (_vel.Vel * _timePrediction);
        float distance = Vector3.Distance(_target.position, _self.position);

        if (directionMultiplier >= distance)
        {
            directionMultiplier = distance / 2;
        }
        Vector3 finitPos = _target.position + _target.forward * directionMultiplier;
        Vector3 dir = (_self.position - finitPos).normalized;
        return dir;
    }
}
