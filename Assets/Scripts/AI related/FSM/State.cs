using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State <T> : IState <T>
{
    Dictionary<T, IState<T>> _stateTransitions = new Dictionary<T, IState<T>>();
    FSM<T> _parentFSM;

    public FSM<T> parentFSM { get => _parentFSM; set => _parentFSM = value; }

    public virtual void Awake()
    {

    }

    public virtual void Execute()
    {

    }

    public virtual void Sleep()
    {

    }

    public void AddTransition (T input, IState<T> transitionState)
    {
        if (!_stateTransitions.ContainsKey(input))
        {
            _stateTransitions[input] = transitionState;
        }
    }

    public void RemoveTransition(T input)
    {
        if (_stateTransitions.ContainsKey(input))
        {
            _stateTransitions.Remove(input);
        }
    }

    public void RemoveTransition (IState<T> state)
    {
        if (_stateTransitions.ContainsValue(state))
        {
            foreach (var item in _stateTransitions)
            {
                if (item.Value == state)
                {
                    _stateTransitions.Remove(item.Key);
                }
            }
        }
    }

    public IState <T> GetTransition (T input)
    {
        if (_stateTransitions.ContainsKey(input))
        {
            return _stateTransitions[input];
        }
        return null;
    }

 
}
