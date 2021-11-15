using System;
using UnityEngine;


public class ChocoboIdleState<T> : State<T>
{
    private Func<bool> _potentialLeader;
    private float _counterToFollow;
    private float _counter;
    private INode _root;
    private Action _idleCommand;
    
    public ChocoboIdleState(Func<bool> potentialLeader, float counterToFollow, Action idleCommand, INode root )
    {
        _potentialLeader = potentialLeader;
        _counterToFollow = counterToFollow;
        _root = root;
        _idleCommand = idleCommand;
    }
    public override void Awake()
    {
        _idleCommand?.Invoke();
        ResetCounter();
    }

    public override void Execute()
    {

        if (!_potentialLeader())
        {
            if (_counter == _counterToFollow) return;
        
            ResetCounter();
            return;
        }
        _counter -= Time.deltaTime;
        if (_counter <= 0)
        {
            ResetCounter();
            _root.Execute();
        }

    }

    void ResetCounter()
    {
        _counter = _counterToFollow;
    }
    
    
}
