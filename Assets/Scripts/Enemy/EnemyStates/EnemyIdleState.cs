using System;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private INode _root;
    private LineOfSightAI _lineOfSightAI;
    private Transform _target;
    private float _idleLenght;
    private float _cooldown;

    private Action _onIdle;
    public EnemyIdleState(float idleLenght,LineOfSightAI lineOfSightAI, Transform target,Action onIdle, INode root)
    {
        _root = root;
        _lineOfSightAI = lineOfSightAI;
        _target = target;
        _idleLenght = idleLenght;
        _onIdle = onIdle;
    }

    public override void Awake()
    {
        ResetCD();
        _onIdle?.Invoke();
    }

    public override void Execute()
    {
        if (Time.time > _cooldown || _lineOfSightAI.SingleTargetInSight(_target))
        {
            Debug.Log("Holis");
            ResetCD();
            _root.Execute();
        }   
    }


    private void ResetCD()
    {
        _cooldown = Time.time + _idleLenght;
    }
    
    
}
