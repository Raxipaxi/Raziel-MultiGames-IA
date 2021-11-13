using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlertState <T> : State<T>
{
    private LineOfSightAI _lineOfSightAI;
    private PlayerModel _target;
    private INode _root;
    private float _counter;
    private float _counterLimit;
    private Action<Vector3> _alert;


    public CameraAlertState(LineOfSightAI lOSComponent, PlayerModel target, INode root, float counterLimit,
        Action<Vector3> alert)
    {
        _lineOfSightAI = lOSComponent;
        _target = target;
        _root = root;
        _counterLimit = counterLimit;
        _alert = alert;
    }

    private void ResetCounter()
    {
        _counter = _counterLimit;
    }
    public override void Awake()
    {
        _alert?.Invoke(_target.transform.position);
        ResetCounter();
    }

  
    public override void Execute()
    {

        _counter -= Time.deltaTime;

        if (_counter > 0) return;

        ResetCounter();
        var seePlayer = _lineOfSightAI.SingleTargetInSight(_target.transform);

        if (!seePlayer)
        {
            _root.Execute();
            return;
        }
        _alert?.Invoke(_target.transform.position);
    }
}
