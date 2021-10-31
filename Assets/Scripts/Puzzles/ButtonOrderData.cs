using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewButtonOrderPuzzle", menuName = "ScriptableObjects/Puzzles/ButtonPuzzle", order = 1)]
public class ButtonOrderData : ScriptableObject
{
    public List<int> buttonSolveOrder;

    public Material pressedMaterial, defaultMaterial, correctMaterial;

    public AudioClip clickedSound, incorrectSound, correctSound;
}
