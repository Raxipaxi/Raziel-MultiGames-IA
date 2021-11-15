using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboView : MonoBehaviour
{
    private Animator _animator;


    private void Awake()
    {
        BakeReferences();
    }

    public void SubscribeToEvents(ChocoboModel model)
    {
        model.OnIdle += IdleAnimation;
        model.OnStartFollow += MoveAnimation;
        model.OnReachGoal += OnReachGoalHandler;
    }
    void BakeReferences()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnReachGoalHandler()
    {
        Debug.Log("Dance");
    }

    private void IdleAnimation()
    {
        _animator.Play("ChocoboIdle");
    }

    private void MoveAnimation()
    {
        _animator.Play("ChocoboWalk");
    }
}
