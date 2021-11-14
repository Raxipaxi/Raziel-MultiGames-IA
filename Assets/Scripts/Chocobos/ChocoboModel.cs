using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChocoboModel : MonoBehaviour
{
    [SerializeField] private ChocoboData _chocoboData;
    
    public LineOfSightAI LineOfSightAI => _lineOfSightAI;
    private LineOfSightAI _lineOfSightAI;


    public void Move(Vector3 dir)
    {
        
    }

    public void Idle()
    {
        
    }

    public void SubscribeToEvents(ChocoboController controller)
    {
        
    }
    public void BakeReferences()
    {
        _lineOfSightAI = GetComponent<LineOfSightAI>();
    }
    
}
