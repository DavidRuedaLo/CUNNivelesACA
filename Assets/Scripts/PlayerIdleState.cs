using UnityEngine;

public class PlayerIdleState : PlayerBaseClass
{
    public PlayerIdleState(PlayerController player) : base(player)
    {
        
    }

    public override void OnEnter()
    {
        player.inputReader.moveEvent += OnMoveInput;
    }

    public override void OnUpdate()
    {   

    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
    }

    private void OnMoveInput(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            player.stateMachine.ChangeState(player.moveState);
        }
    }

}
