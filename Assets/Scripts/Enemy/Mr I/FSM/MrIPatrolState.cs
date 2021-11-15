using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrIPatrolState<T> : State<T>
{
  
    private Func<bool> _attemptSeePlayer;
    private INode _root;

    private Func<Node,List<Node>> _getWaypoints;
    private Action<bool> _setIdleCooldown;
    private Action<Vector3> _onMove;
    private ObstacleAvoidance _obstacleAvoidance;

    private Func<Node> _getRandomNode;
    private MRIModel _model;

    private List<Node> _waypointsToPatrol;
    private float _minimumWaypointDistance;
    private int _patrolSpotIndex;
    private Action _onPatrolEnter;

    public MrIPatrolState(Func<bool> attemptSeePlayer, INode root, Func<Node,List<Node>> getWaypoints, Action <bool> setIdleCooldown, Func<Node> getRandomNode, MRIModel model, float minimumWaypointDistance, ObstacleAvoidance obstacleAvoidance, Action <Vector3> onMove, Action onPatrolEnter)
    {
        _attemptSeePlayer = attemptSeePlayer;
        _root = root;
        _getWaypoints = getWaypoints;
        _setIdleCooldown = setIdleCooldown;
        _getRandomNode = getRandomNode;
        _model = model;
        _minimumWaypointDistance = minimumWaypointDistance;
        _obstacleAvoidance = obstacleAvoidance;
        _onMove = onMove;
        _onPatrolEnter = onPatrolEnter;
    }

    
    public override void Awake()
    {
        _setIdleCooldown?.Invoke(false);
        _patrolSpotIndex = 0;
        _onPatrolEnter?.Invoke();
        SetNextRandomNodeAndPath();
    }

    private void SetNextRandomNodeAndPath()
    {
        //Gets random node from roulette wheel, with weight based on distance to player
        var nextRandomNode = _getRandomNode.Invoke();
        //Pathfinding with AStar+
        _waypointsToPatrol = _getWaypoints?.Invoke(nextRandomNode);
        //Set target waypoint
        if (_waypointsToPatrol != null) _obstacleAvoidance.SetNewTarget(_waypointsToPatrol[_patrolSpotIndex].transform);
        //Set patrol mode to seek
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
    }

    private bool NextWaypointAvailable()
    {
        _patrolSpotIndex++;
        if (_patrolSpotIndex >= _waypointsToPatrol.Count)
        {
            _patrolSpotIndex = 0;
            return false;
        }

        var nextPatrolSpot = _waypointsToPatrol[_patrolSpotIndex];
        _obstacleAvoidance.SetNewTarget(nextPatrolSpot.transform);
        return true;

    }
    
    
    public override void Execute()
    {
        var dir = _obstacleAvoidance.GetDir();
        
        _onMove?.Invoke(dir);

        if (_attemptSeePlayer.Invoke())
        {
            _root.Execute();
            return;
        }

        var pointPosition = _waypointsToPatrol[_patrolSpotIndex].transform.position;
        var distanceToDestination = Vector3.Distance(_model.transform.position,
            pointPosition);

        if (distanceToDestination > _minimumWaypointDistance) return;
        
        var waypointAvailable = NextWaypointAvailable();

        if (waypointAvailable) return;
        
        _setIdleCooldown.Invoke(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _setIdleCooldown.Invoke(true);
    }
}
