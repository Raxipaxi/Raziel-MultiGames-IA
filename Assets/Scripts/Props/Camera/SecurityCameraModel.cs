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

    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>(); //TODO EL GIZMO VA PA CUALQUIER LAAAO NEGRO
        
    }
}
