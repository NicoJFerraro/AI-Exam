using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class StateMachine {

    State _currentState;
    List<State> _states = new List<State>();

    public void Update()
    {
        if (_currentState != null)
            _currentState.Execute();
    }

    public void AddState(State y)
    {
        _states.Add(y);
        if (_currentState == null)
            _currentState = y;
    }

    public void SetState<X>() where X : State
    {
        for (int i = 0; i < _states.Count; i++)
        {
            if (_states[i].GetType() == typeof(X))
            {
                _currentState.Sleep();
                _currentState = _states[i];
                _currentState.Awake();
            }
        }

    }
    public bool IsActSt<X>() where X : State
    {
        return _currentState.GetType() == typeof(X);
    }

    private int SearchSt(Type x)
    {
        int ammountOfSt = _states.Count;
        for (int i = 0; i < ammountOfSt; i++)
        {
            if (_states[i].GetType() == x)
                return i;
        }
        return -1;
    }
	
}
