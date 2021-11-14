using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChocoboFollowState<T> : State<T>
{
    private Transform _leader;
    private List<Transform> Leaders;
    private INode _root;
    private LineOfSightAI _lineOfSightAI;
    private Action<Transform> _onFollow;
    public ChocoboFollowState(List<Transform> leaders, Action<Transform> onFollow,LineOfSightAI lineOfSightAI, INode root)
    {
        Leaders = leaders;
        _onFollow = onFollow;
        _lineOfSightAI = lineOfSightAI;
        _root = root;
    }

    public override void Awake()
    {
        _leader = Leaders.First();
    }

    public override void Execute()
    {
        if (!_lineOfSightAI.SingleTargetInSight(_leader))
        {
            _root.Execute();
            return;
        }
        _onFollow?.Invoke(_leader);
    }
}
