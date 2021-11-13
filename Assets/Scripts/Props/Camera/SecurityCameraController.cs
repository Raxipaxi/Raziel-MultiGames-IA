using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    private SecurityCameraModel _cameraModel;
    [SerializeField] private PlayerModel _playerModel;

    public event Action<Vector3> OnAlert;

    [SerializeField] private CameraData _data;
    

    private FSM<CameraStates> _fsm;
    private INode _root;
    
    private bool _previousInSightState;
    private bool _currentInSightState;
    public enum CameraStates
    {
        Surveillance,
        Alert
    }

    private void OnAlertCommand(Vector3 dir)
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
    
    private bool CheckSightState()
    {
        return _cameraModel.LineOfSightAI.SingleTargetInSight(_playerModel.transform);
    }

    private void InitDecisionTree()
    {

        //Actions
        var goToAlert = new ActionNode(() => _fsm.Transition(CameraStates.Alert));
        var goToSurveillance = new ActionNode(() => _fsm.Transition(CameraStates.Surveillance));
     
        //Question
        var isInSight = new QuestionNode(CheckSightState, goToAlert, goToSurveillance); 
        
        //Root 
        var isPlayerAlive = new QuestionNode(() => _playerModel.LifeControler.IsAlive, isInSight, goToSurveillance);
        _root = isPlayerAlive;
       
    }
    private void InitFsm()
    {
         //--------------- FSM Creation -------------------//                
        // States Creation
        var surveillance = new CameraSurveilanceState<CameraStates>(_cameraModel.LineOfSightAI, _playerModel,
            _data.playerVelocityThreshold, _data.checkPlayerMovementTime, _playerModel, _root);
        var alert = new CameraAlertState<CameraStates>(_cameraModel.LineOfSightAI, _playerModel, _root,
            _data.timeToResumeAlert, OnAlertCommand);

        //Surveillance
        surveillance.AddTransition(CameraStates.Alert, alert);
        //Alert
        alert.AddTransition(CameraStates.Surveillance, surveillance);

        _fsm = new FSM<CameraStates>(surveillance);
    }

    public void BakeReferences()
    {
        _cameraModel = GetComponent<SecurityCameraModel>();
    }
    // Update is called once per frame
    void Update()
    {
        _fsm.UpdateState();
    }
}
