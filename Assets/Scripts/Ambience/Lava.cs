
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Reset");
            other.GetComponent<PlayerModel>().LifeController.GetDamage(10,true);
        }
    }
}
