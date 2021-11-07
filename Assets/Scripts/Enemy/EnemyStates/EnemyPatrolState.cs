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
    private LineOfSightAI _lineOfSightAI;
    private INode _root;
    private Transform _currpatrolPoint =null;
    private HashSet<Transform> visitedWP = new HashSet<Transform>();
    
    
    public EnemyPatrolState(Transform enemyModel, Transform target, Transform[] waypoints, Action<Vector3> OnWalk ,LineOfSightAI lineOfSightAI, INode root)
    {
        _enemyModel = enemyModel;
        _target = target;
        _waypoints = waypoints;
        _lineOfSightAI = lineOfSightAI;
        _onWalk = OnWalk;
        _root = root;
    }

    public override void Awake()
    {
        if (_currpatrolPoint == null)
        {
            _currpatrolPoint = NearestPatPoint();
            if (!visitedWP.Contains(_currpatrolPoint)) visitedWP.Add(_currpatrolPoint);
        }    
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
        if (visitedWP.Count.Equals(_waypoints.Length)) CleanVisitedWP();
        
        return nearestPatrolpt;
    }

    private void CleanVisitedWP()
    {
        visitedWP.Clear();
    }

    public override void Execute()
    {
        _onWalk?.Invoke(_currpatrolPoint.position);
        if (!_lineOfSightAI.SingleTargetInSight(_target))
        {
            if (Vector3.Distance(_enemyModel.position, _currpatrolPoint.position) < 0.5f)
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
