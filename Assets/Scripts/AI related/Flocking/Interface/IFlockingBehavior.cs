using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlockingBehavior
{
    Vector3 GetDir(Transform entity, List<Transform> boids);
}
