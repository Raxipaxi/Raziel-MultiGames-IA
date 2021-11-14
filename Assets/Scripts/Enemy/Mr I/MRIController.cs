using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIController : MonoBehaviour, IAlertable
{
    private MRIModel _model;
    
    private FSM<MriStates> _fsm;
    private INode _root;
    private ObstacleAvoidance Behaviour { get; set; }

    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidanceData;
    private bool _currentInSightState;
    private bool _previousInSightState;
    private bool _isAlerted;

    [SerializeField] private float minimumWaypointDistance;
    [SerializeField] private float timeToCheckOnChase;
    
    

    private Node _lastSeenPlayer;
    
    [SerializeField] private PlayerModel target;
    private bool _waitForIdleState;

    public event Action OnIdleEnter;
    public event Action OnIdleSpin;
    public event Action<Vector3> OnMove;
    public event Action OnEnterFastState;

    public event Action OnChaseEnter;
    private void EnterIdle()
    {
        OnIdleEnter?.Invoke();
    }

    private void EnterGoToSpotState()
    {
        OnEnterFastState?.Invoke();
    }
    private void EnterChase()
    {
        OnChaseEnter?.Invoke();
    }

    private void Spin()
    {
        OnIdleSpin?.Invoke();
    }

    private void Move(Vector3 dir)
    {
        OnMove?.Invoke(dir);
    }

    private List<Node> GetWaypointsToCertainNode(Node node)
    {
        var list = new List<Node>();
        return list;
    }

    private Node GetRandomNode()
    {
        Node n = new Node();

        return n;
    }

    private enum MriStates
    {
        Idle,
        Patrol,
        Chase,
        GoToSightSpot
    }
    public void BakeReferences()
    {
        _model = GetComponent<MRIModel>();
        Owner = gameObject;
        Behaviour = new ObstacleAvoidance(transform, null, obstacleAvoidanceData.radius, obstacleAvoidanceData.maxObjs,
            obstacleAvoidanceData.obstaclesMask, obstacleAvoidanceData.multiplier, target,
            obstacleAvoidanceData.timePrediction, obstacleAvoidanceData._defaultBehaviour);
    }

    private void Start()
    {
        _model.SubscribeToEvents(this);
        InitTree();
        InitFsm();
    }
    
    private bool SightStateChanged()
    { 
        return _currentInSightState != _previousInSightState;
    }

    private bool LastInSightState()
    {     
        _previousInSightState = _currentInSightState;    
        _currentInSightState = _model.LineOfSightAI.SingleTargetInSight(target.transform);
        return _currentInSightState;
    }

    private bool IsAlerted()
    {
        return _isAlerted;
    }

    private bool IsIdleStateCooldown()
    {
        return _waitForIdleState;
    }

    private void SetIdleStateCooldown(bool newState)
    {
        _waitForIdleState = newState;
    }
    private void InitTree()
    {
        var goToIdle = new ActionNode(() => _fsm.Transition(MriStates.Idle));
        var goToPatrol = new ActionNode(() => _fsm.Transition(MriStates.Patrol));
        var goToChase = new ActionNode(() => _fsm.Transition(MriStates.Chase));
        var goToSightSpot = new ActionNode(() => _fsm.Transition(MriStates.GoToSightSpot));
        
        var checkIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, goToIdle, goToPatrol);
        var didSightChangeToFalse = new QuestionNode(SightStateChanged, goToIdle, checkIdleStateCooldown);

        var isInSight = new QuestionNode(LastInSightState, goToChase, didSightChangeToFalse);
        var isAlerted = new QuestionNode(IsAlerted, goToSightSpot, isInSight);
        var isPlayerAlive = new QuestionNode(() => target.LifeControler.IsAlive, isAlerted, goToIdle);

        _root = isPlayerAlive;
    }

    private void InitFsm()
    {
        //States

        var idleState = new MrIIdleState<MriStates>(EnterIdle, Spin, LastInSightState, _root, SetIdleStateCooldown);
        var patrolState = new MrIPatrolState<MriStates>(LastInSightState, _root, GetWaypointsToCertainNode,
            SetIdleStateCooldown, GetRandomNode, _model, minimumWaypointDistance);
        var chaseState = new MrIChaseState<MriStates>(LastInSightState, _root, SetIdleStateCooldown, Move, Behaviour,
            timeToCheckOnChase, EnterChase, target, () => target.LifeControler.IsAlive);
        var goToSighSpotState = new MrIGoToSpotState<MriStates>(_root, Move, GetWaypointsToCertainNode, Behaviour,
            EnterGoToSpotState, () => target.LifeControler.IsAlive, minimumWaypointDistance, _model);

        //Transitions
        
        idleState.AddTransition(MriStates.Patrol, patrolState);
        idleState.AddTransition(MriStates.GoToSightSpot,goToSighSpotState);
        idleState.AddTransition(MriStates.Chase,chaseState);
        
        patrolState.AddTransition(MriStates.Idle,idleState);
        patrolState.AddTransition(MriStates.Chase,chaseState);
        patrolState.AddTransition(MriStates.GoToSightSpot,goToSighSpotState);
        
        chaseState.AddTransition(MriStates.Idle,idleState);
        
        goToSighSpotState.AddTransition(MriStates.Idle,idleState);

        _fsm = new FSM<MriStates>(idleState);
    }

    private Node LastSeenPlayer()
    {
        return _lastSeenPlayer;
    }
    public void OnAlertedHandler(Vector3 targetPos)
    {
        //Sets Nearest Node
        _isAlerted = true;
        Node nearestNode = new Node();
        _lastSeenPlayer = nearestNode;
    }

    public GameObject Owner { get; private set; }
}
