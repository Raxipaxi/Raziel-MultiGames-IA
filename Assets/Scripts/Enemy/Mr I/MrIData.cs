using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemies/MrI", fileName = "NewMrIData", order = 1)]
public class MrIData : ScriptableObject
{

    [Header("Model")]
    public float patrolSpeed;
    public float chaseSpeed;
    public float goToSpotSpeed;

    public float rotationalInput = 0.12f;
    public float rotationsPerSecond = 90;

    [Header("Controller")]
    public float minimumWaypointDistance;

    public LayerMask nodesMask;
    public float radiusToCheckNodes;
    public float timeToCheckOnChase;
}
