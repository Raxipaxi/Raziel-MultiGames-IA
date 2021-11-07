using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (fileName = "NewInteractableBoardMissionData", menuName = "ScriptableObjects/Missions", order = 1)]
public class InteractableBoardData : ScriptableObject
{
    public string[] message;
    public float finalMessageTimeToDissapear;
[TextArea (20,20)]    public string hint;
}
