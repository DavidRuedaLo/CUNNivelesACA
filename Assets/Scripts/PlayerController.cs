using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    [Header("Player Attributes")]
    public float moveSpeed = 5f;
    
    [Header("Referemces")]
    public InputReader inputReader;

    //Camera ref
    public Camera mainCamera;
    public Transform cameraTransform;
    //FSM setup
    public PlayerSM stateMachine;
    
    //player states
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}

    void Awake()
    {
        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);

        stateMachine = GetComponent<PlayerSM>();

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
}
