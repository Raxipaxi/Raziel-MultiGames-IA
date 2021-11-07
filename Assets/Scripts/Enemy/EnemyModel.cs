using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    private EnemyView _enemyView;
    private Rigidbody _rb;
    private float _currSpeed;

    [SerializeField] private PlayerData _enemyData;

    private void Awake()
    {
        BakeReferences();
 
    }

    public void SubscribeToEvents(EnemyController controller)
    {
        controller.OnWalk += Move;

    }

    void BakeReferences()
    {
        _enemyView = GetComponent<EnemyView>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Move(Vector3 dir)
    {
        _rb.velocity = dir * _currSpeed;
        transform.forward = dir.normalized;
        
    }

    private void Die()
    {
        
    }

    private void Attack()
    {
        
    }
}
