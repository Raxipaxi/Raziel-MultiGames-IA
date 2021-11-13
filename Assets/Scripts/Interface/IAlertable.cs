using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlertable
{
   void OnAlertedHandler(Vector3 targetPos);
   GameObject Owner { get; }

}
