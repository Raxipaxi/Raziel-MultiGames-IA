using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IStunable
{
    private EnemyModel _enemyModel;
    private FSM<EnemyStatesConstants> _fsm;
    private INode _root;

    //Try and serialize with scriptable Object
    [SerializeField] private float idleLenght;
    

    #region Steering Properties
    [SerializeField] private PlayerModel target;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float minDistance;
    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidance;
    private bool _waitForIdleState;
    private bool _isStunned;
    public ObstacleAvoidance Behaviour { get; private set; }

    #endregion
    
    
    private bool _previousInSightState;
    private bool _currentInSightState;

    #region Actions
    public event Action<Vector3> OnWalk;
    public event Action<Vector3> OnChase;
    public event Action OnIdle;
    public event Action OnAttack;
    public event Action OnStun;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        BakeReferences();
    }

    #region Commands

    private void OnWalkCommand(Vector3 moveDir)
    {
        OnWalk?.Invoke(moveDir);
    }    
    private void OnIdleCommand()
    {
        OnIdle?.Invoke();
        SetIdleStateCooldown(true);
    }

    private void OnChaseCommand(Vector3 chaseDir)
    {
        OnChase?.Invoke(chaseDir);
    }

    private void OnAttackCommand()
    {
        OnAttack?.Invoke();
    }

    private void OnStunCommand()
    {
        OnStun?.Invoke();
    }
    #endregion
    private void Start()
    {
        _enemyModel.SubscribeToEvents(this);
        InitDecisionTree();
        InitFSM();
        //Events subs??
    }

    public void GetStun()
    {
        OnStunCommand();
    }

    private void SetStun(bool newState)
    {
        _isStunned = newState;
    }

    private bool IsStunned()
    {
        return _isStunned;
    }
    private bool IsIdleStateCooldown()
    {
        return _waitForIdleState;
    }

    private void SetIdleStateCooldown(bool newState)
    {
        _waitForIdleState = newState;
    }

    void InitFSM()
    {
        //--------------- FSM Creation -------------------//                
        // States Creation
        var idle = new EnemyIdleState<EnemyStatesConstants>(idleLenght, _enemyModel.LineOfSightAI, target.transform, OnIdleCommand, _root,SetIdleStateCooldown);
        var patrol = new EnemyPatrolState<EnemyStatesConstants>(_enemyModel,target.transform,waypoints, OnWalkCommand,minDistance,Behaviour,_root,SetIdleStateCooldown);
        var chase = new EnemyChaseState<EnemyStatesConstants>(transform,target.transform, _root,Behaviour, _enemyModel.LineOfSightAI, OnChaseCommand);
        var attack = new EnemyAttackState<EnemyStatesConstants>(_root,OnAttackCommand);
        var stun = new EnemyStunState<EnemyStatesConstants>();
        
        //Idle State
        idle.AddTransition(EnemyStatesConstants.Patrol,patrol);
        idle.AddTransition(EnemyStatesConstants.Attack,attack);
        idle.AddTransition(EnemyStatesConstants.Chase,chase);
        idle.AddTransition(EnemyStatesConstants.Stun,stun);
        
        //Patrol
        patrol.AddTransition(EnemyStatesConstants.Chase,chase);
        patrol.AddTransition(EnemyStatesConstants.Idle,idle);
        patrol.AddTransition(EnemyStatesConstants.Stun,stun);
        
        //Stun
        stun.AddTransition(EnemyStatesConstants.Idle,idle);

        //Chase 
        chase.AddTransition(EnemyStatesConstants.Idle,idle);
        chase.AddTransition(EnemyStatesConstants.Stun,stun);
        chase.AddTransition(EnemyStatesConstants.Attack,attack);
        
        //Attack
        attack.AddTransition(EnemyStatesConstants.Chase,chase);
        attack.AddTransition(EnemyStatesConstants.Stun,stun);
        attack.AddTransition(EnemyStatesConstants.Idle,idle);
        
        _fsm = new FSM<EnemyStatesConstants>(idle);
        
    }

    void InitDecisionTree()
    {
        // Actions

        var goToFollow = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Chase));
        var goToPatrol = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Patrol));
        var goToAttack = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Attack));
        var goToIdle = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Idle));
        var goToStun = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Stun));
        
        //Questions
        var CheckIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, goToIdle, goToPatrol);
         var DidSightChangeToLose = new QuestionNode(SightStateChanged, goToIdle, CheckIdleStateCooldown);
         var attemptPlayerKill = new QuestionNode(DistanceToPlayerEnoughToKill, goToAttack, goToFollow);
         var DidSightChangeToAttack = new QuestionNode(SightStateChanged, goToFollow, attemptPlayerKill);
      
         var IsInSight = new QuestionNode(LastInSightState, DidSightChangeToAttack, DidSightChangeToLose);
         var CheckStunned = new QuestionNode(IsStunned, goToStun, IsInSight);
         
         //Root 
         var IsPlayerAlive = new QuestionNode(() => target.LifeControler.IsAlive, CheckStunned, goToPatrol);
         
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
    private bool DistanceToPlayerEnoughToKill()
    {      
        //Checks distance to player. If within kill range, kill the player. Else starts pursuit state
        float rawDistance = (target.transform.position - transform.position).magnitude;
        //float rawDistance = (_player.transform.position - transform.position).sqrMagnitude;        
        return rawDistance <= minDistance; //_enemyModel.EnemyData.attemptToKillDistance; //HACER EL SCRIPTABLE OBJECT
        //return rawDistance <=_enemyModel.EnemyData.attemptToKillDistance * _enemyModel.EnemyData.attemptToKillDistance;
    }   
    public void BakeReferences()
    {
        _enemyModel = GetComponent<EnemyModel>();
        _enemyView = GetComponent<EnemyView>();

        Behaviour = new ObstacleAvoidance(transform, null, obstacleAvoidance.radius,
            obstacleAvoidance.maxObjs, obstacleAvoidance.obstaclesMask,
            obstacleAvoidance.multiplier, _enemyModel, obstacleAvoidance.timePrediction,
            ObstacleAvoidance.DesiredBehaviour.Seek);

    }
    // Update is called once per frame
    void Update()
    {
        if(target!=null) _fsm.UpdateState();

    }
}
