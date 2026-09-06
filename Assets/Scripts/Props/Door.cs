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

    [Header("Audio")]
    public AudioClip activateSound;
    public AudioClip deactivateSound;

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
            SoundManager.Instance.Play3DSound(activateSound, transform.position);

        }
        else
        {   
            anim.SetInteger("DoorState", (int)DoorState.Deactivate);
            SoundManager.Instance.Play3DSound(deactivateSound, transform.position);
        }
    }
}
