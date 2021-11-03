using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Vector3 GetDir(Transform entity, List<Transform> boids)
    {
        if (boids.Count == 0) return Vector3.zero;
        if (multiplier < 0) multiplier = 0;
        Vector3 center = Vector3.zero;
        for (int i = 0; i < boids.Count; i++)
        {
            center += boids[i].position;
        }
        center /= boids.Count;
        return (center - entity.position).normalized * multiplier;
    }
}
