using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private List<IAlertable> _alertableEnemies;
    [SerializeField] private List<GameObject> alertableObjectsList;
    public static EnemyManager Instance;
    private int _currentQuantity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }

        _alertableEnemies = new List<IAlertable>();

        
        for (int i = 0; i < alertableObjectsList.Count; i++)
        {
            var alertable = alertableObjectsList[i];
            _alertableEnemies.Add(alertable.GetComponent<IAlertable>());
        }
    }

    public List<IAlertable> GetCloseEnemies(Vector3 userPosition, float maxDistance, int maxQuantity)
    {
        var alertables = new List<IAlertable>();
        if (_alertableEnemies.Count == 0) return alertables;
        _currentQuantity = 0;
        
        for (int i = 0; i < _alertableEnemies.Count; i++)
        {
            if (_currentQuantity >= maxQuantity) break;
            
            var curr = _alertableEnemies[i];

            var distance = Vector3.Distance(curr.Owner.transform.position, userPosition);
            
            if (distance > maxDistance) continue;
            
            alertables.Add(curr);
            _currentQuantity++;
        }
        return alertables;
    }

  
    public void GetAllEnemiesOnScene()
    {
        alertableObjectsList = new List<GameObject>();
        var objects = GameObject.FindObjectsOfType<MonoBehaviour>();

        for (int i = 0; i < objects.Length; i++)
        {
            var alertable = objects[i].GetComponent<IAlertable>();
            if (alertable != null) alertableObjectsList.Add(objects[i].gameObject);
        }
        
        EditorUtility.SetDirty(gameObject);
    }

   
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyManager))]
internal class EnemyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var curr = target as EnemyManager;
        
        if (GUILayout.Button("GetAlertableEnemies"))
        {
          
            if (curr != null) curr.GetAllEnemiesOnScene();
            EditorUtility.SetDirty(curr);

        }
    }
}

#endif
