using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboController : MonoBehaviour, IReseteable
{
    private ChocoboModel _chocoModel;

    [SerializeField] private LineOfSightDataScriptableObject _flocksight;

    [SerializeField]private List<Transform> possibleLeaders;

    [SerializeField] private float followCheck;
    private ChocoboFlockingActive _flockingActive;
    private FSM<ChocoboStatesConstants> _fsm;
    private INode _root;

    private bool _reachedGoal;
    public event Action<Transform> OnFollow;
    public event Action OnIdle;
    public event Action OnReset;
    public event Action OnReachGoal;
    private Transform _potentialLeader;
    


    private void Awake()
    {
        BakeReferences();
    }

    private void OnIdleCommand()
    {
        OnIdle?.Invoke();
    }

    private void OnFollowCommand(Transform tr)
    {
        OnFollow?.Invoke(tr);
    }

    private void Start()
    {
        _flockingActive.SubscribeToEvents(this);
        _chocoModel.SubscribeToEvents(this);
        DecisionTreeInit();
        FsmInit();
    }

    private void DecisionTreeInit()
    {
        //Actions
        var goToIdle = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Idle));
        var goToFollow = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Follow));
        var goToReachedGoal = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Dance));
        //Questions
       
        var isAnyLeader = new QuestionNode(PotentialLeader, goToFollow, goToIdle);
        var reachedGoal = new QuestionNode(() => _reachedGoal, goToReachedGoal, isAnyLeader);
        
        //Root
        _root = reachedGoal;
    }

    public void ReachedGoal()
    {
        _reachedGoal = true;
        _root.Execute();
    }

    private void ReachGoal()
    {
        OnReachGoal?.Invoke();
    }
    private void FsmInit()
    {
        // States
        var idle = new ChocoboIdleState<ChocoboStatesConstants>(PotentialLeader, _chocoModel._data.secondsToFollow, OnIdleCommand, OnFollowCommand ,_chocoModel.LineOfSightAI, _root);
        var follow = new ChocoboFollowState<ChocoboStatesConstants>(()=>_potentialLeader, OnFollowCommand,_chocoModel.LineOfSightAI, _flocksight, followCheck, _root);
        var reachedGoal = new EnemyDeadState<ChocoboStatesConstants>(ReachGoal);
        
        // Transitions
        // Idle
        idle.AddTransition(ChocoboStatesConstants.Follow,follow);
        idle.AddTransition(ChocoboStatesConstants.Dance,reachedGoal);
        //Follow
        follow.AddTransition(ChocoboStatesConstants.Idle, idle);
        follow.AddTransition(ChocoboStatesConstants.Dance,reachedGoal);

        _fsm = new FSM<ChocoboStatesConstants>(idle);

    }

    private bool PotentialLeader()
    {
        var closest =  float.MaxValue;
        _potentialLeader = null;
        foreach (var seen in possibleLeaders)
        {
            if (!_chocoModel.LineOfSightAI.SingleTargetInSight(seen)) continue;

            var currDis = Vector3.Distance(seen.position, transform.position);
            if (closest > currDis)
            {
                closest = currDis;
                _potentialLeader = seen;
            }
        }

        return _potentialLeader!=null;
    }

    private void BakeReferences()
    {
        _chocoModel = GetComponent<ChocoboModel>();
        _flockingActive = GetComponent<ChocoboFlockingActive>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        _fsm.UpdateState();
    }

    public void OnLevelReset()
    {
        if (!_reachedGoal)
        {
            OnReset?.Invoke();
            //_root.Execute();
        }
       
    }
}
