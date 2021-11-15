using System;
using UnityEngine;

public class ChocoboFlockingActive : MonoBehaviour
{
    private bool _active;
    private FlockingManager _flocking;
    private Leader _LeaderBehaviour;
    
    public void SubscribeToEvents(ChocoboController _controller)
    {
        _controller.OnFollow += SetActiveFlocking;
    }

    private void Awake()
    {
        BakeReferences();
    }

    public void BakeReferences()
    {
        _flocking = GetComponent<FlockingManager>();
    }

    private void Start()
    {
        
        _LeaderBehaviour = _flocking.GetComponent<Leader>(); //Mmm creo que mande frula

    }
    
    private void EnableDisable()
    {
        if (_LeaderBehaviour.leader!=null && !_flocking.enabled)
        {
            _flocking.enabled = true;
            
        }
        else if (_LeaderBehaviour.leader==null)
        {
            _flocking.enabled = false;
        }
    }

    

    public void SetActiveFlocking(Transform dir)
    {
        _LeaderBehaviour.leader = dir;
        EnableDisable();
    }
}
