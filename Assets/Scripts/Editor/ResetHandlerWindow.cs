using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResetHandlerWindow : MonoBehaviour
{
    
   
    [MenuItem("GameObject/ResetHandler")]
    static void CreateResetHandler()
    {
        var obj = new GameObject();
        obj.AddComponent(typeof(LevelResetHandler));
        obj.name = "NewResetHandler";
        var objCompo = obj.GetComponent<LevelResetHandler>();
        objCompo.BakeResetableObjects();
    }
}
