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
    public float targetVel;
    public float timePrediction;
    public DesiredBehaviour _defaultBehaviour;
    
    
    public enum DesiredBehaviour
    {
        Seek,
        Flee,
        Pursuit,
        Evasion,        
    }
}
