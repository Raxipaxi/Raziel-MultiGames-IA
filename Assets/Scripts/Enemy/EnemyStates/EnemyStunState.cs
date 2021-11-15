

using System;
using UnityEngine;

public class EnemyStunState<T> : State<T>
{

    private Action<bool> _setStunned;
    private float _counter;
    private float _timeToOutOfStun;
    private Action _onStunned;
    private INode _root;
    private Action _onFinishStun;
    public EnemyStunState(Action<bool> setStunned, float timeToOutOfStun, Action onStunned, INode root, Action onFinishStun)
    {
        _setStunned = setStunned;
        _timeToOutOfStun = timeToOutOfStun;
        _onStunned = onStunned;
        _root = root;
        _onFinishStun = onFinishStun;
    }

    private void ResetCounter()
    {
        _counter = _timeToOutOfStun;
    }

    public override void Awake()
    {
        _onStunned?.Invoke();
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime;
        
        if (_counter > 0) return;
        
        _setStunned?.Invoke(false);
        _onFinishStun?.Invoke();
        ResetCounter();
        _root.Execute();
    }

    public override void Sleep()
    {
        _setStunned?.Invoke(false);
    }
}