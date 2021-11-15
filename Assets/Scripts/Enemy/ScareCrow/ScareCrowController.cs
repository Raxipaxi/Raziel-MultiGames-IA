using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrowController : MonoBehaviour, IReseteable,IStunable
{
     private ScareCrowModel _model;
    private FSM<ScareCrowStates> _fsm;
    private INode _root;

    [SerializeField] private ScareCrowData data;

    #region Steering Properties
    [SerializeField] private PlayerModel target;

    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidance;
    private bool _waitForIdleState;
    public ObstacleAvoidance Behaviour { get; private set; }

    #endregion

    private bool _isStunned;
    
    private bool _previousInSightState;
    private bool _currentInSightState;

    #region Actions
    public event Action<Vector3> OnMove;
    public event Action OnChase;
    public event Action OnIdle;
    public event Action OnReset;
    public event Action OnStun;
    public event Action OnFinishStun;
    #endregion

    private enum ScareCrowStates
    {
        Idle,
        Chase,
        Stun
    }
    // Start is called before the first frame update
    void Awake()
    {
        BakeReferences();
    }

    #region Commands

    private void OnFinishStunCommand()
    {
        OnFinishStun?.Invoke();
    }
    private void OnMoveCommand(Vector3 moveDir)
    {
        OnMove?.Invoke(moveDir);
    }    
    private void OnIdleCommand()
    {
        OnIdle?.Invoke();
        SetIdleStateCooldown(true);
    }

    private void OnChaseCommand()
    {
        OnChase?.Invoke();
    }

    private void OnStunCommand()
    {
        OnStun?.Invoke();
    }

    #endregion
    private void Start()
    {
        _model.SubscribeToEvents(this);
        InitDecisionTree();
        InitFsm();
    }

    private bool IsIdleStateCooldown()
    {
        return _waitForIdleState;
    }

    private void SetStun(bool newState)
    {
        _isStunned = newState;
    }

    public void GetStun()
    {
        SetStun(true);
        _root.Execute();
    }

    private bool IsStunned()
    {
        return _isStunned;
    }
    private void SetIdleStateCooldown(bool newState)
    {
        _waitForIdleState = newState;
    }

    private void InitFsm()
    {
        //--------------- FSM Creation -------------------//                
        // States Creation
        var idleState = new EnemyIdleState<ScareCrowStates>(data.idleLenght, () => false,
            OnIdleCommand, _root, SetIdleStateCooldown);
        var chaseState = new EnemyChaseState<ScareCrowStates>(target.transform, _root, Behaviour, OnChaseCommand,
            data.timeToCheckOnPlayer, OnMoveCommand, SetIdleStateCooldown);
        var stunState = new EnemyStunState<ScareCrowStates>(SetStun, data.timeToOutOfStun, OnStunCommand, _root, OnFinishStunCommand);

        //Idle State
        idleState.AddTransition(ScareCrowStates.Chase,chaseState);
        idleState.AddTransition(ScareCrowStates.Stun,stunState);

        //Chase 
        chaseState.AddTransition(ScareCrowStates.Idle,idleState);
        chaseState.AddTransition(ScareCrowStates.Stun,stunState);
        
        //Attack
        stunState.AddTransition(ScareCrowStates.Idle,idleState);
        stunState.AddTransition(ScareCrowStates.Chase,chaseState);

        _fsm = new FSM<ScareCrowStates>(idleState);
    }

    private void InitDecisionTree()
    {
        // Actions

        var goToFollow = new ActionNode(()=> _fsm.Transition(ScareCrowStates.Chase));
        var goToIdle = new ActionNode(() => _fsm.Transition(ScareCrowStates.Idle));
        var goToStun = new ActionNode(() => _fsm.Transition(ScareCrowStates.Stun));

        //Questions
        var checkIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, goToIdle, goToFollow);
        var checkStunnedState = new QuestionNode(IsStunned, goToStun, checkIdleStateCooldown);
        
        //Root 
         var isPlayerAlive = new QuestionNode(() => target.LifeController.IsAlive, checkStunnedState, goToIdle);
         
         Debug.Log("Init tree"); 
          _root = isPlayerAlive;
    }

    private void BakeReferences()
    {
        _model = GetComponent<ScareCrowModel>();
        Behaviour = new ObstacleAvoidance(transform, null, obstacleAvoidance.radius,
            obstacleAvoidance.maxObjs, obstacleAvoidance.obstaclesMask,
            obstacleAvoidance.multiplier, target, obstacleAvoidance.timePrediction,
            ObstacleAvoidance.DesiredBehaviour.Pursuit);
    }
    // Update is called once per frame
    void Update()
    {
        if (!target.LifeController.IsAlive || GameManager.Instance.IsPaused) return;
        _fsm.UpdateState();
    }

    public void OnLevelReset()
    {
        OnReset?.Invoke();
        SetIdleStateCooldown(true);
        _root.Execute();
    }

  
}
