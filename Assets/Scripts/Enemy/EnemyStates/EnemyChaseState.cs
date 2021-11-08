using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState<T> : State<T>
{
    private Transform _self;
    private Transform _target;

    private INode _root;
    private ObstacleAvoidance _behaviour;
    private LineOfSightAI _lineOfSightAI;
    private Action<Vector3> _onChase;

    public EnemyChaseState(Transform self,Transform target, INode root, ObstacleAvoidance behaviour, LineOfSightAI lineOfSightAI,Action<Vector3> onChase)
    {
        _self = self;
        _target = target;
        _root = root;
        _behaviour = behaviour;
        _lineOfSightAI = lineOfSightAI;
        _onChase = onChase;
    }

    public override void Awake()
    {
        _behaviour.SetNewBehaviour(ObstacleAvoidance.DesiredBehaviour.Pursuit);
        _behaviour.SetTarget = _target;
    }


    void Execute()
    {
        //TODO  AGREGAR EL PARAMETRO DE DISTANCIA DE ATAQUE  
      
        if (!_lineOfSightAI.SingleTargetInSight(_target)|| Vector3.Distance(_target.position,_self.position)< 0.65f ) _root.Execute();
        else
        {
            _onChase(_behaviour.GetDir());
        }
            
        
      
    }
}
