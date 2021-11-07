using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel _enemyModel;
    private FSM<EnemyStatesConstants> _fsm;
    private INode _root;
    [SerializeField] private PlayerModel target;
    [SerializeField] private Transform[] waypoints;
    private LineOfSightAI _lineOfSightAI;
    

    // Start is called before the first frame update
    void Awake()
    {
        BakeReferences();
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
        var idle = new EnemyIdleState<EnemyStatesConstants>();
        var patrol = new EnemyPatrolState<EnemyStatesConstants>(_enemyModel,waypoints,_root);
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
        INode walk = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Patrol));
        INode attack = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Attack));
        INode idle = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Idle));
        INode stun = new ActionNode(() => _fsm.Transition(EnemyStatesConstants.Stun));

        //Questions
        // INode isInIdle = new QuestionNode(IsInIdle, walk, idle); 
        // INode isInSight = new QuestionNode(IsInSight,chase,isInIdle); // Is the player in sight?      
        // INode isInRange = new QuestionNode(IsInRange,attack,isInSight); // Is in range to attack?

        //_root = isInRange;
    }
    public void BakeReferences()
    {
        _enemyModel = GetComponent<EnemyModel>();
        _lineOfSightAI = GetComponent<LineOfSightAI>();
    }
    // Update is called once per frame
    void Update()
    {
        // How we get player reference to stop?
        _fsm.UpdateState();

    }
}
