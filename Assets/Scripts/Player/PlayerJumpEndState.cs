using UnityEngine;

public class PlayerJumpEndState : PlayerBaseClass
{
    private float landingTimer;
    public PlayerJumpEndState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        landingTimer = 0f;

        if(player.anim != null)
        {
            player.anim.CrossFadeInFixedTime("Jump_Land", 0.05f);
        }

        player.ResetJumps();
    }

    public override void OnUpdate()
    {
        landingTimer += Time.deltaTime;
        if (landingTimer >= player.landingDuration)
        {
            if(player.CurrentMoveInput != Vector2.zero)
            {
                player.stateMachine.ChangeState(player.moveState);
            }
            else
            {
                player.stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void OnFixedUpdate()
    {
        if(player.rb != null)
        {
            Vector3 currentVelocity = player.rb.linearVelocity;
            player.rb.linearVelocity = new Vector3(0f, currentVelocity.y, 0f);
        }
    }
}
