using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour, IFlocking
{
    public Transform leader;
    public float multiplier;
    public Vector3 GetDir(Transform entity, List<Transform> boids)
    {
        return (leader.position - entity.position).normalized * multiplier;
    }
}
