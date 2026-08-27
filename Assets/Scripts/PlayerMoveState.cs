using UnityEngine;

public class PlayerMoveState : PlayerBaseClass
{
    private Vector2 moveDirection;
    public PlayerMoveState(PlayerController player) : base(player)
    {
        
    }

    public override void OnEnter()
    {
        moveDirection = player.inputReader.moveAction.ReadValue<Vector2>();
        player.inputReader.moveEvent += OnMoveInput;
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        Vector3 forward = player.cameraTransform.forward;
        Vector3 right = player.cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardMovement = forward * moveDirection.y;
        Vector3 sideMovement = right * moveDirection.x;

        Vector3 movement = forwardMovement + sideMovement;

        if (movement.sqrMagnitude > 0f)
        {
            player.transform.rotation = Quaternion.LookRotation(movement);
        }

        player.transform.position += movement * player.moveSpeed * Time.fixedDeltaTime;
    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        moveDirection = Vector2.zero;
    }

    private void OnMoveInput(Vector2 direction)
    {
        moveDirection = direction;

        if(direction == Vector2.zero)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

}
