﻿using System;
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

    [Header ("Jump")]
    public float jumpHeight;
    public float isGroundedRadius;
    public LayerMask groundMask;

    [Header ("Others")]

    public float maxLifes;


}
