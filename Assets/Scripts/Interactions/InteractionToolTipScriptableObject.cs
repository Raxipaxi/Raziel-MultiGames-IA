using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewInteractionToolTipData" , menuName = "ScriptableObjects/UI/InteractionToolTip", order = 1)]
public class InteractionToolTipScriptableObject : ScriptableObject
{
    public float rotationSpeed;
    public string charToDisplay;
    public float maxInteractionDistance;
    public float alphaMultiplier;
    public float dissapearLerp;

    [Header ("Colors")]

    public Color FaceColor;
    public Color GlowColor;
    public Color UnderlayColor;
    public Color OutlineColor;


}
