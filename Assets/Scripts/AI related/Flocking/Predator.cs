using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : MonoBehaviour, IFlocking
{
    public float radius;
    public float multiplier;
    public LayerMask predatorMask;
    public Vector3 GetDir(Transform entity, List<Transform> boids)
    {
        var colls = Physics.OverlapSphere(entity.position, radius, predatorMask);
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < colls.Length; i++)
        {
            Vector3 currDir = entity.position - colls[i].transform.position;
            var distance = currDir.magnitude;
            if (distance >= radius) distance = radius - 0.1f;
            dir += currDir.normalized * (radius - distance);
        }
        return (dir / colls.Length).normalized * multiplier;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
