using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraModel : MonoBehaviour
{
    private LineOfSightAI _lineOfSightAI;

    [SerializeField] private CameraData _data;
    public LineOfSightAI LineOfSightAI => _lineOfSightAI;
    private void Awake()
    {
        BakeReferences();
   }

    public void SubscribeToEvents(SecurityCameraController controller)
    {
        controller.OnAlert += OnAlertHandler;
    }

    private void OnAlertHandler(Vector3 targetPosition)
    {
        Debug.Log("Alerted");
        
        //Get closest enemies, top 2

        var enemiesToAlert = new List<IAlertable>();
        enemiesToAlert = EnemyManager.Instance.GetCloseEnemies(transform.position, _data.alertRadius, _data.maxEnemiesToAlert);

        if (enemiesToAlert.Count == 0) return;

        for (int i = 0; i < enemiesToAlert.Count; i++)
        {
            Debug.Log($"Alerted an enemy");
            enemiesToAlert [i].OnAlertedHandler(targetPosition);
        }
        
        //SONIDO RANCIO
        
        
    }
    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>();
    }
}
