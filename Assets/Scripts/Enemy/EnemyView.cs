using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{

    private Animator _animator;
    private void Awake()
    {
        BakeReferences();
    }

    public void BakeReferences()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetWalkAnimation()
    {
        
    }
    
    public void SetStunAnimation()
    {
        
    } 
    public void SetRunAnimation()
    {
        
    }    
    public void SetIdleAnimation()
    {
        _animator.Play("Idle");
    }    
    public void SetAttackAnimation()
    {
        
    }
}
