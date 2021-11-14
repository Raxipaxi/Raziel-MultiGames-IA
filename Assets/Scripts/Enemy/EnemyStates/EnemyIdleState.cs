using System;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private INode _root;
    private LineOfSightAI _lineOfSightAI;
    private Transform _target;
    private float _idleLenght;
    private float _cooldown;
    private Action<bool> _setIdleCommand;
  

    private Action _onIdle;
    public EnemyIdleState(float idleLenght,LineOfSightAI lineOfSightAI, Transform target,Action onIdle, INode root, Action <bool> setIdleCommand)
    {
        _root = root;
        _lineOfSightAI = lineOfSightAI;
        _target = target;
        _idleLenght = idleLenght;
        _onIdle = onIdle;
        _setIdleCommand = setIdleCommand;
    }

    public override void Awake()
    {
        ResetCd();
        _onIdle?.Invoke();
        _setIdleCommand?.Invoke(true);
    }

    public override void Execute()
    {
        if (Time.time > _cooldown || _lineOfSightAI.SingleTargetInSight(_target))
        {          
            ResetCd();
            _setIdleCommand?.Invoke(false);
            _root.Execute();
        }   
    }


    private void ResetCd()
    {
        _cooldown = Time.time + _idleLenght;
    }
    
    
}
