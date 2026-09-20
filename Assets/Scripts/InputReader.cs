using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
public class InputReader : ScriptableObject
{
    [SerializeField] private InputActionAsset inputActions;
    
    //get actions from the asset
    public InputAction moveAction;
    public InputAction interactAction;
    public InputAction jumpAction;
    public InputAction fireAction;
    public InputAction panelAction;
    public InputAction lookAction;
    public InputAction aimAction;
    public InputAction walkAction;
    
    //define events
    public event Action<Vector2> moveEvent;    
    public event Action interactEvent;
    public event Action interactCanceledEvent;
    public event Action jumpEvent;
    public event Action jumpCanceledEvent;
    public event Action fireEvent;
    public event Action panelEvent;
    public event Action<Vector2> lookEvent;
    public event Action aimEvent;
    public event Action aimCanceledEvent;
    public event Action walkEvent;
    public event Action walkCanceledEvent;

    private void OnEnable()
    {
        moveAction = inputActions.FindAction("Move");
        if(moveAction != null)
        {
            moveAction.Enable();
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMovePerformed;
        }

        lookAction = inputActions.FindAction("Look");
        if(lookAction != null)
        {
            lookAction.Enable();
            lookAction.performed += OnLookPerformed;
            lookAction.canceled += OnLookPerformed;
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

        fireAction = inputActions.FindAction("Fire");
        if(fireAction != null)
        {
            fireAction.Enable();
            fireAction.performed += OnFirePerformed;
        }

        panelAction = inputActions.FindAction("Panel");
        if(panelAction != null)
        {
            panelAction.Enable();
            panelAction.performed += OnPanelTogglePerformed;
        }

        aimAction = inputActions.FindAction("Aim");
        if(aimAction != null)
        {
            aimAction.Enable();
            aimAction.performed += OnAimPerformed;
            aimAction.canceled += OnAimCanceled;
        }

        walkAction = inputActions.FindAction("Walk");
        if(walkAction != null)
        {
            walkAction.Enable();
            walkAction.performed += OnWalkPerformed;
            walkAction.canceled += OnWalkCanceled;
        }
        
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMovePerformed;

        interactAction.performed -= OnInteractPerformed;
        interactAction.canceled -= OnInteractCanceled;

        jumpAction.performed -= OnJumpPerformed;
        jumpAction.canceled -= OnJumpCanceled;

        fireAction.performed -= OnFirePerformed;
        
        if (panelAction != null)
        {
            panelAction.performed -= OnPanelTogglePerformed;
        }
        
        lookAction.performed -= OnLookPerformed;
        lookAction.canceled -= OnLookPerformed;

        if (aimAction != null)
        {
            aimAction.performed -= OnAimPerformed;
            aimAction.canceled -= OnAimCanceled;
        }

        if(walkAction != null)
        {
            walkAction.performed -= OnWalkPerformed;
            walkAction.canceled -= OnWalkCanceled;
        }
    }

    //broadcast signals
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        moveEvent?.Invoke(direction);
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

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        fireEvent?.Invoke(); 
    }

    private void OnPanelTogglePerformed(InputAction.CallbackContext context)
    {
        panelEvent?.Invoke();
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnAimPerformed(InputAction.CallbackContext context)
    {
        aimEvent?.Invoke();
    }

    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        aimCanceledEvent?.Invoke();
    }

    private void OnWalkPerformed(InputAction.CallbackContext context)
    {
        walkEvent?.Invoke();
    }

    private void OnWalkCanceled(InputAction.CallbackContext context)
    {
        walkCanceledEvent?.Invoke();
    }

    //methods to disable player input when needed
    public void DisablePlayerInput()
    {
        inputActions.FindActionMap("Player")?.Disable();
    }

    public void EnablePlayerInput()
    {
        inputActions.FindActionMap("Player")?.Enable();
    }
}
 