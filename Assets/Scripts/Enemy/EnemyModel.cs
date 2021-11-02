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


    void BakeReferences()
    {
        _enemyView = GetComponent<EnemyView>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Move(Vector3 dir)
    {
        
    }

    private void Die()
    {
        
    }

    private void Attack()
    {
        
    }
}
