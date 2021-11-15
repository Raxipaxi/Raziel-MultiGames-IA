using System;
using UnityEngine;

public class ChocoboFlockingActive : MonoBehaviour
{
    private bool _active;
    private FlockingManager _flocking;
    private Leader _leaderBehaviour;
    
    public void SubscribeToEvents(ChocoboController controller)
    {
        controller.OnStartFollow += () => EnableDisable(true);
        controller.OnFollow += SetActiveLeader;
        controller.OnIdle += () => EnableDisable(false);
        controller.OnReachGoal+= ()=> EnableDisable(false);
    }

    private void Awake()
    {
        BakeReferences();
    }

    private void BakeReferences()
    {
        _flocking = GetComponent<FlockingManager>();
    }
    private void Start()
    {
        _leaderBehaviour = _flocking.GetComponent<Leader>();
    }

    private void EnableDisable(bool newState)
    {
        _flocking.enabled = newState;
    }

    private void SetActiveLeader(Transform newLeader)
    {
        _leaderBehaviour.leader = newLeader;
        
    }
}
