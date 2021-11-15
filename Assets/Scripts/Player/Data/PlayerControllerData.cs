using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewPlayerControllerData", menuName = "ScriptableObjects/Player/PlayerControllerData", order = 1)]
public class PlayerControllerData : ScriptableObject
{

  [Header("Inputs")]
  public KeyCode jump;
  public KeyCode run;
  public KeyCode interact;

  [Header("Data")] 
  public float waitToRevive;



}
