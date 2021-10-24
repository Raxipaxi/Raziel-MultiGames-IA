using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewLineOfSightData", menuName = "ScriptableObjects/AI/LineOfSight", order = 1)]
public class LineOfSightDataScriptableObject : ScriptableObject
{
    public float range = 10;
    public float angle = 90;
    public LayerMask obstaclesMask;
    public LayerMask targetMask;

}
