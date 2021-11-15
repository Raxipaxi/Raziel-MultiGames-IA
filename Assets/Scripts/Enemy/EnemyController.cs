using System;
using UnityEngine;

public class EnemyController : MonoBehaviour,IReseteable
{
    private EnemyModel _enemyModel;
    private FSM<EnemyStatesConstants> _fsm;
    private INode _root;

    
    [SerializeField] private BrollaChanData data;

    #region Steering Properties
    [SerializeField] private PlayerModel target;
    [SerializeField] private Transform[] waypoints;
    
    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidance;
    private bool _waitForIdleState;

   
    public ObstacleAvoidance Behaviour { get; private set; }

    #endregion
    
    
    private bool _previousInSightState;
    private bool _currentInSightState;

    #region Actions
    public event Action<Vector3> OnMove;
    public event Action OnChase;
    public event Action OnIdle;
    public event Action OnAttack;

    public event Action OnPatrol;

    public event Action OnReset;
    //public event Action OnStun;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        BakeReferences();
    }

    #region Commands

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

    private void OnAttackCommand()
    {
        OnAttack?.Invoke();
    }

    private void OnPatrolCommand()
    {
        OnPatrol?.Invoke();
    }
   
    #endregion
    private void Start()
    {
        _enemyModel.SubscribeToEvents(this);
        InitDecisionTree();
        InitFsm();
        //Events subs??
    }

    private bool IsIdleStateCooldown()
    {
        return _waitForIdleState;
    }

    private void SetIdleStateCooldown(bool newState)
    {
        _waitForIdleState = newState;
    }

    private bool IsCloseEnoughToAttack()
    {
        var distance = Vector3.Distance(transform.position, target.transform.position);

        return distance <= data.distanceToAttack;
    }

    private void InitFsm()
    {
        //--------------- FSM Creation -------------------//                
        // States Creation
        var idle = new EnemyIdleState<EnemyStatesConstants>(data.idleLenght, CheckPlayerInSight,
            OnIdleCommand, _root, SetIdleStateCooldown);
        var patrol = new EnemyPatrolState<EnemyStatesConstants>(_enemyModel, target.transform, waypoints, OnMoveCommand,
            data.minDistance, Behaviour, _root, SetIdleStateCooldown, CheckPlayerInSight,OnPatrolCommand);
        var chase = new EnemyChaseState<EnemyStatesConstants>(target.transform, _root, Behaviour, OnChaseCommand,
            data.timeToAttemptAttack, OnMoveCommand,SetIdleStateCooldown);
        var attack = new EnemyAttackState<EnemyStatesConstants>(_root,OnAttackCommand, data.timeToOutOfAttack,SetIdleStateCooldown);
      
        
        //Idle State
        idle.AddTransition(EnemyStatesConstants.Patrol,patrol);
        idle.AddTransition(EnemyStatesConstants.Chase,chase);
        
        
        //Patrol
        patrol.AddTransition(EnemyStatesConstants.Chase,chase);
        patrol.AddTransition(EnemyStatesConstants.Idle,idle);

        //Chase 
        chase.AddTransition(EnemyStatesConstants.Idle,idle);
        chase.AddTransition(EnemyStatesConstants.Attack,attack);
        
        //Attack
        attack.AddTransition(EnemyStatesConstants.Chase,chase);
        attack.AddTransition(EnemyStatesConstants.Idle,idle);
        
        _fsm = new FSM<EnemyStatesConstants>(idle);
    }

    private void InitDecisionTree()
    {
        // Actions

        var goToFollow = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Chase));
        var goToPatrol = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Patrol));
        var goToAttack = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Attack));
        var goToIdle = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Idle));
        
        
        //Questions
        var CheckIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, goToIdle, goToPatrol);
        var DidSightChangeToLose = new QuestionNode(SightStateChanged, goToIdle, CheckIdleStateCooldown);
        var attemptPlayerKill = new QuestionNode(IsCloseEnoughToAttack, goToAttack, goToFollow);
        var DidSightChangeToAttack = new QuestionNode(SightStateChanged, goToFollow, attemptPlayerKill);
        var IsInSight = new QuestionNode(LastInSightState, DidSightChangeToAttack, DidSightChangeToLose);

         //Root 
         var IsPlayerAlive = new QuestionNode(() => target.LifeController.IsAlive, IsInSight, goToPatrol);
         
         Debug.Log("Init tree");   
          _root = IsPlayerAlive;
    }
    
    private bool SightStateChanged()
    { 
        return _currentInSightState != _previousInSightState;
    }

    private bool LastInSightState()
    {     
        _previousInSightState = _currentInSightState;    
        _currentInSightState = _enemyModel.LineOfSightAI.SingleTargetInSight(target.transform);
        return _currentInSightState;
    }

    private bool CheckPlayerInSight()
    {
        var playerIsInSight = _enemyModel.LineOfSightAI.SingleTargetInSight(target.transform);
        return playerIsInSight;
    }
   
    public void BakeReferences()
    {
        _enemyModel = GetComponent<EnemyModel>();
        Behaviour = new ObstacleAvoidance(transform, null, obstacleAvoidance.radius,
            obstacleAvoidance.maxObjs, obstacleAvoidance.obstaclesMask,
            obstacleAvoidance.multiplier, target, obstacleAvoidance.timePrediction,
            ObstacleAvoidance.DesiredBehaviour.Seek);
    }
    // Update is called once per frame
    void Update()
    {
        if (!target.LifeController.IsAlive || GameManager.Instance.IsPaused) return;
        
        _fsm.UpdateState();
    }

    public void OnLevelReset()
    {
        Debug.Log("Reset enemy");
        OnReset?.Invoke();
        SetIdleStateCooldown(true);
        _root.Execute();
    }
}
