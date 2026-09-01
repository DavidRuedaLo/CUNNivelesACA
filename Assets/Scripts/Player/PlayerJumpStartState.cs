using Unity.Mathematics;
using UnityEngine;

public class PlayerJumpStartState : PlayerBaseClass
{
    public PlayerJumpStartState(PlayerController player) : base(player)
    {
    }
    

    public override void OnEnter()
    {
        player.inputReader.jumpCanceledEvent += OnJumpCanceled;

        //add the jump force on state enter
        player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.jumpForce, player.rb.linearVelocity.z);

        player.ConsumeJump();
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        player.HandleRotatedMovement(player.CurrentMoveInput, player.moveSpeed);

        //go to on air state once velocity goes negative (falling down)
        if (player.rb.linearVelocity.y <= 0f)
        {
            player.stateMachine.ChangeState(player.jumpMidState);
        }
    }

    public override void OnExit()
    {
        player.inputReader.jumpCanceledEvent -= OnJumpCanceled;
    }

    //redice jump once jump button is released
    private void OnJumpCanceled()
    {
        if (player.rb.linearVelocity.y > 0f)
        {
            player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, (player.rb.linearVelocity.y * player.jumpCutMultiplier), player.rb.linearVelocity.z);
        }
    }
}
