using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator anim;
    public enum DoorState
    {
        Off = 0,
        Activate =  1,
        On = 2,
        Deactivate = 3,
    }

    private bool isActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDoor()
    {
        isActive = !isActive;

        if (isActive)
        {   
            anim.SetInteger("DoorState", (int)DoorState.Activate);
        }
        else
        {   
            anim.SetInteger("DoorState", (int)DoorState.Deactivate);
        }
    }
}
