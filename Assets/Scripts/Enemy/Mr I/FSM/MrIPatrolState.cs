﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrIPatrolState<T> : State<T>
{
  
    private Func<bool> _attemptSeePlayer;
    private INode _root;

    private Func<Node,List<Node>> _getWaypoints;
    private Action<bool> _setIdleCooldown;
    private Action<Vector3> onMove;
    private ObstacleAvoidance _obstacleAvoidance;

    private Func<Node> _getRandomNode;
    private MRIModel _model;

    private List<Node> _waypointsToPatrol;
    private float _minimumWaypointDistance;
    private int _patrolSpotIndex;

    public MrIPatrolState(Func<bool> attemptSeePlayer, INode root, Func<Node,List<Node>> getWaypoints, Action <bool> setIdleCooldown, Func<Node> getRandomNode, MRIModel model, float minimumWaypointDistance)
    {
        _attemptSeePlayer = attemptSeePlayer;
        _root = root;
        _getWaypoints = getWaypoints;
        _setIdleCooldown = setIdleCooldown;
        _getRandomNode = getRandomNode;
        _model = model;
        _minimumWaypointDistance = minimumWaypointDistance;
    }

    
    public override void Awake()
    {
        _setIdleCooldown?.Invoke(false);
        SetNextRandomNodeAndPath();
        
    }

    private void SetNextRandomNodeAndPath()
    {
        var nextRandomNode = _getRandomNode.Invoke();
        _waypointsToPatrol = _getWaypoints?.Invoke(nextRandomNode);
        if (_waypointsToPatrol != null) _obstacleAvoidance.SetNewTarget(_waypointsToPatrol[_patrolSpotIndex].transform);
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
        
        onMove?.Invoke(dir);

        if (_attemptSeePlayer.Invoke())
        {
            _root.Execute();
            return;
        }

        var distanceToDestination = Vector3.Distance(_model.transform.position,
            _waypointsToPatrol[_patrolSpotIndex].transform.position);
        
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
