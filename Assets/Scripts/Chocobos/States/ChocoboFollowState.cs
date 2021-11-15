using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChocoboFollowState<T> : State<T>
{
    private INode _root;
    private Action<Transform> _onFollow;
    private float _followCheckTime;
    private float _counter;
    private Func<bool> _checkForPlayer;
    private Action _onStartFollow;
    private Transform _player;

    public ChocoboFollowState(Action<Transform> onFollow,float followCheckTime, INode root, Func<bool> checkForPlayer, Action onStartFollow, Transform player)
    {
        _onFollow = onFollow;
        _root = root;
        _followCheckTime = followCheckTime;
        _checkForPlayer = checkForPlayer;
        _onStartFollow = onStartFollow;
        _player = player;
    }

    public override void Awake()
    {
        _onStartFollow?.Invoke();
    }

    public override void Execute()
    {
        
        _onFollow?.Invoke(_player);
        
        _counter -= Time.deltaTime;

        if (_counter > 0) return;
        
        ResetCounter();
        var playerIsNear = _checkForPlayer.Invoke();

        if (playerIsNear) return;
        
        _root.Execute();

    }

    private void ResetCounter()
    {
        _counter = _followCheckTime;
    }
}
