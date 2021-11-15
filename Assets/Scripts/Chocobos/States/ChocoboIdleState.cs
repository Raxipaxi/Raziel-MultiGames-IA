using System;
using UnityEngine;


public class ChocoboIdleState<T> : State<T>
{
    private float _counterToFollow;
    private float _counter;
    private INode _root;
    private Action _idleCommand;
    private Action<Transform> _onFollow;
    private Transform _playerTransform;
    private Func<bool> _checkForNearPlayer;


    public ChocoboIdleState(float counterToFollow, Action idleCommand,Action<Transform> onFollow, INode root,Transform playerTransform,Func<bool> checkForNearPlayer)
    {
       
        _counterToFollow = counterToFollow;
        _root = root;
        _idleCommand = idleCommand;
        _onFollow = onFollow;
        _playerTransform = playerTransform;
        _checkForNearPlayer = checkForNearPlayer;
    }
    public override void Awake()
    {
        _onFollow?.Invoke(null);
        _idleCommand?.Invoke();
        ResetCounter();
    }

    public override void Execute()
    {

        var leaderIsNear = _checkForNearPlayer.Invoke();
        if (!leaderIsNear) return;

        _counter -= Time.deltaTime;

        if (_counter > 0) return;

        _onFollow?.Invoke(_playerTransform);
        _root.Execute();
        ResetCounter();
    }

    void ResetCounter()
    {
        _counter = _counterToFollow;
    }
    
    
}
