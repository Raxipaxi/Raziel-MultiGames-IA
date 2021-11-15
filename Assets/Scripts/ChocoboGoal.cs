using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ChocoboGoal : MonoBehaviour
{
    [SerializeField] private int totalChocobos;
    private int _tempTotalChocobos;

    [SerializeField] private UnityEvent onFinishLevel;
    private void OnTriggerEnter(Collider other)
    {
        var chocobo = other.GetComponent<ChocoboController>();


        if (chocobo == null) return;
        
        Debug.Log("Chocobo triggered");
        chocobo.ReachedGoal();
        _tempTotalChocobos++;

        if (_tempTotalChocobos >= totalChocobos)
        {
            onFinishLevel?.Invoke();
        }
    }

    public void GetChocobosOnScene()
    {
        var obj = FindObjectsOfType<ChocoboController>();
        totalChocobos = obj.Length;
    }
    
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(ChocoboGoal))]
internal class ChocoboGoalEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get chocobos"))
        {
            var curr = target as ChocoboGoal;
            
            curr.GetChocobosOnScene();
            
            EditorUtility.SetDirty(curr);
        }
    }
}
#endif