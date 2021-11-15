using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private Transform _enemyModel;
    private Transform _target;
    private Transform[] _waypoints;
    private Action<Vector3> _onWalk;
    private float _minDistance;
    
    private Func<bool> _attemptSeePlayer;
    private ObstacleAvoidance _obstacleAvoidance;
    private INode _root;
    private Transform _currpatrolPoint =null;
    private HashSet<Transform> visitedWP = new HashSet<Transform>();
    private Action<bool> _setIdleCommand;
    private Action _onStartPatrol;
    public EnemyPatrolState(EnemyModel enemyModel, Transform target, Transform[] waypoints, Action<Vector3> OnWalk , float minDistance,ObstacleAvoidance obstacleAvoidance, INode root, Action<bool> setIdleCommand, Func<bool> attemptSeePlayer, Action onStartPatrol)
    {
        _enemyModel = enemyModel.transform;
        _target = target;
        _waypoints = waypoints;
        _attemptSeePlayer = attemptSeePlayer;
        _onWalk = OnWalk;
        _minDistance = minDistance;
        _root = root;
        _obstacleAvoidance = obstacleAvoidance;
        _setIdleCommand = setIdleCommand;
        _onStartPatrol = onStartPatrol;
    }

    public override void Awake()
    {
        if (_currpatrolPoint == null)
        {
            _currpatrolPoint = NearestPatPoint();
        }    
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
        _setIdleCommand?.Invoke(false);
        _onStartPatrol?.Invoke();
    }
    private Transform NearestPatPoint()
    {
        var minDist= float.MaxValue;
        float currDist;
        Transform nearestPatrolpt= null;
        var index = 0;
        
        for (int i = 0; i < _waypoints.Length; i++)
        {
            var waypoint = _waypoints[i];

            if (visitedWP.Contains(waypoint)) continue;
            
            currDist = Vector3.Distance(_enemyModel.position, waypoint.position);
            if (currDist > minDist) continue;

            minDist = currDist;
            index = i;
            nearestPatrolpt = waypoint;
        }

        visitedWP.Add(nearestPatrolpt);
        if (visitedWP.Count==_waypoints.Length) CleanVisitedWp();
        
        
        return nearestPatrolpt;
    }

    private void CleanVisitedWp()
    {
        visitedWP.Clear();
    }

    public override void Execute()
    {
        var dir = _obstacleAvoidance.GetDir();
        dir.y = 0;
        _onWalk?.Invoke(dir);

        var seePlayer = _attemptSeePlayer.Invoke();

        if (seePlayer)
        {
            _root.Execute();
            return;
        }

        var distanceToWaypoint = Vector3.Distance(_enemyModel.position, _currpatrolPoint.position);
        
       
        if (distanceToWaypoint > _minDistance) return;

        ResetPatrolPoint();
        
        _setIdleCommand?.Invoke(true);
       
        _root.Execute();
    }

    public override void Sleep()
    {
        _setIdleCommand?.Invoke(true);
    }

    private void ResetPatrolPoint()
    {
        _currpatrolPoint = NearestPatPoint();
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
    }
}
