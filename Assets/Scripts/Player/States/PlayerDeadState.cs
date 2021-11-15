using System;
using JetBrains.Annotations;
using UnityEngine;


public class PlayerDeadState<T> : State<T>
{
    //Works as an interrumpt to other FSM states

    private float _timeToRevive;
    private float _counter;
    private T _transitionToIdle;
    private Action<Vector3> _onMove;
    public PlayerDeadState(float timeToRevive, T transitionToIdle, Action<Vector3> onMove)
    {
        _timeToRevive = timeToRevive;
        _transitionToIdle = transitionToIdle;
        _onMove = onMove;
    }

    private void ResetTimer()
    {
        _counter = _timeToRevive;
    }

    public override void Awake()
    {
        ResetTimer();  
        _onMove?.Invoke(Vector3.zero);
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime;

        if (_counter <= 0)
        {
            parentFSM.Transition(_transitionToIdle);
            ResetTimer();
        }
    }
}