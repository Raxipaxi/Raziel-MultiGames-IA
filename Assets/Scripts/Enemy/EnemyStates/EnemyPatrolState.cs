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
    
    private LineOfSightAI _lineOfSightAI;
    private ObstacleAvoidance _obstacleAvoidance;
    private INode _root;
    private Transform _currpatrolPoint =null;
    private HashSet<Transform> visitedWP = new HashSet<Transform>();
    
    
    public EnemyPatrolState(EnemyModel enemyModel, Transform target, Transform[] waypoints, Action<Vector3> OnWalk , float minDistance,ObstacleAvoidance obstacleAvoidance, INode root)
    {
        _enemyModel = enemyModel.transform;
        _target = target;
        _waypoints = waypoints;
        _lineOfSightAI = enemyModel.LineOfSightAI;
        _onWalk = OnWalk;
        _minDistance = minDistance;
        _root = root;
        _obstacleAvoidance = obstacleAvoidance;
    }

    public override void Awake()
    {
        Debug.Log("Patrol");
        if (_currpatrolPoint == null)
        {
            Debug.Log("Patrol point null");
            _currpatrolPoint = NearestPatPoint();
        }    
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
    }
    private Transform NearestPatPoint()
    {
        float minDist= float.MaxValue;
        float currDist;
        Transform nearestPatrolpt= null;
        int index = 0;
        
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
        Vector3 dir = _obstacleAvoidance.GetDir();
        dir.y = 0;
        _onWalk?.Invoke(dir);

        if (_lineOfSightAI.SingleTargetInSight(_target))
        {
            _root.Execute();
            return;
        }

        var distanceToWaypoint = Vector3.Distance(_enemyModel.position, _currpatrolPoint.position);
        
       
        if (distanceToWaypoint > _minDistance) return;

        ResetPatrolPoint();
       
        _root.Execute();
    }

    public override void Sleep()
    {
        _currpatrolPoint = null;
    }

    private void ResetPatrolPoint()
    {
        _currpatrolPoint = NearestPatPoint();
        _obstacleAvoidance.SetNewTarget(_currpatrolPoint);
    }
}
