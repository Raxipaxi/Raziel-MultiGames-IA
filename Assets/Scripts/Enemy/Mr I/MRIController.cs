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

    [SerializeField] private PathFinderAStarPlus _pathFinder;

    [SerializeField] private NodesChanceController nodesChanceController;

    [SerializeField] private float minimumWaypointDistance;
    [SerializeField] private float timeToCheckOnChase;

    [SerializeField] private LayerMask nodesMask;
    [SerializeField] private float radiusToCheckNodes;
    

    private Node _lastSeenPlayer;
    private Collider[] _closeNodes;
    
    [SerializeField] private PlayerModel target;
    private bool _waitForIdleState;

    public event Action OnIdleEnter;
    public event Action OnIdleSpin;
    public event Action<Vector3> OnMove;
    public event Action OnEnterFastState;
    public event Action OnChaseEnter;
    public event Action OnPatrolEnter;
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
        var nodesAmount = Physics.OverlapSphereNonAlloc(user.position, radiusToCheckNodes, _closeNodes, nodesMask);

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
            EnterGoToSpotState, () => target.LifeControler.IsAlive, minimumWaypointDistance, _model, LastSeenPlayerNode);

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

    private Node LastSeenPlayerNode()
    {
        return _lastSeenPlayer;
    }

    public void OnAlertedHandler(Node targetPosNode)
    {
        _isAlerted = true;
        _lastSeenPlayer = targetPosNode;
    }

    public GameObject Owner { get; private set; }
}
