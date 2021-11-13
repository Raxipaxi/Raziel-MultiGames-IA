using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour,IAlertable
{
    // Start is called before the first frame update

    private void Awake()
    {
        Owner = gameObject;
    }

   


    public void OnAlertedHandler(Vector3 targetPos)
    {
       Debug.Log($"Ojito alerta10");
    }

    public GameObject Owner { get; private set; }
}
