using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public bool debugMode;
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;
    public float jumpCutMultiplier = 0.5f;
    public float fallMultiplier = 2.5f;
    public float maxFallSpeed = -20f;
    public int maxJumps = 2;
    public int jumpsRemaining;
    public float landingDuration = 0.3f;
    public float sphereRadius = 0.3f;
    public float castDistance = 1.1f;

    [Header("Equipment")]
    public Transform weaponSocket;
    private GameObject currentWeapon;
    private Weapon equippedWeaponScript;
    
    [Header("References")]
    public InputReader inputReader;
    public Rigidbody rb;
    public LayerMask groundLayer;
    public CinemachineCamera cmCam;
    public Animator anim;
    
    //safe positions for respawn
    public Vector3 currentSpawnPoint;

    //Camera ref
    public Camera mainCamera;
    public Transform cameraTransform;
    
    //FSM setup
    public PlayerSM stateMachine;

    //interactables
    public PushableBox activePushableBox {get; private set;}
    public Lever activeLever {get; private set;}
    
    //player states
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerPushState pushState {get; private set;}
    public PlayerJumpStartState jumpStartState {get; private set;}
    public PlayerJumpMidState jumpMidState {get; private set;}
    public PlayerJumpEndState jumpEndState {get; private set;}
    public PlayerAimState aimState {get; private set;}

    void Awake()
    {
        //instance states to be used
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        pushState = new PlayerPushState(this);
        jumpStartState = new PlayerJumpStartState(this);
        jumpMidState = new PlayerJumpMidState(this);
        jumpEndState = new PlayerJumpEndState(this);
        aimState = new PlayerAimState(this);

        stateMachine = GetComponent<PlayerSM>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


        if(stateMachine != null)
        {
            stateMachine.playerController = this;
            stateMachine.Initialize(idleState);
        }
        else if (debugMode)
        {
            Debug.Log("Can't find FSM for player!");
        }
        
        //get the main camera transform for further stuff
        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }


       
        if(mainCamera != null)
        {
            if (debugMode)
            {
                Debug.Log("Found main camera!");
            }
            
            if(cameraTransform != null)
            {
                if (debugMode)
                {
                    Debug.Log("Found main camera transform!");
                }
            }
            else if (debugMode)
            {
                Debug.Log("Can't find main camera transform!");
            }
        }
        else if (debugMode)
        {
            Debug.Log("Can't find main camera to attach!");
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //detect pushable boxes
        PushableBox box = other.GetComponentInParent<PushableBox>();
        if (box != null)
        {
            activePushableBox = box;
        }

        //detect levers
        Lever lever = other.GetComponentInParent<Lever>();
        if (lever != null)
        {
            activeLever = lever;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PushableBox box = other.GetComponentInParent<PushableBox>();
        if (box != null && box == activePushableBox)
        {
            activePushableBox = null;
        }

        Lever lever = other.GetComponentInParent<Lever>();
        if (lever != null && lever == activeLever)
        {
            activeLever = null;
        }
    }

    public bool IsGrounded()
    {
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

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if(currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        currentWeapon = Instantiate(weaponPrefab, weaponSocket);

        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        equippedWeaponScript = currentWeapon.GetComponent<Weapon>();
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = currentSpawnPoint;

        if(cmCam != null)
        {
            cmCam.PreviousStateIsValid = false;
        }
    }

    //weapon handling

    public void FireWeapon()
    {
        if(equippedWeaponScript != null)
        {
            equippedWeaponScript.Fire();
        }
    }

    public void ToggleWeaponLaser(bool isActive)
    {
        if(equippedWeaponScript != null)
        {
            equippedWeaponScript.ToggleLaser(isActive);
        }
    }

    public void UpdateWeaponLaser()
    {
        if(equippedWeaponScript != null)
        {
            equippedWeaponScript.UpdateLaser();
        }
    }

}
