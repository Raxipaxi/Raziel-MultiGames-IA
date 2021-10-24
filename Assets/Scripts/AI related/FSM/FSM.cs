using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM <T>
{
    IState<T> _current;

    public IState<T> Current => _current;

    public FSM (IState <T> initialState)
    {
        SetInitialState(initialState);
    }

    private void SetInitialState(IState <T> initState)
    {
        _current = initState;
        _current.Awake();
        _current.parentFSM = this;
    }
    public void UpdateState()
    {
        if (_current != null)
            _current.Execute();
    }

    public void Transition (T input)
    {
        IState<T> newState = _current.GetTransition(input);
        if (newState != null)
        {
            _current.Sleep();
            _current = newState;
            _current.parentFSM = this;
            _current.Awake();           
        }
    }
}
