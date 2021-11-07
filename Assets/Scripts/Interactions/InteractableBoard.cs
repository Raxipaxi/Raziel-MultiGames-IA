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

    private Coroutine _deactivateCanvasCoroutine;

    [SerializeField] private Canvas missionCanvas;
    
    [SerializeField] private UnityEvent OnInteractionEnd;
    private WaitForSecondsRealtime _timeBetweenMessage;

    private void Awake()
    {
        _timeBetweenMessage = new WaitForSecondsRealtime(data.finalMessageTimeToDissapear);
    }

    public bool OnInteract()
    {
        if (_deactivateCanvasCoroutine != null)
        {
            return true;
        }
        
        missionText.text = data.message[_textIndex]; 
        missionCanvas.gameObject.SetActive(true);
        _textIndex++;

        if (_textIndex >= data.message.Length)
        {
          _deactivateCanvasCoroutine = StartCoroutine(DeactivateCanvas());
            _textIndex = 0;
            return true;
        }

        return false;
    }

    public bool OnTriggerContinueInteract()
    {
        if (_textIndex >= data.message.Length)
        {
            _textIndex = 0;
            _deactivateCanvasCoroutine = StartCoroutine(DeactivateCanvas());
            return true;
        }
        missionText.text = data.message[_textIndex];
        _textIndex++;
        return false;
    }

    private IEnumerator DeactivateCanvas()
    {
        yield return _timeBetweenMessage;
        
        InteractCanvas.gameObject.SetActive(false);

        _deactivateCanvasCoroutine = null;
    }

    public void BakeReferences()
    {
        var tempMissionCanvas = GameObject.FindGameObjectWithTag("MissionCanvas");

        if (tempMissionCanvas == null) return;

        missionCanvas = tempMissionCanvas.GetComponent<Canvas>();
        missionText = missionCanvas.GetComponentInChildren<TextMeshProUGUI>(true);
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
        }
    }
}

#endif
