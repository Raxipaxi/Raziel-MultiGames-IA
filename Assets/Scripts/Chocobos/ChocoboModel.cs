using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour, IMove
{
   public LineOfSightAI LineOfSightAI => _lineOfSightAI;
   private LineOfSightAI _lineOfSightAI;
   private ChocoboView _chocoboView;
   [SerializeField] public ChocoboData _data;
   
    private void Awake()
    {
        BakeReferences();
    }

    public void Idle()
    {
 
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

    }

    public void Move(Vector3 dir)
    {
        dir.y = 0;
     //  var currDir = dir.normalized;
        transform.position += dir * Time.deltaTime * _data.chocoboFollowSpeed;
     //   _rb.velocity =  dir * _data.chocoboFollowSpeed;
        if (dir==Vector3.zero)return;
        transform.forward = dir.normalized;
        _chocoboView.MoveAnimation();
    }
}
