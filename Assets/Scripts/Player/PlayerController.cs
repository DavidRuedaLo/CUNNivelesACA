using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    
    [Header("Referemces")]
    public InputReader inputReader;
    public Rigidbody rb;

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

    void Awake()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);
        pushState = new PlayerPushState(this);

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
}
