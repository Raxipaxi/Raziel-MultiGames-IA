using System;
using UnityEngine;


public class ChocoboIdleState<T> : State<T>
{
    private Func<bool> _potentialLeader;
    private float _counterToFollow;
    private float _counter;
    private LineOfSightAI _lineOfSightAI;
    private INode _root;
    private Action _idleCommand;
    private Action<Transform> _onFollow;
    
    public ChocoboIdleState(Func<bool> potentialLeader, float counterToFollow, Action idleCommand,Action<Transform> onFollow,LineOfSightAI lineOfSightAI, INode root )
    {
        _potentialLeader = potentialLeader;
        _counterToFollow = counterToFollow;
        _lineOfSightAI = lineOfSightAI;
        _root = root;
        _idleCommand = idleCommand;
        _onFollow = onFollow;
    }
    public override void Awake()
    {
        _onFollow?.Invoke(null);
        _idleCommand?.Invoke();
        _lineOfSightAI.SwapLineOfSightData(_lineOfSightAI.OriginalData);
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
