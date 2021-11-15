using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Animator _animator;
    private static readonly int Velocity = Animator.StringToHash("Velocity");

    private void Awake()
    {
        BakeReferences();
    }

    public void SubscribeToEvents(EnemyModel model)
    {
        model.onMove += SetMoveAnimation;
        model.OnAttack += SetAttackAnimation;
    }

    public void BakeReferences()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetMoveAnimation(float velocity)
    {
        _animator.SetFloat(Velocity, velocity);
    }

    public void SetAttackAnimation()
    {
        _animator.Play("Attack");   
    }
}
