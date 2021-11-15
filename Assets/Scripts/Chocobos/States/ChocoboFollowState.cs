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
    private float _leaderCheck;
    private float _followCheck;
    
    public ChocoboFollowState(Func<Transform> potentialLeader, Action<Transform> onFollow,LineOfSightAI lineOfSightAI,LineOfSightDataScriptableObject newLineOfSight, float leaderCheck, INode root)
    {
        _potentialLeader = potentialLeader;
        _onFollow = onFollow;
        _lineOfSightAI = lineOfSightAI;
        _newLineOfSightAI = newLineOfSight;
        _leaderCheck = leaderCheck;
        _root = root;
    }

    public override void Awake()
    {
      
        _lineOfSightAI.SwapLineOfSightData(_newLineOfSightAI);
        AddCooldown();

    }

    public override void Execute()
    {
        _onFollow?.Invoke(_potentialLeader?.Invoke());
        if (!(_followCheck < Time.time)) return;
        AddCooldown();
        
        if (!_lineOfSightAI.SingleTargetInSight(_potentialLeader?.Invoke()))
        {
            _root.Execute();
        }
        
    }

    private void AddCooldown()
    {
        _followCheck = _leaderCheck + Time.time;
    }
}
