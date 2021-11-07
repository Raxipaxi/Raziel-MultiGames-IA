using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewInteractionToolTipData" , menuName = "ScriptableObjects/UI/InteractionToolTip", order = 1)]
public class InteractionToolTipScriptableObject : ScriptableObject
{
    public float rotationSpeed;
    public string charToDisplay;
}
