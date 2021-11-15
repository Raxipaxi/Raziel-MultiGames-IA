using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightAI : MonoBehaviour
{   
    [SerializeField] private LineOfSightDataScriptableObject _sightData;
    [SerializeField] private Transform lineOfSightOrigin;
    private LineOfSightDataScriptableObject _originalData;
    public LineOfSightDataScriptableObject OriginalData => _originalData;

    private void Awake()
    {
        _originalData = _sightData;
    }

    public bool SingleTargetInSight(Transform target)
    {       

        Vector3 diff = target.position - lineOfSightOrigin.position;
        float distance = diff.magnitude;

       
        if (distance > _sightData.range) return false;

        Vector3 front = lineOfSightOrigin.forward;

        if (!IsInVisionAngle(diff, front)) return false;

        if (!IsInView(diff.normalized, distance, _sightData.obstaclesMask)) return false;

        return true;      
    }

    private bool IsInVisionAngle(Vector3 origin, Vector3 target)
    {       
        float angleToTarget = Vector3.Angle(origin, target);
        return angleToTarget < (_sightData.angle / 2);
    }

    public List <Transform> LineOfSightMultiTarget()
    {
        Collider[] colls = Physics.OverlapSphere(lineOfSightOrigin.position, _sightData.range, _sightData.targetMask);
        List<Transform> listToReturn = new List<Transform>();
        if (colls.Length == 0) return listToReturn;

        Vector3 front = lineOfSightOrigin.forward;

        for (int i = 0; i < colls.Length; i++)
        {
            Collider curr = colls[i];            
            Vector3 diff = curr.transform.position - lineOfSightOrigin.position;
            if (!IsInVisionAngle(diff, front)) continue;
            float distance = diff.magnitude;
            if (!IsInView(diff.normalized, distance, _sightData.obstaclesMask)) continue;
            listToReturn.Add(curr.transform);          
        }

        return listToReturn;
    }

    private bool IsInView(Vector3 normalizedDirection, float distance, LayerMask obstacleMask)
    {
        return !Physics.Raycast(lineOfSightOrigin.position, normalizedDirection, distance, obstacleMask);      
    }
    
    public void SwapLineOfSightData(LineOfSightDataScriptableObject newSightData)
    {
        _sightData = newSightData;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lineOfSightOrigin.position, _sightData.range);
    }
#endif
}