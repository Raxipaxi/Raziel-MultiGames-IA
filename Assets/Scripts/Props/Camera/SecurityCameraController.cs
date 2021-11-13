using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    private SecurityCameraModel _cameraModel;
   [SerializeField] private PlayerModel _playerModel;

    public event Action<Vector3> OnAlert;

    [SerializeField] private float timeToResumeAlert;

    private FSM<CameraStates> _fsm;
    
    private bool _previousInSightState;
    private bool _currentInSightState;
    public enum CameraStates
    {
        Surveillance,
        Alert
    }
    public void OnAlertCommand(Vector3 dir)
    {
        OnAlert?.Invoke(dir);
        Debug.Log("Saw player");
    }

    private void Awake()
    {
        BakeReferences();
    }

    private void Start()
    {
        _cameraModel.SubscribeToEvents(this);
        InitDecisionTree();
        InitFsm();
    }

    private void InitFsm()
    {
         //--------------- FSM Creation -------------------//                
        // States Creation
        //var surveillance = new CameraSurveilanceState<CameraStates> ()
        //var alert = new CameraAlertState<CameraStates>();

        //Surveillance
        //surveillance.AddTransition(CameraStates.Alert, alert);
        //Alert
        //alert.AddTransition(CameraStates.Surveillance, surveillance);

        //_fsm = new FSM<CameraStates>(surveillance);
    }

    private bool SightStateChanged()
    { 
        return _currentInSightState != _previousInSightState;
    }

    private bool LastInSightState()
    {     
        _previousInSightState = _currentInSightState;    
        _currentInSightState = _cameraModel.LineOfSightAI.SingleTargetInSight(_playerModel.transform);
        return _currentInSightState;
    }
    private void InitDecisionTree()
    {
        /*
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
        _root = IsPlayerAlive;*/
    }
    public void BakeReferences()
    {
        _cameraModel = GetComponent<SecurityCameraModel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraModel.LineOfSightAI.SingleTargetInSight(_playerModel.transform))
        {
            OnAlertCommand(_playerModel.transform.position);
        }   
    }
}
