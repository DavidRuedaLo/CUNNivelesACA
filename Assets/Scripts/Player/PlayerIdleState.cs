using UnityEngine;

public class PlayerIdleState : PlayerBaseClass
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        
    }

    public override void OnEnter()
    {
        player.inputReader.moveEvent += OnMoveInput;
        player.inputReader.interactEvent += OnInteractInput;
        player.inputReader.jumpEvent += OnJumpInput;

        player.ResetJumps();
    }

    public override void OnUpdate()
    {   

    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        player.inputReader.interactEvent -= OnInteractInput;
        player.inputReader.jumpEvent -= OnJumpInput;
    }

    private void OnMoveInput(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }

    private void OnInteractInput()
    {   
        //check first for box interactions
        if (player.activePushableBox != null)
        {
            player.pushState.SetInteractableBox(player.activePushableBox);
            player.stateMachine.ChangeState(player.pushState);
        }
        //check for lever interactions
        else if (player.activeLever != null)
        {
            player.activeLever.ToggleLever();
        }
    }

    private void OnJumpInput()
    {
        player.stateMachine.ChangeState(player.jumpStartState);
    }

}
