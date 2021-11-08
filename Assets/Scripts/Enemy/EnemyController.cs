using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel _enemyModel;
    private EnemyView _enemyView;
    private FSM<EnemyStatesConstants> _fsm;
    
    private INode _root;

    [SerializeField] private float idleLenght;
    

    #region Steering Properties
    [SerializeField] private PlayerModel target;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float minDistance;
    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidance;
    public ObstacleAvoidance Behaviour { get; private set; }
    private LineOfSightAI _lineOfSightAI;

    #endregion
    
    
    private bool _previousInSightState;
    private bool _currentInSightState;

    #region Actions

    public event Action<Vector3> OnWalk;
    public event Action OnIdle;
    

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        BakeReferences();
    }
    private void OnWalkCommand(Vector3 moveDir)
    {
        OnWalk.Invoke(moveDir);
    }    
    private void OnIdleCommand()
    {
        OnIdle.Invoke();
    }
    private void Start()
    {
        InitFSM();
        InitDecisionTree();
        //Events subs??
    }

    void InitFSM()
    {
        //--------------- FSM Creation -------------------//                
        // States Creation
        var idle = new EnemyIdleState<EnemyStatesConstants>(idleLenght, _lineOfSightAI, target.transform, OnIdleCommand, _root);
        var patrol = new EnemyPatrolState<EnemyStatesConstants>(_enemyModel,target.transform,waypoints, OnWalkCommand,minDistance,_root);
        var chase = new EnemyChaseState<EnemyStatesConstants>();
        var dead = new EnemyDeadState<EnemyStatesConstants>();
        var attack = new EnemyAttackState<EnemyStatesConstants>();
        var stun = new EnemyStunState<EnemyStatesConstants>();
        
        //Idle State
        idle.AddTransition(EnemyStatesConstants.Patrol,patrol);
        idle.AddTransition(EnemyStatesConstants.Attack,attack);
        idle.AddTransition(EnemyStatesConstants.Dead,dead);
        idle.AddTransition(EnemyStatesConstants.Chase,chase);
        idle.AddTransition(EnemyStatesConstants.Stun,stun);
        
        //Patrol
        patrol.AddTransition(EnemyStatesConstants.Chase,chase);
        patrol.AddTransition(EnemyStatesConstants.Idle,idle);
        patrol.AddTransition(EnemyStatesConstants.Dead,dead);
        patrol.AddTransition(EnemyStatesConstants.Stun,stun);
        
        //Stun
        stun.AddTransition(EnemyStatesConstants.Idle,idle);
        stun.AddTransition(EnemyStatesConstants.Dead,dead);
        
        //Chase 
        chase.AddTransition(EnemyStatesConstants.Idle,idle);
        chase.AddTransition(EnemyStatesConstants.Stun,stun);
        chase.AddTransition(EnemyStatesConstants.Dead,dead);
        chase.AddTransition(EnemyStatesConstants.Attack,attack);
        
        //Attack
        attack.AddTransition(EnemyStatesConstants.Chase,chase);
        attack.AddTransition(EnemyStatesConstants.Dead,dead);
        attack.AddTransition(EnemyStatesConstants.Stun,stun);
        attack.AddTransition(EnemyStatesConstants.Idle,idle);
        
        _fsm = new FSM<EnemyStatesConstants>(idle);
        
    }

    void InitDecisionTree()
    {
        // Actions

        INode follow = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Chase));
        INode patrol = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Patrol));
        INode attack = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Attack));
        INode idle = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Idle));
        INode stun = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Stun));

        //Questions
        // INode isInIdle = new QuestionNode(IsInIdle, patrol, idle); 
        // INode isInSight = new QuestionNode(IsInSight,chase,isInIdle); // Is the player in sight?      
        // INode isInRange = new QuestionNode(IsInRange,attack,isInSight); // Is in range to attack?

        //_root = isInRange;
    }
    
    private bool SightStateChanged()
    {
      
        return (_currentInSightState != _previousInSightState);
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
        return rawDistance <= 3f; //_enemyModel.EnemyData.attemptToKillDistance; //HACER EL SCRIPTABLE OBJECT
        //return rawDistance <=_enemyModel.EnemyData.attemptToKillDistance * _enemyModel.EnemyData.attemptToKillDistance;
    }   
    public void BakeReferences()
    {
        _enemyModel = GetComponent<EnemyModel>();
        _enemyView = GetComponent<EnemyView>();
        _lineOfSightAI = GetComponent<LineOfSightAI>();
        
        Behaviour = new ObstacleAvoidance(transform, target.transform, obstacleAvoidance.radius,
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
