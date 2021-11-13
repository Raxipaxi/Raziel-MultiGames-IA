using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSurveilanceState<T> : State<T>
{
  
    private LineOfSightAI _lineOfSightAI;

    private IVel _playerIvel;
    private PlayerModel _target;

    private float _velocityThreshold;

    private float _counter;

    private float _counterLimit;

    private INode _root;
    //private Action


    public CameraSurveilanceState(LineOfSightAI lOSComponent, IVel playerIvel, float velocityThreshold, float counterLimit, PlayerModel target, INode root)
    {
        _lineOfSightAI = lOSComponent;
        _playerIvel = playerIvel;
        _velocityThreshold = velocityThreshold;
        _counterLimit = counterLimit;
        _target = target;
        _root = root;
    }
    public override void Awake()
    {
        ResetCounter();
    }

    public override void Execute()
    {
        var seePlayer = _lineOfSightAI.SingleTargetInSight(_target.transform);

        if (!seePlayer) return;

        if (_playerIvel.Vel >= _velocityThreshold)
        {
            _root.Execute();
            return;
        }

        if (_playerIvel.Vel != 0)
        {
            _counter -= Time.deltaTime;
            if (_counter <= 0)
            {
                _root.Execute();
                ResetCounter();
            }
        }
    }

    private void ResetCounter()
    {
        _counter = _counterLimit;
    }
}
