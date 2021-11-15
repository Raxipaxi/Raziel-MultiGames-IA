using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
   
    private Transform _target;

    private INode _root;
    private ObstacleAvoidance _behaviour;
   
    private Action _onChase;
    private float _timeToAttemptAttack;
    private float _counter;
    private Action<Vector3> _onMove;
    private Action <bool> _setIdleCommand;
    public EnemyChaseState(Transform target, INode root, ObstacleAvoidance behaviour,Action onChase, float timeToAttemptAttack, Action<Vector3> onMove, Action<bool> setIdleCommand)
    {
        _root = root;
        _behaviour = behaviour;
        _onChase = onChase;
        _timeToAttemptAttack = timeToAttemptAttack;
        _onMove = onMove;
        _target = target;
        _setIdleCommand = setIdleCommand;
    }

    private void ResetCounter()
    {
        _counter = _timeToAttemptAttack;
    }
    public override void Awake()
    {
        _behaviour.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Pursuit);
        _behaviour.SetNewTarget(_target);
        _onChase?.Invoke();
        _setIdleCommand?.Invoke(false);
        ResetCounter();
    }

    public override void Execute()
    {
        var dir = _behaviour.GetDir();
        _onMove?.Invoke(dir);
        
        _counter -= Time.deltaTime;
        
        Debug.LogError("ChaseState");
        
        if (_counter > 0) return;
        
        _root.Execute();
        _setIdleCommand?.Invoke(false);
        ResetCounter();
    }
    
    
}
