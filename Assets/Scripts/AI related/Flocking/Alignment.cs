using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment : MonoBehaviour, IFlocking
{
    public float multiplier;
    public Vector3 GetDir(Transform entity, List<Transform> boids)
    {
        Vector3 dir = Vector3.zero;
        if (boids.Count != 0)
        {
            for (int i = 0; i < boids.Count; i++)
            {
                dir += boids[i].forward;
            }
            dir /= boids.Count;
        }
        return dir * multiplier;
    }
}
