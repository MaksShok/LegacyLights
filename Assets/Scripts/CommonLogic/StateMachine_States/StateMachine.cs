using System;
using System.Collections.Generic;
using CommonLogic.StateMachine_States.States;
using UnityEngine;

namespace CommonLogic.StateMachine_States
{
    public class StateMachine
    {
        private static readonly List<StateTransition> EmptyCollection = new ();
    
        private IState _currentState;
        private List<StateTransition> _currentTransitions = new ();
        private List<StateTransition> _anyTransitions = new();
        private Dictionary<Type, List<StateTransition>> _transitionMap = new ();

        public void Update()
        {
            if (_currentState == null || _currentState.CanExit)
            {
                if (TryGetTransition(out var transition))
                {
                    SetState(transition.ToState);
                }
            }
        
            _currentState?.Update(Time.deltaTime);
        }

        public void FixedUpdateState(float fixedDeltaTime)
        {
            _currentState?.FixedUpdate(Time.fixedDeltaTime);
        }

        public void SetState(IState state)
        {
            if (state == _currentState)
                return;
        
            _currentState?.Exit();
            _currentState = state;

            _currentTransitions = _transitionMap.GetValueOrDefault(_currentState.GetType(), EmptyCollection);
            _currentState?.Enter();
        }

        public void AddTransition(IState fromState, IState toState, Func<bool> predicate)
        {
            if (!_transitionMap.TryGetValue(fromState.GetType(), out var transitions))
            {
                transitions = new List<StateTransition>();
                transitions.Add(new StateTransition(toState, predicate));
                _transitionMap.Add(fromState.GetType(), transitions);
            }
        
            transitions.Add(new StateTransition(toState, predicate));
        }

        public void AddAnyTransition(IState toState, Func<bool> predicate)
        {
            _anyTransitions.Add(new StateTransition(toState, predicate));
        }

        private bool TryGetTransition(out StateTransition transition)
        {
            transition = null;
            
            foreach (var st in _anyTransitions)
            {
                if (st.CheckCanGo())
                {
                    transition = st;
                    return true;
                }
            }
                
            foreach (var st in _currentTransitions)
            {
                if (st.CheckCanGo())
                {
                    transition = st;
                    return true;
                }
            }

            return false;
        }
    }

    public class StateTransition
    {
        public IState ToState { get; }
        
        private readonly Func<bool> _condition;

        public StateTransition(IState to, Func<bool> condition)
        {
            ToState = to;
            _condition = condition;
        }

        public bool CheckCanGo()
        {
            return _condition.Invoke();
        }
    }
}