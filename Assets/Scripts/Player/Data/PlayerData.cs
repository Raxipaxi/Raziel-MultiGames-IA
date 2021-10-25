using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewPlayerData", menuName = "ScriptableObjects/Player/Data", order = 1)]
public class PlayerData : ScriptableObject
{

    public float walkSpeed;
    public float runSpeed;
    public float interactionRadius;
    public float jumpHeight;
    public float maxLifes;

  



}
