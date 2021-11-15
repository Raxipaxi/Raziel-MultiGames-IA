using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour
{
    public LineOfSightAI LineOfSightAI => _lineOfSightAI;
    private LineOfSightAI _lineOfSightAI;
    private ChocoboView _chocoboView;


    private void Awake()
    {
        BakeReferences();
    }

    public void Move(Transform dir)
    {
        _chocoboView.MoveAnimation();
    }

    public void Idle()
    {
        _chocoboView.IdleAnimation();   
    }

    public void SubscribeToEvents(ChocoboController controller)
    {
        controller.OnIdle += Idle;
        controller.OnFollow += Move;
    }
    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>();
        _chocoboView = GetComponent<ChocoboView>();
    }
    
}
