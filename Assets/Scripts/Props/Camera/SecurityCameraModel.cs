using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraModel : MonoBehaviour
{
    private LineOfSightAI _lineOfSightAI;

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
    }
    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>(); //TODO EL GIZMO VA PA CUALQUIER LAAAO NEGRO
        
    }
}
