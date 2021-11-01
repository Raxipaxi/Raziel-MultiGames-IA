using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (Rigidbody))]

public class PlayerModel : MonoBehaviour
{
    private PlayerView _playerView;
    private Rigidbody _rb;
    private float _currentSpeed;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private PlayerData _playerData;

    public void SubscribeToEvents(PlayerController controller)
    {
        controller.OnMove += OnMoveHandler;
        controller.OnJump += OnJumpHandler;
    }

    

    private void OnJumpHandler()
    {
        Debug.Log("Jump");
    }
    private void OnMoveHandler(Vector3 moveDir)
    {
        var newPosition = transform.position + _currentSpeed * Time.deltaTime * moveDir;
        _rb.MovePosition(newPosition);
    }
    
    private void Awake()
    {
        BakeReferences();
        ResetState();
    }

    private void ResetState()
    {
        _currentSpeed = _playerData.walkSpeed;
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, _playerData.isGroundedRadius, _playerData.groundMask);
    }
   

    public void ChangeMoveSpeed(bool isRunSpeed)
    {
        _currentSpeed = isRunSpeed ? _playerData.runSpeed : _playerData.walkSpeed;
    }
    public void BakeReferences()
    {
        _playerView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody>();
    }

    private Collider[] _interactablesArray = new Collider[10];
    private Collider InteractableAtReach()
    {
        var interactablesLength = Physics.OverlapSphereNonAlloc(transform.position, _playerData.tryInteractRadius, _interactablesArray, _playerData.tryInteractLayers);

        if (interactablesLength != 0) return _interactablesArray[0];
        return default;
    }
    public bool TryInteract()
    {
        var collidedInteractable = InteractableAtReach();
        if (collidedInteractable != null) return true;
        return false;
    }
    public IInteractable GetInteractable()
    {   
        var interactable = _interactablesArray[0].GetComponent<IInteractable>();
        if (interactable != null) return interactable;
        return default;
    }
}