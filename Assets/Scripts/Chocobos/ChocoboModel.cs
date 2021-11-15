using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour, IMove
{
    public LineOfSightAI LineOfSightAI => _lineOfSightAI;
    private LineOfSightAI _lineOfSightAI;
    private ChocoboView _chocoboView;
    [SerializeField] private ChocoboData _data;


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
        _chocoboView.IdleAnimation();   
    }

    public void SubscribeToEvents(ChocoboController controller)
    {
        controller.OnIdle += Idle;
        controller.OnFollowDir += Move;
    }
    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>();
        _chocoboView = GetComponent<ChocoboView>();
    }

    public void Move(Vector3 dir)
    {
        transform.position += Time.deltaTime * dir * _data.chocoboFollowSpeed;
        transform.forward = Vector3.Lerp(transform.forward, dir, _data.chocoboFollowSpeed * Time.deltaTime);
        _chocoboView.MoveAnimation();
    }
}
