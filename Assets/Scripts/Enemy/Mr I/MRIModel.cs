using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIModel : MonoBehaviour
{

    [SerializeField] private LineOfSightAI lineOfSight;
    private MRIView _view;

    [SerializeField] private MrIData data;
    public LineOfSightAI LineOfSightAI => lineOfSight;


    private Vector3 _initialPos;
    private float _currentSpeed;

    private Transform _selfTransform;

    private void Awake()
    {
        BakeReferences();
        _initialPos = transform.position;
    }

    public void BakeReferences()
    {
        _view = GetComponent<MRIView>();
        _selfTransform = transform;
    }

    public void SubscribeToEvents(MRIController controller)
    {
        controller.OnMove += OnMoveHandler;
        controller.OnChaseEnter += OnChaseEnterHandler;
        controller.OnIdleEnter += OnIdleEnterHandler;
        controller.OnIdleSpin += OnIdleSpinHandler;
        controller.OnEnterFastState += OnEnterFastStateHandler;
        controller.OnPatrolEnter += OnPatrolEnterHandler;
        controller.OnResetLevel += OnResetLevel;
    }

    private void OnResetLevel()
    {
        transform.position = _initialPos;
    }

    private void OnIdleEnterHandler()
    {
        OnMoveHandler(Vector3.zero);
    }

    private void OnPatrolEnterHandler()
    {
        _currentSpeed = data.patrolSpeed;
    }

    private Vector3 _rotation;
    private void OnIdleSpinHandler()
    {
         _rotation = new Vector3(0, data.rotationsPerSecond * Time.deltaTime, 0);
         _selfTransform.Rotate(_rotation);
    }
    private void OnEnterFastStateHandler()
    {
        _currentSpeed = data.goToSpotSpeed;
    }
    private void OnChaseEnterHandler()
    {
        _currentSpeed = data.chaseSpeed;
    }
    private void OnMoveHandler(Vector3 dir)
    {
        dir.y = 0;
        transform.position += dir * _currentSpeed * Time.deltaTime;
        if (dir == Vector3.zero) return;
        transform.forward = dir;
    }

}
