using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrowModel : MonoBehaviour, IVel
{

    public float Vel { get; private set; }
    [SerializeField] private ScareCrowData data;
    private ScareCrowView _view;

    private Vector3 _startingPosition;
    public void SubscribeToEvents(ScareCrowController controller)
    {
        controller.OnChase += ()=> OnChase?.Invoke();
        controller.OnIdle += ()=>OnIdle?.Invoke();
        controller.OnMove += OnMoveHandler;
        controller.OnReset += OnResetHandler;
        controller.OnStun += () => OnStunned?.Invoke();
        controller.OnFinishStun += () => OnFinishStun?.Invoke();
    }
    
    public event Action<float> OnMove;
    public event Action OnStunned;
    public event Action OnChase;
    public event Action OnIdle;
    public event Action OnFinishStun;
    
    private void OnResetHandler()
    {
        transform.position = _startingPosition;
        OnMoveHandler(Vector3.zero);
    }

    private void OnMoveHandler(Vector3 dir)
    {
        dir = dir.normalized;
        Vel = dir.magnitude * data.chaseSpeed;
        dir.y = 0;
        transform.position += dir * data.chaseSpeed * Time.deltaTime;
        OnMove?.Invoke(Vel);
        if (dir == Vector3.zero) return;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * data.rotationSpeed);
    }
    
    private void Awake()
    {
        BakeReferences();
    }
    private void BakeReferences()
    {
        _startingPosition = transform.position;
        _view = GetComponent<ScareCrowView>();
    }
    private void Start()
    {
        _view.SubscribeToEvents(this);
    }

}
