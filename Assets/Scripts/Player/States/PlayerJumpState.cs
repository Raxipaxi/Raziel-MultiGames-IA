using System;
using UnityEngine;

public class PlayerJumpState<T> : State<T>
{
    private Action _onJump;
    private T _transitionToIdle;
    private T _transitionToMove;
    private Func<bool> _isGrounded;
    private Action<bool> _isRunning;
    private Action<Vector3> _onMove;
    public PlayerJumpState (T transitionToIdle, T transitionToMove, Action onJump, Func <bool> isGrounded,  Action <Vector3> onMove, Action <bool> isRunning)
    {
        _transitionToIdle = transitionToIdle;
        _transitionToMove = transitionToMove;
        _onJump = onJump;
        _isGrounded = isGrounded;
        _isRunning = isRunning;
        _onMove = onMove;
    }


    public override void Awake()
    {
        _onJump?.Invoke();
        //Set speed to not running? Check afterwards
        _isRunning?.Invoke(false);
    }

    public override void Execute()
    {
        
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        var moveDir = new Vector3(x, 0, z);

        _onMove?.Invoke(moveDir);

        if (_isGrounded())
        {
            parentFSM.Transition(moveDir != Vector3.zero ? _transitionToMove : _transitionToIdle);
        }
    }

}
