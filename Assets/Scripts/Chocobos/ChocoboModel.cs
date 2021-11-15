using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour, IMove
{
   public LineOfSightAI LineOfSightAI => _lineOfSightAI;
   private LineOfSightAI _lineOfSightAI;
   [SerializeField] private LineOfSightDataScriptableObject _flockSight; 
   private ChocoboView _chocoboView;
   [SerializeField] private ChocoboData _data;

   private Rigidbody _rb;
   


    private void Awake()
    {
        BakeReferences();
    }

    // public void Move(Transform dir)
    // {
    //     _chocoboView.MoveAnimation();
    //}

    public void Idle()
    {
        _rb.velocity = Vector3.zero;
         _chocoboView.IdleAnimation();   
    }

    public void SubscribeToEvents(ChocoboController controller)
    {
        controller.OnIdle += Idle;
    }
    public void BakeReferences()
    {
         _lineOfSightAI = GetComponent<LineOfSightAI>();
     
         _chocoboView = GetComponent<ChocoboView>();
         _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {


        dir.y = 0;
        
        _rb.velocity =  dir * _data.chocoboFollowSpeed;
        if (dir==Vector3.zero)return;
        transform.forward = dir.normalized;
        _chocoboView.MoveAnimation();
    }
}
