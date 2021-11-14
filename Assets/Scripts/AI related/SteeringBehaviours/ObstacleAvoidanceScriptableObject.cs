using UnityEngine;


[CreateAssetMenu(fileName = "NewObsctacleAvoidanceData", menuName = "ScriptableObjects/AI/ObstacleAvoidance", order = 1)]
public class ObstacleAvoidanceScriptableObject : ScriptableObject
{
    public Transform self;
    public Transform target;
    public float radius;
    public int maxObjs;
    public LayerMask obstaclesMask;
    public float multiplier;
    public float timePrediction;
    public ObstacleAvoidance.DesiredBehaviour _defaultBehaviour;
    
}
