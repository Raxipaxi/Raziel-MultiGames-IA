using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState<T> : State<T>
{
    private INode _root;
    public EnemyIdleState(INode root)
    {
        _root = root;
    }

   
    void Execute()
    {
        
    }
}
