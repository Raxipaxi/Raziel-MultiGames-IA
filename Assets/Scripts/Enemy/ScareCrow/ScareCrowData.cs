using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "ScriptableObjects/Enemies/ScareCrow", fileName = "NewScareCrowData", order = 1)]
public class ScareCrowData : ScriptableObject
{
    [Header("Model")]
    public float rotationSpeed = 10;
    public float chaseSpeed = 3.5f;
   [Header("Controller")]
    public float timeToCheckOnPlayer = 2;
    public float idleLenght = 4.9f;
    public float timeToOutOfStun;

}
