using System;
using UnityEngine;
public class PlayerIdleState<T> : State<T>
{
    private T _transitionToMove;
    private T _transitionToJump;
    private T _transitionToInteract;

    private Action <bool>_isRunning;
    private PlayerControllerData _data;

    
    private Func<bool> _tryInteract;
    private PlayerController _controller;
    //Input for this one comes from outside
    private T _transitionToDead;

 
    public PlayerIdleState(T transitionToMove, T transitionToJump, T transitionToDead, T transitionToInteract, Func <bool> TryInteract, PlayerControllerData data, Action <bool> isRunning)
    {
        _transitionToMove = transitionToMove;
        _transitionToInteract = transitionToInteract;
        _transitionToDead = transitionToDead;
        _transitionToJump = transitionToJump;
        _tryInteract = TryInteract;
        _data = data;
        _isRunning = isRunning;
    }

    public override void Execute()
    {
        _isRunning?.Invoke(Input.GetKey(KeyCode.LeftShift));
        
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {        
            parentFSM.Transition(_transitionToMove);
        }

        if (Input.GetKeyDown(_data.jump))
        {
            parentFSM.Transition(_transitionToJump);
        }

        if (Input.GetKeyDown(_data.interact) && _tryInteract())
        {
            parentFSM.Transition(_transitionToInteract);
        }
    }

   

}
