using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerJumpMidState : PlayerBaseClass
{
    public PlayerJumpMidState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        player.inputReader.jumpEvent += OnAirJumpInput;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        player.HandleRotatedMovement(player.CurrentMoveInput, player.moveSpeed);

        //adds multiplier to gravity for faster time down
        //since unity's physics is already adding 1.0x internally, the fall mult needs to be minus 1 for actual bonus
        //e.g. 2.5 becomes 1.5 (150% gravity)
        player.rb.linearVelocity += Vector3.up * (Physics.gravity.y * (player.fallMultiplier -1) * Time.fixedDeltaTime);

        //caps the falling speed to prevent infinite acceleration
        if (player.rb.linearVelocity.y < player.maxFallSpeed)
        {
            player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.maxFallSpeed, player.rb.linearVelocity.z);
        }

        if(player.IsGrounded())
        {
            player.ResetJumps();

            player.stateMachine.ChangeState(player.jumpEndState);
        }
    }

    public override void OnExit()
    {
        player.inputReader.jumpEvent -= OnAirJumpInput;
    }

    private void OnAirJumpInput()
    {
        if(player.CanJump())
        {
            player.ConsumeJump();

            //send back to jumpStartState for subsequent jumps
            player.stateMachine.ChangeState(player.jumpStartState);
        }
    }

}
