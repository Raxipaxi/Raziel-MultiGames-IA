using System;
using UnityEngine;


public class PlayerInteractAndWait<T> : State<T>
{
    private T _transitionToIdle;

    private IInteractable _currentInteractable;

    

    private Func <IInteractable> _getInteractable;
    public PlayerInteractAndWait (Func <IInteractable> getInteractable, T transitionToIdle)
    {
        _getInteractable = getInteractable;
        _transitionToIdle = transitionToIdle;                           
    } 

    public override void Awake()
    {
        _currentInteractable = _getInteractable();

        if (_currentInteractable == null || _currentInteractable.OnInteract())
        {
            parentFSM.Transition(_transitionToIdle);
        }        
    }
    public override void Execute()
    {
        if (Input.GetKeyDown(KeyCode.E) && _currentInteractable.OnTriggerContinueInteract())
        {
            parentFSM.Transition(_transitionToIdle);
        }
    }

    public override void Sleep()
    {
        _currentInteractable = default;
    }




}
