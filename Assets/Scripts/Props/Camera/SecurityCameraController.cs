using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    private SecurityCameraModel _cameraModel;
    [SerializeField] private PlayerModel _playerModel;

    public event Action<Node> OnAlert;

    [SerializeField] private CameraData _data;
    

    private FSM<CameraStates> _fsm;
    private INode _root;

    [SerializeField] private List<Node> closeWaypointNodes;
    

    private bool _previousInSightState;
    private bool _currentInSightState;

    private enum CameraStates
    {
        Surveillance,
        Alert
    }

    private Node GetNodeClosestToPlayer()
    {
        var minDistance = float.MaxValue;
        var index = 0;
        for (int i = 0; i < closeWaypointNodes.Count; i++)
        {
            var waypointNode = closeWaypointNodes[i];
            var newDistance = Vector3.Distance(waypointNode.transform.position, _playerModel.transform.position);
            if (newDistance > minDistance) continue;
            minDistance = newDistance;
            index = i;
        }

        return closeWaypointNodes[index];
    }
    private void OnAlertCommand()
    {
        var closesNodeToPlayer = GetNodeClosestToPlayer();
        OnAlert?.Invoke(closesNodeToPlayer);
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
 
    void Update()
    {
        _fsm.UpdateState();
    }

    public void GetNearNodes()
    {
        var nodes =Physics.OverlapSphere(transform.position, _data.nodesGetRadius, _data.nodesMask);

        closeWaypointNodes = new List<Node>();
        for (int i = 0; i < nodes.Length; i++)
        {
            var curr = nodes[i].GetComponent<Node>();
            closeWaypointNodes.Add(curr);
        }
        
        Debug.Log($"Got {closeWaypointNodes.Count} nodes");
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(SecurityCameraController))]
internal class CameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get Near Nodes"))
        {
            var curr = target as SecurityCameraController;
            curr.GetNearNodes();
            EditorUtility.SetDirty(curr);
        }
    }
}
#endif
