using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState<T> : State<T>
{
    private EnemyModel _enemyModel;
    private Transform[] _waypoints;
    private INode _root;
    private Transform _currpatrolPoint =null;
    private HashSet<Transform> visitedWP = new HashSet<Transform>();
    
    
    public EnemyPatrolState(EnemyModel enemyModel, Transform[] waypoints, INode root)
    {
        _enemyModel = enemyModel;
        _waypoints = waypoints;
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
            currDist = Vector3.Distance(_enemyModel.transform.position, _waypoints[i].position);
            if (currDist<minDist)
            {
                if (_currpatrolPoint!=null)
                {
                    if (currDist>Vector3.Distance(_enemyModel.transform.position,_currpatrolPoint.position)&&!visitedWP.Contains(_waypoints[i]))
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
        base.Execute();
    }


}
