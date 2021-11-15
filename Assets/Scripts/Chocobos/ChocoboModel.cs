using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour, IMove
{
   private ChocoboView _chocoboView;
   [SerializeField] public ChocoboData _data;


   private Vector3 _startingPosition;
   public event Action OnStartFollow;
   public event Action OnIdle;
   public event Action OnReachGoal;
    private void Awake()
    {
        BakeReferences();
    }

    private void Start()
    {
        _chocoboView.SubscribeToEvents(this);
    }

    private void Idle()
    {
        OnIdle?.Invoke();
    }

    private void OnResetHandler()
    {
        transform.position = _startingPosition;
    }

    private void OnReachGoalHandler()
    {
        OnReachGoal?.Invoke();
    }
    public void SubscribeToEvents(ChocoboController controller)
    {
        controller.OnIdle += Idle;
        controller.OnStartFollow += OnStartFollowHandler;
        controller.OnReset += OnResetHandler;
    }

    private void OnStartFollowHandler()
    {
        OnStartFollow?.Invoke();
    }

    private void BakeReferences()
    {
        _chocoboView = GetComponent<ChocoboView>();
        _startingPosition = transform.position;
    }

    public void Move(Vector3 dir)
    {
        dir = dir.normalized;
        dir.y = 0;
        transform.position += dir * Time.deltaTime * _data.chocoboFollowSpeed;
   
        if (dir==Vector3.zero)return;
        transform.forward = dir;
    }
}
