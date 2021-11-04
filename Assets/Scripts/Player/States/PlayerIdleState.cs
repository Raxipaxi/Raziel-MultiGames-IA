using System;
using UnityEngine;
public class PlayerIdleState<T> : State<T>
{
    private T _transitionToMove;
    private T _transitionToJump;
    private T _transitionToInteract;

    private Action <bool>_isRunning;

    
    private Func<bool> _tryInteract;
    private PlayerController _controller;
    //Input for this one comes from outside
    private T _transitionToDead;

 
    public PlayerIdleState(T transitionToMove, T transitionToJump, T transitionToDead, T transitionToInterract, Func <bool> TryInteract, PlayerController controller, Action <bool> isRunning)
    {
        _transitionToMove = transitionToMove;
        _transitionToInteract = transitionToInterract;
        _transitionToDead = transitionToDead;
        _transitionToJump = transitionToJump;
        _tryInteract = TryInteract;
        _controller = controller;
        _isRunning = isRunning;
    }

    public override void Execute()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRunning?.Invoke(true);
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRunning?.Invoke(false);
        }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {        
            parentFSM.Transition(_transitionToMove);
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            parentFSM.Transition(_transitionToJump);
        }

        else if (Input.GetKeyDown(KeyCode.E) && _tryInteract())
        {
            parentFSM.Transition(_transitionToInteract);
        }
    }

   

}
