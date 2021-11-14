using System;
using UnityEngine;

public class ChocoboFlockingActive : MonoBehaviour
{
    private bool _active;
    private FlockingManager _flocking;
    private Leader _naranaranaraliiiider;



    public void Subscribe(ChocoboController _controller)
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
        _active = false;
        _naranaranaraliiiider = _flocking.GetComponent<Leader>(); //Mmm creo que mande frula
    }


    private void Update()
    {
        if (_active && !_flocking.gameObject.activeSelf) 
        {
            
               _flocking.gameObject.SetActive(_active);
        }
        else if (!_active)
        {
            _flocking.gameObject.SetActive(_active);
        }
    }
    
    
    

    public void SetActiveFlocking(Transform dir)
    {
        _naranaranaraliiiider.leader = dir;
        _active = !_active;
    }
}
