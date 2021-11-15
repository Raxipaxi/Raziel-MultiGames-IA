using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public float radius;
    public float timeToGetBoids;
    public int maxBoids;
    public LayerMask mask;
    IFlockingBehavior[] _behaviours;
    List<Transform> _boids = new List<Transform>();
    Collider[] _colls;
    IMove _move;
    void Awake()
    {
        _colls = new Collider[maxBoids];
        _move = GetComponent<IMove>();
        _behaviours = GetComponents<IFlockingBehavior>();
    }
    void Start()
    {
        StartCoroutine(WaitToGetBoids());
    }
    void Update()
    {
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _behaviours.Length; i++)
        {
            dir += _behaviours[i].GetDir(transform, _boids);
        }
        dir = dir.normalized;
        _move.Move(dir);
    }
    IEnumerator WaitToGetBoids()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToGetBoids);
            GetBoidsNonAlloc();
        }
    }
    void GetBoids()
    {
        _boids.Clear();
        Collider[] colls = Physics.OverlapSphere(transform.position, radius, mask);
        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].gameObject != gameObject)
                _boids.Add(colls[i].transform);
        }
    }
    void GetBoidsNonAlloc()
    {
        _boids.Clear();
        int count = Physics.OverlapSphereNonAlloc(transform.position, radius, _colls, mask);
        for (int i = 0; i < count; i++)
        {
            if (_colls[i].gameObject != gameObject)
                _boids.Add(_colls[i].transform);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}