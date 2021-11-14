using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrIIdleState<T> : State<T>
{
    private Action _onStateEnter;
    private Action _onStateStay;
    private Action<bool> _setIdleCooldown;

    private Func<bool> _attemptSeePlayer;
    private INode _root;
    private float _counter;
    private float _timeToOutOfIdle;

    public MrIIdleState(Action onStateEnter, Action onStateStay, Func<bool> attemptSeePlayer, INode root, Action<bool> setIdleCooldown)
    {
        _onStateEnter = onStateEnter;
        _onStateStay = onStateStay;
        _attemptSeePlayer = attemptSeePlayer;
        _root = root;
        _setIdleCooldown = setIdleCooldown;
    }

    public override void Awake()
    {
        _onStateEnter.Invoke();
        _setIdleCooldown.Invoke(true);
        ResetTimer();
    }

    private void ResetTimer()
    {
        _counter = _timeToOutOfIdle;
    }
    public override void Execute()
    {
        _onStateStay.Invoke();
        
        _counter -= Time.deltaTime;
        if (_counter <= 0)
        {
            _root.Execute();
            _setIdleCooldown.Invoke(false);
            ResetTimer();
            return;
        }

        if (_attemptSeePlayer.Invoke()) _root.Execute();
    }

    public override void Sleep()
    {
        _setIdleCooldown.Invoke(false);
    }
}
