using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemies/MrI", fileName = "NewMrIData", order = 1)]
public class MrIData : ScriptableObject
{

    public float patrolSpeed;
    public float chaseSpeed;
    public float goToSpotSpeed;

    public float rotationalInput = 0.12f;
    public float rotationsPerSecond = 90;
}
