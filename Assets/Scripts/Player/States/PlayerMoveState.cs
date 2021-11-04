using UnityEngine;
using System;
public class PlayerMoveState<T> : State<T>
{
    private Action<Vector3> _onMove;
    private T _transitionToIdle;
    private T _transitionToJump;
    private T _transitionToInteract;
    private Func<bool> _tryInteract;
    private Action <bool>_isRunning;

    public PlayerMoveState (Action <Vector3> OnMove, T transitionToIdle, T transitionToJump, T transitionToInteract,Func <bool> tryInteract, Action<bool> isRunning)
    {
        _onMove = OnMove;
        _transitionToIdle = transitionToIdle;
        _transitionToJump = transitionToJump;
        _tryInteract = tryInteract;
        _isRunning = isRunning;
    }

    public override void Execute()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isRunning?.Invoke(true);
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isRunning?.Invoke(false);
        }

        var moveVector = new Vector3(x,0,z);        
        _onMove?.Invoke(moveVector); 

        if (x == 0 && z == 0)
        {
            parentFSM.Transition(_transitionToIdle);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            parentFSM.Transition (_transitionToJump);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && _tryInteract())
        {
            parentFSM.Transition(_transitionToInteract);
        }


    }
}
