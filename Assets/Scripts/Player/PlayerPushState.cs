using UnityEngine;

public class PlayerPushState : PlayerBaseClass
{
    public PlayerPushState(PlayerController player) : base(player){}

    private PushableBox currentBox;
    private Vector3 lockedAxis;
    private float pushSpeed = 4f;
    private Vector2 moveDirection;
    //private Vector2 currentMoveInput;

    public void SetInteractableBox(PushableBox box)
    {
        currentBox = box;
    }

    public override void OnEnter()
    {
        moveDirection = player.inputReader.moveAction.ReadValue<Vector2>();

        player.inputReader.moveEvent += OnMoveInput;
        player.inputReader.interactCanceledEvent += OnInteractReleased;

        if(currentBox != null)
        {
            lockedAxis = currentBox.GetLockedAxis(player.transform.position);
            currentBox.beingPushed = true;
        }
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        if (moveDirection.sqrMagnitude == 0) return;

        Vector3 forward = player.cameraTransform.forward;
        Vector3 right = player.cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 intendedMovement = (forward * moveDirection.y) + (right * moveDirection.x);

        float alignment = Vector3.Dot(intendedMovement, lockedAxis);

        if (Mathf.Abs(alignment) > 0.05f)
        {
            Vector3 movementDelta = lockedAxis * (alignment * pushSpeed * Time.fixedDeltaTime);

            player.rb.MovePosition(player.rb.position + movementDelta);
            currentBox.MoveBox(movementDelta);
        }

        
    }

    public override void OnExit()
    {
        player.inputReader.moveEvent -= OnMoveInput;
        player.inputReader.interactCanceledEvent -= OnInteractReleased;
        
        if(currentBox != null)
        {
            currentBox.beingPushed = false;
            currentBox.prompt.SetActive(true);
        }

        currentBox = null;
        moveDirection = Vector2.zero;
    }

    private void OnInteractReleased()
    {
        player.stateMachine.ChangeState(player.idleState);
    }

    private void OnMoveInput(Vector2 direction)
    {
        moveDirection = direction;
    }
}
