using System;
using UnityEngine;

public class EnemyModel : MonoBehaviour, IVel
{
    private EnemyView _enemyView;
    private Rigidbody _rb;
    public float Vel => _rb.velocity.magnitude;
    
    private float _currSpeed = 0f;

  [SerializeField] private LineOfSightAI _lineOfSightAI;

  [SerializeField] private Transform hitboxPoint;

  [SerializeField] private BrollaChanData data;

  private Collider[] _hitPlayerHitbox = new Collider[10];
  private Vector3 _startingPosition;
    public LineOfSightAI LineOfSightAI => _lineOfSightAI;


    public event Action<float> onMove;
    public event Action OnAttack;

    private void Awake()
    {
        BakeReferences();
        _startingPosition = transform.position;
    }

    public void SubscribeToEvents(EnemyController controller)
    {
        controller.OnMove += Move;
        controller.OnChase += OnChaseHandler;
        controller.OnIdle += OnIdleHandler;
        controller.OnAttack += Attack;
        controller.OnPatrol += OnPatrolHandler;
        controller.OnReset += OnResetHandler;

    }

    private void OnResetHandler()
    {
        transform.position = _startingPosition;
        Move(Vector3.zero);
    }

    private void BakeReferences()
    {
        _enemyView = GetComponent<EnemyView>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _enemyView.SubscribeToEvents(this);
    }

    private void Move(Vector3 dir)
    {
        dir = dir.normalized;
        _rb.velocity = dir * _currSpeed;
        onMove?.Invoke(Vel);
        if (dir == Vector3.zero) return;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * data.rotationSpeed);
    }   
    private void OnChaseHandler()
    {
        _currSpeed = data.chaseSpeed;
    }

    private void OnPatrolHandler()
    {
        _currSpeed = data.patrolSpeed;
    }

    private void OnIdleHandler()
    {
        _rb.velocity = Vector3.zero;
        Move(Vector3.zero);
    }

    private void Attack()
    {
        OnAttack?.Invoke();
    }

    private void HitboxAgainstPlayer()
    {
        //To be used by animation component
        var playerHitAmount = Physics.OverlapSphereNonAlloc(hitboxPoint.position, data.hitboxSize, _hitPlayerHitbox, data.playerLayer);

        if (playerHitAmount == 0) return;

        for (int i = 0; i < playerHitAmount; i++)
        {
            var curr = _hitPlayerHitbox[i];
            var isPlayer = curr.GetComponent<PlayerModel>();

            if (isPlayer == null) continue;
            
            isPlayer.LifeController.GetDamage(10,true);
            break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitboxPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitboxPoint.position,data.hitboxSize);
    }
}
