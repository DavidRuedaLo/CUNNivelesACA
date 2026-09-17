using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    public IState currentState;
    public PlayerController playerController;

    private bool IsDebugLoggingEnabled()
    {
        return playerController != null && playerController.debugMode;
    }

    public void Initialize(IState startingState)
    {
        currentState = startingState;
        currentState.OnEnter();

        if (IsDebugLoggingEnabled())
        {
            Debug.Log("Entering: " + currentState);
        }
    }

    void Update()
    {
        if(currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    void FixedUpdate()
    {
        if(currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit();
        }

        currentState = newState;

        currentState.OnEnter();

        if (IsDebugLoggingEnabled())
        {
            Debug.Log("Entering: " + currentState);
        }
    }
}
