using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIModel : MonoBehaviour
{

    [SerializeField] private LineOfSightAI lineOfSight;
    private MRIView _view;

    [SerializeField] private MrIData data;
    public LineOfSightAI LineOfSightAI => lineOfSight;


    private float _currentSpeed;

    private Transform _selfTransform;

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
       
        _selfTransform.position += dir * _currentSpeed * Time.deltaTime;
        transform.forward = Vector3.Lerp(_selfTransform.forward, dir,Time.deltaTime * data.rotationalInput);
    }

}
