using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboController : MonoBehaviour
{
    private ChocoboModel _chocoModel;
   [SerializeField] private ChocoboData data;

    [SerializeField]private List<Transform> actors;
    private ChocoboFlockingActive _flockingActive;
    private FSM<ChocoboStatesConstants> _fsm;
    private INode _root;
  

    public event Action<Vector3> OnFollowDir;
    public event Action<Transform> OnFollowTr;
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

    private void OnFollowCommand(Vector3 dir, Transform tr)
    {
        OnFollowDir?.Invoke(dir);
        OnFollowTr?.Invoke(tr);
        
    }

    void Start()
    {
        _flockingActive.SubscribeToEvents(this);
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
        var isAnyLeader = new QuestionNode(PotentialLeader, goToFollow, goToIdle);
        
        //Root
        _root = isAnyLeader;

    }
    void FSMInit()
    {
        // States
        var idle = new ChocoboIdleState<ChocoboStatesConstants>(PotentialLeader, data.secondsToFollow, OnIdleCommand, _root);
        var follow = new ChocoboFollowState<ChocoboStatesConstants>(actors, OnFollowCommand,_chocoModel.LineOfSightAI, _root);
        
        // Transitions
        // Idle
        idle.AddTransition(ChocoboStatesConstants.Follow,follow);
        //Follow
        follow.AddTransition(ChocoboStatesConstants.Idle, idle);

        _fsm = new FSM<ChocoboStatesConstants>(idle);

    }

    private bool PotentialLeader()
    {
        
        var closest =  float.MaxValue;
        _potentialLeader = null;
        foreach (var seen in actors)
        {
            if (!_chocoModel.LineOfSightAI.SingleTargetInSight(seen)) continue;

            var currDis = Vector3.Distance(seen.position, transform.position);
            if (closest > currDis)
            {
                closest = currDis;
                _potentialLeader = seen;
            }
        }
       // Debug.Log("potential leader");
        return _potentialLeader!=null;
    }
    public void BakeReferences()
    {
        _chocoModel = GetComponent<ChocoboModel>();
        _flockingActive = GetComponent<ChocoboFlockingActive>();
    }

    
    void Update()
    {
        _fsm.UpdateState();
    }
    
}
