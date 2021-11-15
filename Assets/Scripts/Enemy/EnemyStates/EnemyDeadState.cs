using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState<T> : State<T>
{

    private Action _onDead;
    public EnemyDeadState(Action onDead)
    {
        _onDead = onDead;
    }

    public override void Awake()
    {
        _onDead?.Invoke();
    }
}
