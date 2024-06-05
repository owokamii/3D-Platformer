using UnityEngine;

// This state machine approach ensures the initial state needs to be set first
// and calls the Enter method right after.
public class StateMachine
{
    private StateBase currentState;

    private bool initialized = false;

    public void SetInitialState(StateBase s)
    {
        currentState = s;
        currentState.Enter();
        initialized = true;
    }

    public void Tick(float deltaTime)
    {
        if (!initialized)
        {
            Debug.LogWarning("Call SetInitialState first!");
            return;
        }

        currentState.Update(deltaTime);

        if (currentState.TryGetNextTransition(out StateBase next))
        {
            currentState.Exit();
            currentState = next;
            currentState.Enter();
        }
    }
}