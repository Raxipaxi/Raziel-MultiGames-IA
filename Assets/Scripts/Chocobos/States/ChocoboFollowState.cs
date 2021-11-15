using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChocoboFollowState<T> : State<T>
{
    private Func<Transform> _potentialLeader;
    private INode _root;
    private LineOfSightAI _lineOfSightAI;
    private LineOfSightDataScriptableObject _newLineOfSightAI;
    private Action<Transform> _onFollow;
    public ChocoboFollowState(Func<Transform> potentialLeader, Action<Transform> onFollow,LineOfSightAI lineOfSightAI,LineOfSightDataScriptableObject newLineOfSight, INode root)
    {
        _potentialLeader = potentialLeader;
        _onFollow = onFollow;
        _lineOfSightAI = lineOfSightAI;
        _newLineOfSightAI = newLineOfSight;
        _root = root;
    }

    public override void Awake()
    {
      
        _lineOfSightAI.SwapLineOfSightData(_newLineOfSightAI);
        
    }

    public override void Execute()
    {
        if (!_lineOfSightAI.SingleTargetInSight(_potentialLeader?.Invoke()))
        {
            _root.Execute();
            return;
        }
        _onFollow?.Invoke(_potentialLeader?.Invoke());
    }
}
