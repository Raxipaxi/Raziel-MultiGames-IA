using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelResetHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _resetableObjects;

    private Coroutine _resetCoroutine;

    [SerializeField] private float timeToReset = 4;
    private List<IReseteable> _resetables;

    private void Awake()
    {
        _resetables = new List<IReseteable>();
        for (int i = 0; i < _resetableObjects.Count; i++)
        {
            var obj = _resetableObjects[i];
             var reseteable = obj.GetComponent<IReseteable>();
             if (reseteable == null) continue;
             
             _resetables.Add(reseteable);
        }
    }

    private void Start()
    {
        GameManager.Instance.GetResetLevelHandler(this);
    }
    
    public void HandleResetOfLevel()
    {
        if (_resetCoroutine != null) return;
        _resetCoroutine = StartCoroutine(ResetLevelCoroutine());
    }

    private IEnumerator ResetLevelCoroutine()
    {
        GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.ToBlack);
        yield return new WaitForSeconds(timeToReset);
        GameManager.Instance.MainCanvas.TransitionSet(MainCanvas.TransitionStates.FromBlack);

        foreach (var item in _resetables)
        {
            item.OnLevelReset();
        }

        _resetCoroutine = null;
    }
    public void BakeResetableObjects()
    {
        var objects = FindObjectsOfType<MonoBehaviour>();

        _resetableObjects = new List<GameObject>();

        foreach (var item in objects)
        {
             if (_resetableObjects.Contains(item.gameObject)) continue;
            var curr = item.GetComponent<IReseteable>();
            if (curr == null) continue;
            _resetableObjects.Add(item.gameObject);
        }
    }
    
    
}
#if UNITY_EDITOR
[CustomEditor(typeof(LevelResetHandler))]
internal class LevelResetHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get resetable objects"))
        {
            var curr = target as LevelResetHandler;
            curr.BakeResetableObjects();
        }
    }
}
#endif

