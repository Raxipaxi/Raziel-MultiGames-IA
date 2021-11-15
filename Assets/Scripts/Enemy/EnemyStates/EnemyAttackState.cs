using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : State<T>
{
    private INode _root;
    private Action _onAttack;
    private float _timeToOutOfAttack;
    private Action<bool> _setIdleState;
    private float _counter;
    public EnemyAttackState(INode root, Action onAttack, float timeToOutOfAttack, Action<bool> setIdleState)
    {
        _root = root;
        _onAttack = onAttack;
        _timeToOutOfAttack = timeToOutOfAttack;
        _setIdleState = setIdleState;
    }

    public override void Awake()
    {
        _onAttack?.Invoke();
        _setIdleState?.Invoke(false);
    }

    private void ResetCounter()
    {
        _counter = _timeToOutOfAttack;
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime;
        
      

        if (_counter > 0) return;
        
        _setIdleState?.Invoke(false);
        ResetCounter();
        _root.Execute();

    }
}
