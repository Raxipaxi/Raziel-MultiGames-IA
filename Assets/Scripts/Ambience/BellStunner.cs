using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class BellStunner : MonoBehaviour,IInteractable
{

    [SerializeField] private List<GameObject> stunnable;

    private List<IStunable> _stunable;


    private void Awake()
    {
        _stunable = new List<IStunable>();
        foreach (var item in stunnable)
        {
            var stunableObject = item.GetComponent<IStunable>();
            _stunable.Add(stunableObject);
        }
    }

    public void GetStunnables()
    {
        stunnable = new List<GameObject>();
        var obj = FindObjectsOfType<GameObject>();

        foreach (var item in obj)
        {
            var stunable = item.GetComponent<IStunable>();
            if (stunable == null) continue;
            stunnable.Add(item);
        }

        Debug.Log($"Stunables is {_stunable.Count}");
    }

    private void StunEnemies()
    {
        Debug.Log("Stunned enemies");
        for (int i = 0; i < _stunable.Count; i++)
        {
           _stunable[i].GetStun();
        }
    }
    public bool OnInteract()
    {
        StunEnemies();
        return true;
    }

    public bool OnTriggerContinueInteract()
    {
        return true;
    }

    public Canvas InteractCanvas { get; }
}

#if UNITY_EDITOR

[CustomEditor(typeof(BellStunner))]
internal class BellStunnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get Stunnables"))
        {
            var curr = target as BellStunner;
            curr.GetStunnables();
        }
    }
}
#endif