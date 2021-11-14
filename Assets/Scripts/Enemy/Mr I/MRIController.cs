using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRIController : MonoBehaviour, IAlertable
{
    private MRIModel _model;
    
    private FSM<MRIStates> _fsm;
    private INode _root;
    public ObstacleAvoidance Behaviour { get; private set; }

    [SerializeField] private ObstacleAvoidanceScriptableObject obstacleAvoidanceData;
    private bool _currentInSightState;
    private bool _previousInSightState;
    private bool _isAlerted;
   

    private Vector3 _lastSeenPlayer;
    
    [SerializeField] private PlayerModel target;
    private bool _waitForIdleState;


    private List<Vector3> GetWaypointTrack()
    {
        return new List<Vector3>();
    }

    private enum MRIStates
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
        InitTree();
        InitFSM();
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
        var goToIdle = new ActionNode(() => _fsm.Transition(MRIStates.Idle));
        var goToPatrol = new ActionNode(() => _fsm.Transition(MRIStates.Patrol));
        var goToChase = new ActionNode(() => _fsm.Transition(MRIStates.Chase));
        var goToSightSpot = new ActionNode(() => _fsm.Transition(MRIStates.GoToSightSpot));
        
        var checkIdleStateCooldown = new QuestionNode(IsIdleStateCooldown, goToIdle, goToPatrol);
        var didSightChangeToFalse = new QuestionNode(SightStateChanged, goToIdle, checkIdleStateCooldown);

        var isInSight = new QuestionNode(LastInSightState, goToChase, didSightChangeToFalse);
        var isAlerted = new QuestionNode(IsAlerted, goToSightSpot, isInSight);
        var isPlayerAlive = new QuestionNode(() => target.LifeControler.IsAlive, isAlerted, goToIdle);

        _root = isPlayerAlive;
    }

    private void InitFSM()
    {
        //States
        MrIPatrolState<>


    }

    private Vector3 LastSeenPlayer()
    {
        return _lastSeenPlayer;
    }
    public void OnAlertedHandler(Vector3 targetPos)
    {
        _isAlerted = true;
        _lastSeenPlayer = targetPos;
    }

    public GameObject Owner { get; private set; }
}
