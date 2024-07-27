using UnityEngine;

public class StateMachine
{
    private StateBase currentState;
    private bool initialized = false;

    public string CurrentStateName => currentState?.Name;

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
            if (next != currentState)
            {
                currentState.Exit();
                currentState = next;
                currentState.Enter();
            }
        }
    }
}