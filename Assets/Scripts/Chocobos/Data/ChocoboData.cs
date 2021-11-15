using UnityEngine;


[CreateAssetMenu(fileName = "New Chocobo", menuName = "ScriptableObjects / Chocobos", order = 0)]
public class ChocoboData : ScriptableObject
{
    
    [Header("Model")]
    public float secondsToFollow;
    public float chocoboFollowSpeed;
    public float flockingRadius;

    [Header("Controller")]
    public float checkForPlayerRadius = 4;
    public LayerMask playerMask;
}
