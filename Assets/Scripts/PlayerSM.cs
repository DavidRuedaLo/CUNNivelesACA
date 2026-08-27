using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    public IState currentState;

    public void Initialize(IState startingState)
    {
        currentState = startingState;
        currentState.OnEnter();
        Debug.Log("Entering: " + currentState);
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

        Debug.Log("Entering: " + currentState);
    }
}
