using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemies/Brolla", fileName = "NewBrollaData", order = 1)]
public class BrollaChanData : ScriptableObject
{
    [Header("Model")]
    
    public float rotationSpeed = 10;
    public float patrolSpeed = 2;
    public float chaseSpeed = 3.5f;
    public LayerMask playerLayer;
    public float hitboxSize = 0.5f;

    [Header("Controller")] 
    public float distanceToAttack = 2;
    public float timeToAttemptAttack = 2;
    public float idleLenght = 4.9f;
    public float minDistance = 0.5f;
    public float timeToOutOfAttack = 1f;
}
