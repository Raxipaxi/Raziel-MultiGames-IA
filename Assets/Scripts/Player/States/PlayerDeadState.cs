using UnityEngine;


public class PlayerDeadState<T> : State<T>
{
    //Works as an interrumpt to other FSM states

    public override void Awake()
    {
        Debug.Log("Exploto el cerebro");
    }
}