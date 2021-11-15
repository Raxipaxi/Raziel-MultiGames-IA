using System;
using UnityEngine;

public class ChocoboFlockingActive : MonoBehaviour
{
    private bool _active;
    private FlockingManager _flocking;
    private Leader _naranaranaraliiiider;
    
    public void SubscribeToEvents(ChocoboController _controller)
    {
        _controller.OnFollowTr += SetActiveFlocking;
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
        _active = false;
        
        _naranaranaraliiiider = _flocking.GetComponent<Leader>(); //Mmm creo que mande frula
        ActivateorDisableFlock();

    }


    private void Update()
    {
        if (_active && !_flocking.gameObject.activeSelf) 
        {
            
               ActivateorDisableFlock();
        }
        else if (!_active)
        {
           ActivateorDisableFlock();
        }
    }

    void ActivateorDisableFlock()
    {
        _flocking.enabled = _active;
    }
    

    public void SetActiveFlocking(Transform dir)
    {
        _naranaranaraliiiider.leader = dir;
        _active = !_active;
    }
}
