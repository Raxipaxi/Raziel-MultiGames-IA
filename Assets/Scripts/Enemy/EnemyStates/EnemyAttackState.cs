using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState<T> : State<T>
{
    private INode _root;
    private Action _onAttack;

    public EnemyAttackState(INode root, Action onAttack)
    {
        _root = root;
        _onAttack = onAttack;
    }


    public override void Execute()
    {
        _onAttack?.Invoke();
        _root.Execute();
    }
}
