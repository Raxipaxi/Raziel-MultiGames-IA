using UnityEngine;


[CreateAssetMenu(fileName = "New Chocobo", menuName = "ScriptableObjects / Chocobos", order = 0)]
public class ChocoboData : ScriptableObject
{
    public float secondsToFollow;
    public float chocoboFollowSpeed;
    public float flockingRadius;
    
}
