using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour, IFlocking
{
    public float radius = 1;
    public float multiplier;
    public Vector3 GetDir(Transform entity, List<Transform> boids)
    {
        Vector3 dir = Vector3.zero;
        if (boids.Count == 0) return dir;
        int count = 0;
        for (int i = 0; i < boids.Count; i++)
        {
            Vector3 currDir = entity.position - boids[i].position;
            if (currDir.magnitude >= radius) continue;
            count++;
            dir += currDir.normalized * (radius - currDir.magnitude);
        }
        if (count != 0)
            dir /= count;
        return dir.normalized * multiplier;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
