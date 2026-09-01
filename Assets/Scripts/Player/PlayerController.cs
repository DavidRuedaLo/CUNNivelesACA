using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float jumpCutMultiplier = 0.5f;
    public float fallMultiplier = 2.5f;
    public float maxFallSpeed = -20f;
    public int maxJumps = 2;
    public int jumpsRemaining;
    
    [Header("Referemces")]
    public InputReader inputReader;
    public Rigidbody rb;
    public LayerMask groundLayer;

    //Camera ref
    public Camera mainCamera;
    public Transform cameraTransform;
    
    //FSM setup
    public PlayerSM stateMachine;

    //interactables
    public PushableBox activePushableBox {get; private set;}
    
    //player states
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerPushState pushState {get; private set;}
    public PlayerJumpStartState jumpStartState {get; private set;}
    public PlayerJumpMidState jumpMidState {get; private set;}
    public PlayerJumpEndState jumpEndState {get; private set;}

    void Awake()
    {
        //instance states to be used
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        pushState = new PlayerPushState(this);
        jumpStartState = new PlayerJumpStartState(this);
        jumpMidState = new PlayerJumpMidState(this);
        jumpEndState = new PlayerJumpEndState(this);

        stateMachine = GetComponent<PlayerSM>();
        rb = GetComponent<Rigidbody>();

        if(stateMachine != null)
        {
            stateMachine.Initialize(idleState);
        }
        else
        {
            Debug.Log("Can't find FSM for player!");
        }
        
        //get the main camera transform for further stuff
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
       
        if(mainCamera != null)
        {
            Debug.Log("Found main camera!");
            
            if(cameraTransform != null)
            {
                Debug.Log("Found main camera transform!");
            }
            else
            {
                Debug.Log("Can't find main camera transform!");
            }
        }
        else
        {
            Debug.Log("Can't find main camera to attach!");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PushableBox box = other.GetComponentInParent<PushableBox>();
        if (box != null)
        {
            activePushableBox = box;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PushableBox box = other.GetComponentInParent<PushableBox>();
        if (box != null && box == activePushableBox)
        {
            activePushableBox = null;
        }
    }

    public bool IsGrounded()
    {
        float sphereRadius = 0.3f;

        float castDistance = 1.1f;

        return Physics.SphereCast(transform.position, sphereRadius, Vector3.down,
        out RaycastHit hit, castDistance, groundLayer);
    }

    //used to determine move direction when handling things like camera rotation and jumps
    public Vector2 CurrentMoveInput => inputReader.moveAction.ReadValue<Vector2>();

    //used to keep movement input tied to the camera's directions and not the world's
    public void HandleRotatedMovement(Vector2 moveInput, float speed)
    {
        if(moveInput.sqrMagnitude > 0f)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            
            forward = forward.normalized;
            right = right.normalized;

            Vector3 intendedMovement = (forward * moveInput.y) + (right * moveInput.x);

            transform.rotation = Quaternion.LookRotation(intendedMovement);

            transform.position += intendedMovement * moveSpeed * Time.fixedDeltaTime;
        }
    }

    //multiple jump logic
    //called when touching the ground to give new jumps
    public void ResetJumps()
    {
        jumpsRemaining = maxJumps;
    }

    //used to check if there are jumps available
    public bool CanJump()
    {
        return jumpsRemaining > 0;
    }

    //takes one jump from pool with each use
    public void ConsumeJump()
    {
        jumpsRemaining--;
    }
}
