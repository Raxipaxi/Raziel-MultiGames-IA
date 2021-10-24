using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionNode : INode
{
    Action _actionToExecute;
    public ActionNode(Action action)
    {
        _actionToExecute = action;
    }
    public void Execute()
    {
        _actionToExecute();
    }
}
