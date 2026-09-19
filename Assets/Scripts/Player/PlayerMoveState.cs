using UnityEngine;

public class PlayerMoveState : PlayerBaseClass
{
    public PlayerMoveState(PlayerController player) : base(player)
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

        if (player.anim != null)
        {
            player.anim.CrossFadeInFixedTime("Locomotion", 0.1f);
        }
    }

    public override void OnUpdate()
    {
        if (player.anim != null)
        {
            //clamped values to prevent analog input from occasionally exceeding 1.0
            float currentSpeed = Mathf.Clamp01(player.CurrentMoveInput.magnitude);
            player.anim.SetFloat(speedHash, currentSpeed);
        }
    }

    public override void OnFixedUpdate()
    {
        //movement scaled by magnituied to slow down when walking
        float scaledSpeed = player.moveSpeed * Mathf.Clamp01(player.CurrentMoveInput.magnitude);
        player.HandleRotatedMovement(player.CurrentMoveInput, player.moveSpeed);
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

    private void OnAimInput()
    {
        player.stateMachine.ChangeState(player.aimState);
    }

}
