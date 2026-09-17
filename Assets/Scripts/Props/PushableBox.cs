using System;
using UnityEngine;

public class PushableBox : MonoBehaviour, IRespawnableProp
{
    private Rigidbody rb;
    private Camera mainCamera;
    public GameObject prompt;
    public bool beingPushed;
    
    public float respawnOffset = 7.5f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        mainCamera = Camera.main;
        prompt.SetActive(false);
        beingPushed = false;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {   
        //make the prompt appear as a billboard
        if(prompt.activeInHierarchy)
        {
            prompt.transform.rotation = mainCamera.transform.rotation;
        }

        if(beingPushed)
        {
            prompt.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            prompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            prompt.SetActive(false);
        }
    }

    public void MoveBox(Vector3 movementDelta)
    {
        rb.MovePosition(rb.position + movementDelta);
    }

    //calculate the dot product between the box's and the player's movement vectors
    //to determine which axis to lock
    public Vector3 GetLockedAxis(Vector3 playerPosition)
    {
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

        float dotForward = Mathf.Abs(Vector3.Dot(directionToPlayer, transform.forward));
        float dotRight = Mathf.Abs(Vector3.Dot(directionToPlayer, transform.right));

        if(dotForward > dotRight)
        {
            return transform.forward;
        }
        else
        {
            return transform.right;
        }
    }

    //method to be called when respawning after touching a deathzone
    public void Respawn()
    {   
        Debug.Log("Touched the deathzone!");
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Vector3 dropPosition = initialPosition;
        dropPosition.y += respawnOffset;

        transform.position = dropPosition;
        transform.rotation = initialRotation;
    }
}
