using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;
    private FSM<PlayerStatesConstants> _fsm;


   [SerializeField] private PlayerControllerData _controllerData;

    
    public event Action<Vector3> OnMove;
    public event Action OnJump;


    private void Awake()
    {
        BakeReferences();
    }

    private void Start()
    {
        InitFSM();
        _playerModel.SubscribeToEvents(this);
    }

    private void IsRunning (bool runState)
    {
        _playerModel.ChangeMoveSpeed(runState);
    }

    private bool TryInteraction()
    {
        Debug.Log("Try interaction");
        return _playerModel.TryInteract();
    }

    private IInteractable GetInteractable()
    {
        Debug.Log("GetInteractable");
        return _playerModel.GetInteractable();
    }
    private void OnMoveCommand(Vector3 moveDir)
    {
        OnMove?.Invoke(moveDir);
    }

    private void OnJumpCommand()
    {
        OnJump?.Invoke();
    }

    private bool IsGrounded()
    {
        return _playerModel.IsGrounded();
    }
    private void InitFSM()
    {
        //--------------- FSM Creation -------------------//                
        // States Creation
        var idle = new PlayerIdleState<PlayerStatesConstants>(PlayerStatesConstants.Move, PlayerStatesConstants.Jump, PlayerStatesConstants.Dead, PlayerStatesConstants.Interact, TryInteraction,_controllerData,IsRunning);
        var move = new PlayerMoveState<PlayerStatesConstants>(OnMoveCommand, PlayerStatesConstants.Idle, PlayerStatesConstants.Jump, PlayerStatesConstants.Interact, TryInteraction, IsRunning,_controllerData);
        var interact = new PlayerInteractAndWait<PlayerStatesConstants>(GetInteractable, PlayerStatesConstants.Idle);
        var jump = new PlayerJumpState<PlayerStatesConstants>(PlayerStatesConstants.Idle, PlayerStatesConstants.Move, OnJumpCommand, IsGrounded, OnMoveCommand, IsRunning);
        var dead = new PlayerDeadState<PlayerStatesConstants>();

        // Idle State
        idle.AddTransition(PlayerStatesConstants.Move, move);
        idle.AddTransition(PlayerStatesConstants.Jump, jump);
        idle.AddTransition(PlayerStatesConstants.Interact, interact);
        idle.AddTransition(PlayerStatesConstants.Dead, dead);

        // Move State
        move.AddTransition(PlayerStatesConstants.Idle, idle);
        move.AddTransition(PlayerStatesConstants.Dead, dead);
        move.AddTransition(PlayerStatesConstants.Interact, interact);
        move.AddTransition(PlayerStatesConstants.Jump, jump);

        // Interact State
        interact.AddTransition(PlayerStatesConstants.Idle, idle);
        interact.AddTransition(PlayerStatesConstants.Dead, dead);


        // Jump State
        jump.AddTransition(PlayerStatesConstants.Idle, idle);
        jump.AddTransition(PlayerStatesConstants.Move, move);
        jump.AddTransition(PlayerStatesConstants.Dead, dead);

        

        _fsm = new FSM<PlayerStatesConstants>(idle);

    }

    public void BakeReferences()
    {
        _playerModel = GetComponent<PlayerModel>();
    }


    private void Update()
    {
        if (GameManager.Instance.IsPaused) return;
        
        _fsm.UpdateState();
    }
}


