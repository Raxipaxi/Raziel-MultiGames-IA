using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel _enemyModel;
    private FSM<EnemyStatesConstants> _fsm;
    private INode _root;

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
        var walk = new EnemyWalkState<EnemyStatesConstants>();
        var chase = new EnemyChaseState<EnemyStatesConstants>();
        var dead = new EnemyDeadState<EnemyStatesConstants>();
        var attack = new EnemyAttackState<EnemyStatesConstants>();
        var stun = new EnemyStunState<EnemyStatesConstants>();
        
        //Idle State
        idle.AddTransition(EnemyStatesConstants.Walk,walk);
        idle.AddTransition(EnemyStatesConstants.Attack,attack);
        idle.AddTransition(EnemyStatesConstants.Dead,dead);
        idle.AddTransition(EnemyStatesConstants.Chase,chase);
        idle.AddTransition(EnemyStatesConstants.Stun,stun);
        
        //Walk
        walk.AddTransition(EnemyStatesConstants.Chase,chase);
        walk.AddTransition(EnemyStatesConstants.Idle,idle);
        walk.AddTransition(EnemyStatesConstants.Dead,dead);
        walk.AddTransition(EnemyStatesConstants.Stun,stun);
        
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
        INode walk = new ActionNode(()=> _fsm.Transition(EnemyStatesConstants.Walk));
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
    }
    // Update is called once per frame
    void Update()
    {
        // How we get player reference to stop?
        _fsm.UpdateState();

    }
}
