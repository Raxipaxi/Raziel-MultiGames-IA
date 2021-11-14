using System;
using UnityEngine;

public class MrIChaseState<T> : State<T>
{
    private Func<bool> _attemptSeePlayer;
    private INode _root;
    private Action<bool> _setIdleCooldown;
    private Action<Vector3> _onMove;
    private ObstacleAvoidance _obstacleAvoidance;
    private float _timeToCheckOnPlayerSight;
    private float _counter;
    private Action _onEnterChase;
    private PlayerModel _target;
    private Func<bool> _checkPlayerAlive;

    public MrIChaseState(Func<bool> attemptSeePlayer, INode root, Action<bool> setIdleCooldown, Action<Vector3> onMove,
        ObstacleAvoidance obstacleAvoidance, float timeToCheckOnPlayerSight, Action onEnterChase, PlayerModel target, Func<bool> checkPlayerAlive)
    {
        _attemptSeePlayer = attemptSeePlayer;
        _root = root;
        _setIdleCooldown = setIdleCooldown;
        _onMove = onMove;
        _obstacleAvoidance = obstacleAvoidance;
        _timeToCheckOnPlayerSight = timeToCheckOnPlayerSight;
        _onEnterChase = onEnterChase;
        _target = target;
        _checkPlayerAlive = checkPlayerAlive;
    }

    public override void Awake()
    {
        //Changes speed mainly
        _onEnterChase?.Invoke();
        ResetCounter();
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Pursuit);
        _obstacleAvoidance.SetNewTarget(_target.transform);
    }

    private void ResetCounter()
    {
        _counter = _timeToCheckOnPlayerSight;
    }

    public override void Execute()
    {
        if (!_checkPlayerAlive.Invoke())
        {
            _root.Execute();
            return;
        }
        
        var dir = _obstacleAvoidance.GetDir();
        _onMove?.Invoke(dir);
        _counter -= Time.deltaTime;

        if (_counter <= 0)
        {
            ResetCounter();
            _root.Execute();
        }
    }
}
