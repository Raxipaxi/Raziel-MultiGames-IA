using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrIGoToSpotState<T> : State<T>
{
    private INode _root;
    
    private Action<Vector3> _onMove;
    private Func<Node,List<Node>> _getWaypoints;
    private ObstacleAvoidance _obstacleAvoidance;
    private Action _onEnterFastCheck;
    private Func<bool> _checkPlayerAlive;
    private Func<Node> _getClosestNodeToPlayer;

    private List<Node> _waypoints;
    private float _minimumWaypointDistance;
    private MRIModel _model;
    private int _waypointIndex;
    public MrIGoToSpotState(INode root, Action<Vector3> onMove, Func<Node, List<Node>> getWaypoints,
        ObstacleAvoidance obstacleAvoidance, Action onEnterFastCheck, Func<bool> checkPlayerAlive, float minimumWaypointDistance, MRIModel model)
    {
        _root = root;
        _onMove = onMove;
        _getWaypoints = getWaypoints;
        _obstacleAvoidance = obstacleAvoidance;
        _onEnterFastCheck = onEnterFastCheck;
        _checkPlayerAlive = checkPlayerAlive;
        _minimumWaypointDistance = minimumWaypointDistance;
        _model = model;
    }

    public override void Awake()
    {
        _onEnterFastCheck?.Invoke();
        var node = _getClosestNodeToPlayer.Invoke();
        _waypoints = _getWaypoints.Invoke(node);
        _waypointIndex = 0;
        _obstacleAvoidance.SetNewTarget(_waypoints[_waypointIndex].transform);
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
    }

    private bool NextWaypoint()
    {
        _waypointIndex++;
        if (_waypointIndex >= _waypoints.Count)
        {
            _waypointIndex = 0;
            return false;
        }

        var nextPatrolSpot = _waypoints[_waypointIndex];
        _obstacleAvoidance.SetNewTarget(nextPatrolSpot.transform);
        return true;
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

        var distance = Vector3.Distance(_model.transform.position, _waypoints[_waypointIndex].transform.position);

        if (distance > _minimumWaypointDistance) return;

        var waypointAvailable = NextWaypoint();

        if (waypointAvailable) return;
        
        _root.Execute();

    }
}
