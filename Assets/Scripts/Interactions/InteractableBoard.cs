using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBoard : MonoBehaviour, IInteractable
{

    [SerializeField] private InteractableBoardData data;
    [SerializeField] private TextMeshProUGUI missionText;
    private int _textIndex;

   
    public Canvas InteractCanvas => missionCanvas;



    [SerializeField] private MainCanvas mainCanvas;
    [SerializeField] private Canvas missionCanvas;
    
    [SerializeField] private UnityEvent OnInteractionEnd;
   

  

    public bool OnInteract()
    {
       
        
        mainCanvas.ChangeCurrentMission(data.message[_textIndex]);
        
        _textIndex++;
        var endInteraction = _textIndex >= data.message.Length;
        if (!endInteraction) return false;
        _textIndex = 0;
        OnInteractionEnd?.Invoke();
        mainCanvas.EndCurrentMissionCanvas();
        return true;

    }

   

    public bool OnTriggerContinueInteract()
    {
        
        if (_textIndex >= data.message.Length)
        {
            _textIndex = 0;
            OnInteractionEnd?.Invoke();
            mainCanvas.EndCurrentMissionCanvas();
            return true;
        }

        mainCanvas.ChangeCurrentMission(data.message [_textIndex]);
        _textIndex++;
        return false;
    }


    public void BakeReferences()
    {
        mainCanvas = FindObjectOfType<MainCanvas>();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(InteractableBoard))]
internal class InteractableBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var curr = target as InteractableBoard;
        
       if(GUILayout.Button("Bake References"))
        {
            curr.BakeReferences();
            EditorUtility.SetDirty(curr);
        }
    }
}

#endif
