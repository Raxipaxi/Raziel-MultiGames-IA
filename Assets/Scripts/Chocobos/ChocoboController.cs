using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboController : MonoBehaviour
{
    private ChocoboModel _chocoModel;
    [SerializeField] private ChocoboData _data;

    [SerializeField] private List<Transform> _actors;

    private FSM<ChocoboStatesConstants> _fsm;
    private INode _root;
  

    public event Action<Transform> OnFollow;
    public event Action OnIdle;

    private Transform _potentialLeader;


    private void Awake()
    {
        BakeReferences();
    }

    private void OnIdleCommand()
    {
        OnIdle?.Invoke();
    }

    private void OnFollowCommand(Transform dir)
    {
        OnFollow?.Invoke(dir);
    }

    void Start()
    {
        
        _chocoModel.SubscribeToEvents(this);
        DecisionTreeInit();
        FSMInit();
    }

    void DecisionTreeInit()
    {
        //Actions
        var goToIdle = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Idle));
        var goToFollow = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Follow));

        //Questions
        var IsAnyLeader = new QuestionNode(PotentialLeader, goToFollow, goToIdle);
        
        //Root
        _root = IsAnyLeader;

    }
    
    


    void FSMInit()
    {
        // States
        var idle = new ChocoboIdleState<ChocoboStatesConstants>(_chocoModel.LineOfSightAI, _data.secondsToFollow, OnIdleCommand, _root);
        var follow = new ChocoboFollowState<ChocoboStatesConstants>(_actors, OnFollow,_chocoModel.LineOfSightAI, _root);
        
        // Transitions
        // Idle
        idle.AddTransition(ChocoboStatesConstants.Follow,follow);
        
        
        //Follow
        follow.AddTransition(ChocoboStatesConstants.Idle, idle);
        
    }

    private bool PotentialLeader()
    {
        _actors = _chocoModel.LineOfSightAI.LineOfSightMultiTarget();
        return _actors.Count > 0;
    }
    public void BakeReferences()
    {
        _chocoModel = GetComponent<ChocoboModel>();

    }


    void Update()
    {
        _fsm.UpdateState();
    }


}
