using System;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;
    public GameObject prompt;
    public bool beingPushed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        mainCamera = Camera.main;
        prompt.SetActive(false);
        beingPushed = false;
    }

    // Update is called once per frame
    void Update()
    {
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
}
