using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject
{
    [SerializeField] private InputActionAsset inputActions;
    
    //get actions from the asset
    public InputAction moveAction;
    public InputAction rotateCamRightAction;
    public InputAction rotateCamLeftAction;
    public InputAction interactAction;
    public InputAction jumpAction;
    
    //define events
    public event Action<Vector2> moveEvent;    
    public event Action<bool> rotateCamRightEvent;
    public event Action<bool> rotateCamLeftEvent;
    public event Action interactEvent;
    public event Action interactCanceledEvent;
    public event Action jumpEvent;
    public event Action jumpCanceledEvent;

    private void OnEnable()
    {
        moveAction = inputActions.FindAction("Move");
        if(moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMovePerformed;
        }

        rotateCamRightAction = inputActions.FindAction("Rotate Cam Right");
        if(rotateCamRightAction != null)
        {
            rotateCamRightAction.Enable();
            rotateCamRightAction.performed += OnRotateCamRight;
            rotateCamRightAction.canceled += OnRotateCamRight;
        }
        
        rotateCamLeftAction = inputActions.FindAction("Rotate Cam Left");
        if(rotateCamLeftAction != null)    
        {
            rotateCamLeftAction.Enable();
            rotateCamLeftAction.performed += OnRotateCamLeft;
            rotateCamLeftAction.canceled += OnRotateCamLeft;
        }

        interactAction = inputActions.FindAction("Interact");
        if(interactAction != null)
        {
            interactAction.Enable();
            interactAction.performed += OnInteractPerformed;
            interactAction.canceled += OnInteractCanceled;
        }

        jumpAction = inputActions.FindAction("Jump");
        if(jumpAction != null)
        {
            jumpAction.Enable();
            jumpAction.performed += OnJumpPerformed;
            jumpAction.canceled += OnJumpCanceled;
        }
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMovePerformed;

        rotateCamRightAction.performed -= OnRotateCamRight;
        rotateCamRightAction.canceled -= OnRotateCamRight;

        rotateCamLeftAction.performed -= OnRotateCamLeft;
        rotateCamLeftAction.canceled -= OnRotateCamLeft;

        interactAction.performed -= OnInteractPerformed;
        interactAction.canceled -= OnInteractCanceled;

        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;
    }

    //broadcast signals
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        moveEvent?.Invoke(direction);
    }

    private void OnRotateCamRight(InputAction.CallbackContext context)
    {
        rotateCamRightEvent?.Invoke(context.ReadValueAsButton());
    }

    private void OnRotateCamLeft(InputAction.CallbackContext context)
    {
        rotateCamLeftEvent?.Invoke(context.ReadValueAsButton());
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        interactEvent?.Invoke();
    }
    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        interactCanceledEvent?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpEvent?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpCanceledEvent?.Invoke();
    }
}
 