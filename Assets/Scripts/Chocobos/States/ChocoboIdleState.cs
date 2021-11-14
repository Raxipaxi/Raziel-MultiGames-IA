using System;
using UnityEngine;


public class ChocoboIdleState<T> : State<T>
{
    private LineOfSightAI _lineOfSightAI;
    private float _counterToFollow;
    private float _counter;
    private INode _root;
    private Action _idleCommand;
    
    public ChocoboIdleState(LineOfSightAI lineOfSightAI, float counterToFollow, Action idleCommand, INode root )
    {
        _lineOfSightAI = lineOfSightAI;
        _counterToFollow = counterToFollow;
        _root = root;
        _idleCommand = idleCommand;
    }
    public override void Awake()
    {
        ResetCounter();
    }

    public override void Execute()
    {
        
        
        
        if (_counter <= 0)
        {
            _root.Execute();
            ResetCounter();
            return;
        }
        
    }

    void ResetCounter()
    {
        _counter = _counterToFollow;
    }
    
    
}
