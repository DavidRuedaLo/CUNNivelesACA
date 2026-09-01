using UnityEngine;

public class PlayerJumpEndState : PlayerBaseClass
{
    public PlayerJumpEndState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        //change to idlestate instantly for now since there's no animation state yet
        if(player.IsGrounded())
        {
            if(player.CurrentMoveInput.sqrMagnitude > 0f)
            {
                player.stateMachine.ChangeState(player.moveState);
            }
            else
            {
                player.stateMachine.ChangeState(player.idleState);
            }
        }

        player.ResetJumps();
    }
}
