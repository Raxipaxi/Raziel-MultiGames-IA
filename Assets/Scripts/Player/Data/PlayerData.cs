using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/Data", order = 1)]
public class PlayerData : ScriptableObject
{

    [Header ("Interaction")]
    public float tryInteractRadius;
    public LayerMask tryInteractLayers;
    
    [Header ("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float rotationSmoothTime = 0.12f;

    [Header ("Jump")]
    public float jumpHeight;

    public float jumpCooldown;
    public float isGroundedRadius;
    public LayerMask groundMask;

    [Header ("Others")]

    public float maxLifes;


}
