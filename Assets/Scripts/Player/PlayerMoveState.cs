using UnityEngine;

public class PlayerMoveState : PlayerBaseClass
{
    public PlayerMoveState(PlayerController player) : base(player)
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

    public override void OnFixedUpdate()
    {
        player.HandleRotatedMovement(player.CurrentMoveInput, player.moveSpeed);
    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        player.inputReader.interactEvent -= OnInteractInput;
        player.inputReader.jumpEvent -= OnJumpInput;

    }

    private void OnMoveInput(Vector2 direction)
    {
        if(direction == Vector2.zero)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
    private void OnInteractInput()
    {
        if (player.activePushableBox != null)
        {
            player.pushState.SetInteractableBox(player.activePushableBox);
            player.stateMachine.ChangeState(player.pushState);
        }
    }

    private void OnJumpInput()
    {
        player.stateMachine.ChangeState(player.jumpStartState);
    }

}
