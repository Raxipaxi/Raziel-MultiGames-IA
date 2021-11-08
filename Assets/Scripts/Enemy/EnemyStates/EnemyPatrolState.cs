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
    
    
    public EnemyPatrolState(EnemyModel enemyModel, Transform target, Transform[] waypoints, Action<Vector3> OnWalk , float minDistance,INode root)
    {
        _enemyModel = enemyModel.transform;
        _target = target;
        _waypoints = waypoints;
        _lineOfSightAI = enemyModel.LineOfSightAI;
        _onWalk = OnWalk;
        //_obstacleAvoidance = enemyModel.ObstacleAvoidance;
        _minDistance = minDistance;
        _root = root;
    }

    public override void Awake()
    {
        if (_currpatrolPoint == null)
        {
            _currpatrolPoint = NearestPatPoint();
            _obstacleAvoidance.SetTarget = _currpatrolPoint;
            if (!visitedWP.Contains(_currpatrolPoint)) visitedWP.Add(_currpatrolPoint);
        }    
        _obstacleAvoidance.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Seek);
    }
    private Transform NearestPatPoint()
    {
        float minDist= float.MaxValue;
        float currDist;
        Transform nearestPatrolpt= null;
        
        for (int i = 0; i < _waypoints.Length; i++)
        {
            currDist = Vector3.Distance(_enemyModel.position, _waypoints[i].position);
            if (currDist<minDist)
            {
                if (_currpatrolPoint!=null)
                {
                    if (currDist>Vector3.Distance(_enemyModel.position,_currpatrolPoint.position)&&!visitedWP.Contains(_waypoints[i]))
                    {
                        minDist = currDist;
                        nearestPatrolpt = _waypoints[i]; 
                    }
                }
                else
                {
                    minDist = currDist;
                    nearestPatrolpt = _waypoints[i];    
                }
            }
        }
        if (visitedWP.Count.Equals(_waypoints.Length)) CleanVisitedWp();
        
        return nearestPatrolpt;
    }

    private void CleanVisitedWp()
    {
        visitedWP.Clear();
    }

    public override void Execute()
    {
        
        _onWalk?.Invoke(_obstacleAvoidance.GetDir());
        
        if (!_lineOfSightAI.SingleTargetInSight(_target))
        {
            if (Vector3.Distance(_enemyModel.position, _currpatrolPoint.position) < _minDistance)
            {
                _currpatrolPoint = NearestPatPoint();
                visitedWP.Add(_currpatrolPoint);
                _root.Execute();
            }
        }
        else
        {
            _root.Execute();
        }
        
    }

}
