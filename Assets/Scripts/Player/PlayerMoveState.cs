using UnityEngine;

public class PlayerMoveState : PlayerBaseClass
{
    public PlayerMoveState(PlayerController player) : base(player)
    {
        
    }

    private readonly int speedHash = Animator.StringToHash("Speed");
    private bool isWalkKeyPressed;

    public override void OnEnter()
    {
        player.inputReader.moveEvent += OnMoveInput;
        player.inputReader.interactEvent += OnInteractInput;
        player.inputReader.jumpEvent += OnJumpInput;
        player.inputReader.aimEvent += OnAimInput;

        player.inputReader.walkEvent += OnWalkInput;
        player.inputReader.walkCanceledEvent += OnWalkCanceled;

        player.ResetJumps();
        isWalkKeyPressed = false;

        if (player.anim != null)
        {
            player.anim.CrossFadeInFixedTime("Locomotion", 0.1f);
        }
    }

    public override void OnUpdate()
    {
        if (player.anim != null)
        {
            player.anim.SetFloat(speedHash, GetTargetMagnitude());
        }

        if(!player.IsGrounded())
        {
            player.stateMachine.ChangeState(player.jumpMidState);
        }
    }

    public override void OnFixedUpdate()
    {
        //movement scaled by magnituied to slow down when walking
        float scaledSpeed = player.moveSpeed * GetTargetMagnitude();
        player.HandleRotatedMovement(player.CurrentMoveInput, scaledSpeed);
    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        player.inputReader.interactEvent -= OnInteractInput;
        player.inputReader.jumpEvent -= OnJumpInput;
        player.inputReader.aimEvent -= OnAimInput;
        
        player.inputReader.walkEvent += OnWalkInput;
        player.inputReader.walkCanceledEvent += OnWalkCanceled;

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

    private void OnWalkInput()
    {
        isWalkKeyPressed = true;
    }

    private void OnWalkCanceled()
    {
        isWalkKeyPressed = false;
    }

    private void OnJumpInput()
    {
        player.stateMachine.ChangeState(player.jumpStartState);
    }

    private void OnAimInput()
    {
        player.stateMachine.ChangeState(player.aimState);
    }

    //helper method to get magnitude
    private float GetTargetMagnitude()
    {
        float rawMagnitude = Mathf.Clamp01(player.CurrentMoveInput.magnitude);

        //cap if holding walk key
        if (isWalkKeyPressed)
        {
            return Mathf.Min(rawMagnitude, player.walkMultiplier);
        }

        return rawMagnitude;
    }

}
