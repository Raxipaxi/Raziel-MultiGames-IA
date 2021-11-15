using System;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private INode _root;
    private Func<bool> _attemptSeePlayer;
    private float _idleLenght;
    private float _cooldown;
    private Action<bool> _setIdleCommand;
    private float _counter;
    private Action _onIdle;

    public EnemyIdleState(float idleLenght,Func<bool> attemptSeePlayer,Action onIdle, INode root, Action <bool> setIdleCommand)
    {
        _root = root;
        _attemptSeePlayer = attemptSeePlayer;
        _idleLenght = idleLenght;
        _onIdle = onIdle;
        _setIdleCommand = setIdleCommand;
    }
    public override void Awake()
    {
        ResetCd();
        _onIdle?.Invoke();
        _setIdleCommand?.Invoke(true);
    }
    public override void Execute()
    {
        _counter -= Time.deltaTime;
        var seePlayer = _attemptSeePlayer.Invoke();

        if (_counter <= 0 || seePlayer)
        {
            _setIdleCommand?.Invoke(false);
            _root.Execute();
            ResetCd();
        }
    }

    private void ResetCd()
    {
        _counter = _idleLenght;
    }
}
