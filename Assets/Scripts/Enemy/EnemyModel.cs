﻿using UnityEngine;

public class EnemyModel : MonoBehaviour, IVel
{
    private EnemyView _enemyView;
    private Rigidbody _rb;
    public float Vel { get; }
    
    private float _currSpeed = 2f;

    private LineOfSightAI _lineOfSightAI;


    public LineOfSightAI LineOfSightAI => _lineOfSightAI;

    //TODO Hace el escriptabel
    [SerializeField] private PlayerData _enemyData;

    private void Awake()
    {
        BakeReferences();
    }

    public void SubscribeToEvents(EnemyController controller)
    {
        controller.OnWalk += Move;
        controller.OnChase += Chase;
        controller.OnIdle += Idle;
        controller.OnAttack += Attack;

    }

    void BakeReferences()
    {
        _enemyView = GetComponent<EnemyView>();
        _rb = GetComponent<Rigidbody>();
        _lineOfSightAI = GetComponent<LineOfSightAI>();

    }

    private void Move(Vector3 dir)
    {
        Debug.Log("ji");
        _rb.velocity = dir * _currSpeed;
        transform.forward = dir.normalized;
        _enemyView.SetWalkAnimation();
        
    }   
    private void Chase(Vector3 dir)
    {
        //TODO Ver lo del scriptable object con la data del enemigo
        _rb.velocity = dir * _currSpeed;
        transform.forward = dir.normalized;
        _enemyView.SetRunAnimation();
        
    }

    private void Idle()
    {
        _rb.velocity = Vector3.zero;
        _enemyView.SetIdleAnimation();
    }

    private void Die()
    {
        
    }

    private void Attack()
    {
        _enemyView.SetAttackAnimation();
    }


}
