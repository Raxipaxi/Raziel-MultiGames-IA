using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraController : MonoBehaviour
{

    private SecurityCameraModel _cameraModel;
    private PlayerModel _playerModel;

    public event Action<Vector3> OnAlert;


    public void OnAlertCommand(Vector3 dir)
    {
        OnAlert?.Invoke(dir);
    }

    private void Awake()
    {
        BakeReferences();
    }

    public void BakeReferences()
    {
        _cameraModel = GetComponent<SecurityCameraModel>();
        _playerModel = GetComponent<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cameraModel.LineOfSightAI.SingleTargetInSight(_playerModel.transform))
        {
            OnAlertCommand(_playerModel.transform.position);
        }   
    }
}
