using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/CameraData", fileName = "NewCameraData", order = 1)]
public class CameraData : ScriptableObject
{
   public float alertRadius = 10;
   public float playerVelocityThreshold = 3;
   public float timeToResumeAlert = 3;
   public float checkPlayerMovementTime = 1.5f;
   public int maxEnemiesToAlert = 1;
   public float nodesGetRadius = 10;
   public LayerMask nodesMask;

}
