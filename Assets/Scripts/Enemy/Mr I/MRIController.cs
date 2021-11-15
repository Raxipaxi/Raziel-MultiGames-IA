using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIController : MonoBehaviour, IAlertable,IReseteable
{
    private MRIModel _model;
    
    private FSM<MriStates> _fsm;
    private INode _root;

    [SerializeField] private MrIData data;
    private ObstacleAvoidance Behaviour { get; set; }

    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidanceData;
    private bool _currentInSightState;
    private bool _previousInSightState;
    private bool _isAlerted;

    [SerializeField] private PathFinderAStarPlus _pathFinder;
    
    

    [SerializeField] private NodesChanceController nodesChanceController;


    [SerializeField] private Transform obstacleAvoidancePivot;

    private Node _lastSeenPlayer;
    private Collider[] _closeNodes = new Collider[5];
    
    [SerializeField] private PlayerModel target;
    private bool _waitForIdleState;

    public event Action OnIdleEnter;
    public event Action OnIdleSpin;
    public event Action<Vector3> OnMove;
    public event Action OnEnterFastState;
    public event Action OnChaseEnter;
    public event Action OnPatrolEnter;

    public event Action OnResetLevel;
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

    private void OnPatrol()
    {
        OnPatrolEnter?.Invoke();
    }

    private Node GetClosestNodeToController(Transform user)
    {
        var nodesAmount = Physics.OverlapSphereNonAlloc(user.position, data.radiusToCheckNodes, _closeNodes, data.nodesMask);

        if (nodesAmount == 0) return default;
        Node nodeToReturn;
        var index = 0;
        
        if (nodesAmount > 1)
        {
            var distance = float.MaxValue;
            
            for (int i = 0; i < nodesAmount; i++)
            {
                var currNodeCollider = _closeNodes[i];
                var currDistance = Vector3.Distance(user.position, currNodeCollider.transform.position);
                if (currDistance > distance) continue;
                distance = currDistance;
                index = i;
            }
        }
        nodeToReturn = _closeNodes[index].GetComponent<Node>();
        return nodeToReturn;
    }
    private List<Node> GetWaypointsToCertainNode(Node targetNode)
    {
        var closeNode = GetClosestNodeToController(transform);
        var list = _pathFinder.GetPathAStarPlus(closeNode, targetNode);
        return list;
    }
    private Node GetRandomNode()
    {
        return nodesChanceController.GetRandomNode();
    }

    private enum MriStates
    {
        Idle,
        Patrol,
        Chase,
        GoToSightSpot
    }

    private void Awake()
    {
        BakeReferences();
    }

    public void BakeReferences()
    {
        _model = GetComponent<MRIModel>();
        Owner = gameObject;
        Behaviour = new ObstacleAvoidance(obstacleAvoidancePivot, null, obstacleAvoidanceData.radius, obstacleAvoidanceData.maxObjs,
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
        _currentInSightState = IsPlayerOnSight();
        return _currentInSightState;
    }

    private bool IsAlerted()
    {
        return _isAlerted;
    }

    private void SetIsAlerted(bool newState)
    {
        _isAlerted = newState;
    }

    private bool IsIdleStateCooldown()
    {
        return _waitForIdleState;
    }

    private bool IsPlayerOnSight()
    {
        var isPlayerInSight = _model.LineOfSightAI.SingleTargetInSight(target.transform);
        return isPlayerInSight;
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

        var idleState = new MrIIdleState<MriStates>(EnterIdle, Spin, IsPlayerOnSight, _root, SetIdleStateCooldown, data.timeToOutOfIdle);
        var patrolState = new MrIPatrolState<MriStates>(IsPlayerOnSight, _root, GetWaypointsToCertainNode,
            SetIdleStateCooldown, GetRandomNode, _model, data.minimumWaypointDistance, Behaviour, Move,OnPatrol);
        var chaseState = new MrIChaseState<MriStates>(IsPlayerOnSight, _root, SetIdleStateCooldown, Move, Behaviour,
            data.timeToCheckOnChase, EnterChase, target, () => target.LifeControler.IsAlive);
        var goToSighSpotState = new MrIGoToSpotState<MriStates>(_root, Move, GetWaypointsToCertainNode, Behaviour,
            EnterGoToSpotState, () => target.LifeControler.IsAlive, data.minimumWaypointDistance, _model, LastSeenPlayerNode,SetIsAlerted,SetIdleStateCooldown);

        //Transitions
        
        idleState.AddTransition(MriStates.Patrol, patrolState);
        idleState.AddTransition(MriStates.GoToSightSpot,goToSighSpotState);
        idleState.AddTransition(MriStates.Chase,chaseState);
        
        patrolState.AddTransition(MriStates.Idle,idleState);
        patrolState.AddTransition(MriStates.Chase,chaseState);
        patrolState.AddTransition(MriStates.GoToSightSpot,goToSighSpotState);
        
        chaseState.AddTransition(MriStates.Idle,idleState);

        goToSighSpotState.AddTransition(MriStates.Idle,idleState);
        goToSighSpotState.AddTransition(MriStates.Chase, chaseState);
        goToSighSpotState.AddTransition(MriStates.Patrol,patrolState);

        _fsm = new FSM<MriStates>(idleState);
    }

    private Node LastSeenPlayerNode()
    {
        return _lastSeenPlayer;
    }

    public void OnAlertedHandler(Node targetPosNode)
    {
        SetIsAlerted(true);
        _lastSeenPlayer = targetPosNode;
        _root.Execute();
    }

    public void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        _fsm.UpdateState();
    }

    public GameObject Owner { get; private set; }
    public void OnLevelReset()
    {
        SetIdleStateCooldown(true);
        OnResetLevel?.Invoke();
        _root.Execute();
    }
}
