using System;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPair
{
    public Func<float, bool> predicate;
    public StateBase nextState;
}

public abstract class StateBase
{
    public abstract string Name { get; }

    protected List<TransitionPair> Transitions = new List<TransitionPair>();
    private static List<TransitionPair> AnyTransitions = new List<TransitionPair>();

    private float timeEnteredState = -1f;
    private float timeInState = 0f;

    public float TimeEnteredState => timeEnteredState;
    public float TimeInState => timeInState;

    public void Enter()
    {
        Debug.Log($"<color=green>Entering {Name}</color>");
        timeEnteredState = Time.time;
        OnEnter();
    }

    public void Exit()
    {
        Debug.Log($"<color=red>Exiting {Name}</color>");
        OnExit();
    }

    public void Update(float deltaTime)
    {
        timeInState = Time.time - timeEnteredState;
        OnUpdate(deltaTime);
    }

    public void AddTransition(StateBase to, Func<float, bool> predicate)
    {
        Transitions.Add(new TransitionPair { nextState = to, predicate = predicate });
    }

    public static void AddAnyTransition(StateBase to, Func<float, bool> predicate)
    {
        AnyTransitions.Add(new TransitionPair { nextState = to, predicate = predicate });
    }

    public bool TryGetNextTransition(out StateBase state)
    {
        foreach (var t in AnyTransitions)
        {
            if (t.predicate(timeInState))
            {
                state = t.nextState;
                return true;
            }
        }

        foreach (var t in Transitions)
        {
            if (t.predicate(timeInState))
            {
                state = t.nextState;
                return true;
            }
        }

        state = null;
        return false;
    }

    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    protected virtual void OnUpdate(float deltaTime) { }
}