﻿using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

[RequireComponent (typeof (Rigidbody))]

public class PlayerModel : MonoBehaviour, IMove,IVel
{
    private PlayerView _playerView;
    private Rigidbody _rb;
    private float _currentSpeed;
    [SerializeField] private Transform interactPoint;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private PlayerData playerData;

    private float _lastMoveMagnitude;
    
    public LifeController LifeControler { get; private set; }

    private Transform _selfTransform;
    private float _jumpCooldownCounter;

    private Vector3 _startingPoint;

  

    public event Action<float> OnMove;
    public float Vel
    {
        get => _lastMoveMagnitude;
        private set => _lastMoveMagnitude = value;
    }
    public void SubscribeToEvents(PlayerController controller)
    {
        controller.OnMove += Move;
        controller.OnJump += OnJumpHandler;
        controller.OnReset += OnResetHandler; 
        
        //Makes controller subscribe to death

        LifeControler.OnDead += controller.DeadBrain;
    }

    private void OnDeadHandler()
    {
        GameManager.Instance.ResetLevelState();
    }

    private void OnResetHandler()
    {
        transform.position = _startingPoint;
        LifeControler.Revive();
    }
    private void OnJumpHandler()
    {
        if (_jumpCooldownCounter > 0 || !IsGrounded()) return;
        _jumpCooldownCounter = playerData.jumpCooldown;
        _rb.AddForce(Vector3.up * playerData.jumpHeight, ForceMode.Impulse);
    }

    private void Start()
    {
        _playerView.SubscribeToEvents(this);
    }

    private void Awake()
    {
        BakeReferences();
        _startingPoint = transform.position;
        ResetState();
        LifeControler.OnDead += OnDeadHandler;
    }


  
    private void ResetState()
    {
        _currentSpeed = playerData.walkSpeed;
        _jumpCooldownCounter = playerData.jumpCooldown;
        LifeControler = new LifeController(playerData.maxLifes,gameObject);
    }
    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, playerData.isGroundedRadius, playerData.groundMask);
    }
   

    public void ChangeMoveSpeed(bool isRunSpeed)
    {
        _currentSpeed = isRunSpeed ? playerData.runSpeed : playerData.walkSpeed;
    }

    private void BakeReferences()
    {
        _playerView = GetComponent<PlayerView>();
        _rb = GetComponent<Rigidbody>();
        _selfTransform = transform;
    }

    private Collider[] _interactablesArray = new Collider[1];
    private Collider InteractableAtReach()
    {
        var interactablesLength = Physics.OverlapSphereNonAlloc(interactPoint.position, playerData.tryInteractRadius, _interactablesArray, playerData.tryInteractLayers);

        if (interactablesLength != 0) return _interactablesArray[0];
        return default;
    }
    public bool TryInteract()
    {
        var collidedInteractable = InteractableAtReach();
        return collidedInteractable != null;
    }
    public IInteractable GetInteractable()
    {   
        var interactable = _interactablesArray[0].GetComponent<IInteractable>();
        return interactable;
    }
    private float _rotationVelocity;
    private void CorrectRotation(Vector3 moveDir)
    {
        var targetRotation = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + GameManager.Instance.MainCamera.transform.eulerAngles.y;
        var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, playerData.rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

    }

    public void Move(Vector3 dir)
    {
        var normalizedDir = dir.normalized;
        CorrectRotation(normalizedDir);
        transform.position += normalizedDir * Time.deltaTime * _currentSpeed;
        var dirMagnitude = normalizedDir.magnitude;
        var moveMagnitude = _currentSpeed * dirMagnitude;
        Vel = moveMagnitude;
        OnMove?.Invoke(moveMagnitude);
    }

    private void Update()
    {
        _jumpCooldownCounter -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (playerData == null) return;

        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position,playerData.isGroundedRadius);
        }

        if (interactPoint == null) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(interactPoint.position, playerData.tryInteractRadius);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            LifeControler.GetDamage(10,true);
            return;
        }
    }

    
}