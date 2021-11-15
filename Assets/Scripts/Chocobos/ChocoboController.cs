using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboController : MonoBehaviour, IReseteable
{
    private ChocoboModel _chocoModel;
    [Header("Minima distancia con el lider")]
    [SerializeField] private float followCheck;
    private ChocoboFlockingActive _flockingActive;
    private FSM<ChocoboStatesConstants> _fsm;
    private INode _root;

    [SerializeField] private ChocoboData data;
    [SerializeField] private Transform player;
    [SerializeField] private Transform checkForPlayerPoint;
    private Collider[] _playerAsLeader;

    private bool _reachedGoal;
    public event Action<Transform> OnFollow;
    public event Action OnIdle;
    public event Action OnReset;
    public event Action OnStartFollow;
    public event Action OnReachGoal;

    private void Awake()
    {
        BakeReferences();
    }

    private void OnStartFollowCommand()
    {
        OnStartFollow?.Invoke();
    }
    private void OnIdleCommand()
    {
        OnIdle?.Invoke();
    }

    private void OnFollowCommand(Transform tr)
    {
        OnFollow?.Invoke(tr);
    }

    private void Start()
    {
        _flockingActive.SubscribeToEvents(this);
        _chocoModel.SubscribeToEvents(this);
        DecisionTreeInit();
        FsmInit();
    }

    private void DecisionTreeInit()
    {
        //Actions
        var goToIdle = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Idle));
        var goToFollow = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Follow));
        var goToReachedGoal = new ActionNode(() => _fsm.Transition(ChocoboStatesConstants.Dance));
        
        //Questions
        
        var isAnyLeader = new QuestionNode(CheckForPlayer, goToFollow, goToIdle);
        var reachedGoal = new QuestionNode(() => _reachedGoal, goToReachedGoal, isAnyLeader);
        
        //Root
        _root = reachedGoal;
    }

    public void ReachedGoal()
    {
        //Callable from outside
        _reachedGoal = true;
        _root.Execute();
    }

    private void ReachGoal()
    {
        OnReachGoal?.Invoke();
    }
    private void FsmInit()
    {
        // States
        var idle = new ChocoboIdleState<ChocoboStatesConstants>(data.secondsToFollow, OnIdleCommand, OnFollowCommand , _root,player,CheckForPlayer);
        var follow = new ChocoboFollowState<ChocoboStatesConstants>(OnFollowCommand, followCheck, _root, CheckForPlayer,
            OnStartFollowCommand, player);
        var reachedGoal = new EnemyDeadState<ChocoboStatesConstants>(ReachGoal);
        
        // Transitions
        // Idle
        idle.AddTransition(ChocoboStatesConstants.Follow,follow);
        idle.AddTransition(ChocoboStatesConstants.Dance,reachedGoal);
        //Follow
        follow.AddTransition(ChocoboStatesConstants.Idle, idle);
        follow.AddTransition(ChocoboStatesConstants.Dance,reachedGoal);

        _fsm = new FSM<ChocoboStatesConstants>(idle);
    }

    private bool CheckForPlayer()
    {
        var playerIsNear = Physics.OverlapSphereNonAlloc(checkForPlayerPoint.position, data.checkForPlayerRadius,
            _playerAsLeader, data.playerMask);
        return playerIsNear != 0;
    }
    private void BakeReferences()
    {
        _chocoModel = GetComponent<ChocoboModel>();
        _flockingActive = GetComponent<ChocoboFlockingActive>();
        _playerAsLeader = new Collider[1];
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        _fsm.UpdateState();
    }

    public void OnLevelReset()
    {
        if (!_reachedGoal)
        {
            OnReset?.Invoke();
            _root.Execute();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkForPlayerPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(checkForPlayerPoint.position,data.checkForPlayerRadius);
        }
    }
}
