using UnityEngine;
using UnityEngine.Events;
public class Lever : MonoBehaviour
{   
    private Rigidbody rb;
    private Animator anim;
    private Camera mainCamera;
    public GameObject prompt;

    [Header("Audio")]
    public AudioClip activateSound;
    public AudioClip deactivateSound;

    public enum LeverState
    {
        Off = 0,
        Activate =  1,
        On = 2,
        Deactivate = 3,
        
    }

    //Unity events to broadcast signals to other objects
    public UnityEvent onActivate;
    public UnityEvent onDeactivate;
    private bool isActive;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //make the prompt appear as a billboard
        if(prompt.activeInHierarchy)
        {
            prompt.transform.rotation = mainCamera.transform.rotation;
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

    public void ToggleLever()
    {
        isActive = !isActive;

        if (isActive)
        {   
            anim.SetInteger("LeverState", (int)LeverState.Activate);
            SoundManager.Instance.Play3DSound(activateSound, transform.position);
            onActivate?.Invoke();
        }
        else
        {   
            anim.SetInteger("LeverState", (int)LeverState.Deactivate);
            SoundManager.Instance.Play3DSound(deactivateSound, transform.position);
            onDeactivate?.Invoke();
        }
    }
}
