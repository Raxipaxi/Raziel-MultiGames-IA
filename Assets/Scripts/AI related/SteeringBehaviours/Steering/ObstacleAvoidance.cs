using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    private Transform _self;
    private Transform _target;
    private float _checkRadius;  
    private Collider[] obstaclesColliders;
    private LayerMask _obstaclesMask;
    private float _multiplier;
    private ISteering[] _behaviours;

    public bool StopMovement;

    public enum DesiredBehaviour
    {
        Seek,
        Flee,
        Pursuit,
        Evasion,        
    }

    private ISteering _currentBehaviour;

    public Transform SetTarget { set => _target = value; }

    public ObstacleAvoidance (Transform self,Transform target,
                              float radius, int maxObjs,
                              LayerMask obstaclesMask, float multiplier,
                              IVel targetVel, float timePrediction,
                              DesiredBehaviour _defaultBehaviour)
    {
        _self = self;
        _target = target;
        _checkRadius = radius;
        obstaclesColliders = new Collider[maxObjs];
        _obstaclesMask = obstaclesMask;
        _multiplier = multiplier;

        _behaviours = new ISteering[4];

        SetBehaviours(_self, target, targetVel,timePrediction);

        SetNewBehaviour(_defaultBehaviour);
    }

    private void SetBehaviours(Transform self, Transform target, IVel targetIvel, float timePrediction)
    {
        _behaviours[0] = new Seek(self, target);
        _behaviours[1] = new Flee(self, target);
        _behaviours[2] = new Pursuit(self,target,targetIvel,timePrediction);
        _behaviours[3] = new Evasion(self, target, targetIvel, timePrediction);
    }
    public Vector3 GetDir()
    {

        if (StopMovement) return Vector3.zero;

        Vector3 dir = _currentBehaviour.GetDir(); 
        //Vector3 dir = (_target.position - _self.position).normalized;

        int countObjs = Physics.OverlapSphereNonAlloc(_self.position, _checkRadius, obstaclesColliders, _obstaclesMask);      

        Collider nearestObject = null;
        float distanceNearObj = 0;

        for (int i = 0; i < countObjs; i++)
        {
            var curr = obstaclesColliders[i];
            if (_self.position == curr.transform.position) continue;
            Vector3 closestPointToSelf = curr.ClosestPointOnBounds(_self.position);
            float distanceCurr = Vector3.Distance(_self.position, closestPointToSelf);

            if (nearestObject == null)
            {
                nearestObject = curr;
                distanceNearObj = distanceCurr;
            }
            else
            {               
                float distance = Vector3.Distance(_self.position, curr.transform.position);
                if (distanceNearObj > distance)
                {
                    nearestObject = curr;
                    distanceNearObj = distanceCurr;
                }
            }
        }

        if (nearestObject != null)
        {
            var posObj = nearestObject.transform.position;
            Vector3 dirObstacleToSelf = (_self.position - posObj);          
            dirObstacleToSelf= dirObstacleToSelf.normalized * ((_checkRadius - distanceNearObj) / _checkRadius) * _multiplier;
            dir += dirObstacleToSelf;
            dir = dir.normalized;
        }

        return dir;
    }

    

    public void SetNewTarget (Transform _newTarget)
    {
        for (int i = 0; i < _behaviours.Length; i++)
        {
            _behaviours[i].SetTarget = _newTarget;
        }

        //_target = _newTarget;
    }
    public void SetNewBehaviour(DesiredBehaviour newBehaviour)
    {
        _currentBehaviour = _behaviours[(int)newBehaviour];
    }

}
