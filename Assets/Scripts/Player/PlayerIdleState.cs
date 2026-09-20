using UnityEngine;

public class PlayerIdleState : PlayerBaseClass
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        
    }

    private readonly int speedHash = Animator.StringToHash("Speed");

    public override void OnEnter()
    {
        player.inputReader.moveEvent += OnMoveInput;
        player.inputReader.interactEvent += OnInteractInput;
        player.inputReader.jumpEvent += OnJumpInput;
        player.inputReader.aimEvent += OnAimInput;
        

        player.ResetJumps();

        if(player.anim != null)
        {
            player.anim.SetFloat(speedHash, 0f);
            player.anim.CrossFadeInFixedTime("Locomotion", 0.1f);
        }
    }

    public override void OnUpdate()
    {   
        if(!player.IsGrounded())
        {
            player.stateMachine.ChangeState(player.jumpMidState);
        }
    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        player.inputReader.interactEvent -= OnInteractInput;
        player.inputReader.jumpEvent -= OnJumpInput;
        player.inputReader.aimEvent -= OnAimInput;
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

    private void OnAimInput()
    {
        player.stateMachine.ChangeState(player.aimState);
    }

}
