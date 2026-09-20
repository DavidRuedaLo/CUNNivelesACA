using UnityEngine;

public class PlayerJumpStartState : PlayerBaseClass
{
    private float windupTimer;
    private bool hasLaunched;
    private bool jumpCanceledEarly;
    public PlayerJumpStartState(PlayerController player) : base(player)
    {
    }
    public override void OnEnter()
    {
        player.inputReader.jumpCanceledEvent += OnJumpCanceled;

        windupTimer = 0f;
        hasLaunched = false;
        jumpCanceledEarly = false;

        //stop horizontal movement during windup
        player.rb.linearVelocity = new Vector3(0f, player.rb.linearVelocity.y, 0f);

        player.ConsumeJump();

        if (player.anim != null)
        {
            player.anim.CrossFadeInFixedTime("Jump_Start", 0.1f);
        }
    }

    public override void OnUpdate()
    {
        if(!hasLaunched)
        {
            windupTimer += Time.deltaTime;

            if(windupTimer >= player.jumpStartTime)
            {
                LaunchJump();
            }
        }
    }

    public override void OnFixedUpdate()
    {
        //restore movement once player has launched
        if(hasLaunched)
        {
            player.HandleRotatedMovement(player.CurrentMoveInput, player.moveSpeed);
        }        

        //go to on air state once velocity goes negative (falling down) and player has jumped once
        if (hasLaunched && player.rb.linearVelocity.y <= 0f)
        {
            player.stateMachine.ChangeState(player.jumpMidState);
        }
    }

    private void LaunchJump()
    {
        hasLaunched = true;
        //add the jump force
        player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.jumpForce, player.rb.linearVelocity.z);

        if(jumpCanceledEarly)
        {
            player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, (player.rb.linearVelocity.y * player.jumpCutMultiplier), player.rb.linearVelocity.z);
        }
    }

    //redice jump once jump button is released
    private void OnJumpCanceled()
    {
        if(!hasLaunched)
        {
            jumpCanceledEarly = true;
        }
        else if (player.rb.linearVelocity.y > 0f)
        {
            player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, (player.rb.linearVelocity.y * player.jumpCutMultiplier), player.rb.linearVelocity.z);
        }
    }

        public override void OnExit()
    {
        player.inputReader.jumpCanceledEvent -= OnJumpCanceled;
    }
}
