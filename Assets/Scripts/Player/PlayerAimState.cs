using UnityEngine;

public class PlayerAimState : PlayerBaseClass
{
    public PlayerAimState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        player.inputReader.fireEvent += OnFireInput;
        player.inputReader.aimCanceledEvent += OnAimCanceled;

        player.ToggleWeaponLaser(true);

        if(player.anim != null)
        {
            player.anim.CrossFadeInFixedTime("Aiming", 0.1f);
        }
    }

    public override void OnUpdate()
    {
        //rotate the player to match the camera's forward
        Vector3 camForward = player.cameraTransform.forward;
        camForward.y = 0f;

        if(camForward.sqrMagnitude > 0.01f)
        {
            player.transform.rotation = Quaternion.LookRotation(camForward.normalized);
        }

        player.UpdateWeaponLaser();
    }

    public override void OnFixedUpdate()
    {
        //freze horizontal movement
        player.rb.linearVelocity = new Vector3(0f, player.rb.linearVelocity.y, 0f);
    }

    public override void OnExit()
    {
        player.inputReader.fireEvent -= OnFireInput;
        player.inputReader.aimCanceledEvent -= OnAimCanceled;
        player.ToggleWeaponLaser(false);
    }

    private void OnFireInput()
    {
        player.FireWeapon();
    }

    private void OnAimCanceled()
    {
        player.stateMachine.ChangeState(player.idleState);
    }
}
