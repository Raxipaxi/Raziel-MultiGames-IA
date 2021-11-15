using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrowView : MonoBehaviour
{

    private Animator _anim;
    private static readonly int Velocity = Animator.StringToHash("Velocity");
    private static readonly int StunNegated = Animator.StringToHash("StunNegated");

    public void SubscribeToEvents(ScareCrowModel model)
    {
        model.OnChase += OnChaseHandler;
        model.OnIdle += OnIdleHandler;
        model.OnMove += OnMoveHandler;
        model.OnStunned += OnStunHandler;
        model.OnFinishStun += OnFinishStunHandler;
    }

    private void Awake()
    {
        BakeReferences();
    }

    private void BakeReferences()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnFinishStunHandler()
    {
        _anim.SetTrigger(StunNegated);
    }
    private void OnChaseHandler()
    {
        
    }
    private void OnMoveHandler(float velocity)
    {
        _anim.SetFloat(Velocity,velocity);
    }

    private void OnStunHandler()
    {
        _anim.Play("Stun");
    }

    private void OnIdleHandler()
    {
        
    }
}
