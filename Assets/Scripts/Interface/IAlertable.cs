using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlertable
{
   void OnAlertedHandler(Node targetPosNode);
   GameObject Owner { get; }

}
