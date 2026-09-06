using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    private Animator anim;

    [Header("Audio")]
    public AudioClip activateSound;
    public AudioClip deactivateSound;

    public enum PlateState
    {
        Off = 0,
        Activate =  1,
        On = 2,
        Deactivate = 3,
    }

    //hashset to track unique colliders and avoid double-counting
    private HashSet<Collider> occupants = new HashSet<Collider>();

    public UnityEvent onActivate;
    public UnityEvent onDeactivate;

    private bool isActive;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") 
        || other.CompareTag("PushableBox") 
        || other.CompareTag("PushableBoxRed")
        || other.CompareTag("PushableBoxGreen")
        || other.CompareTag("PushableBoxBlue"))
        {
            if (occupants.Count == 0)
            {
                TogglePlate();
            }

            Debug.Log("player entered switch");

            occupants.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(occupants.Contains(other))
        {
            occupants.Remove(other);

            if (occupants.Count == 0)
            {
                TogglePlate();
            }
        }
    }

    public void TogglePlate()
    {
        isActive = !isActive;

        if (isActive)
        {   
            anim.SetInteger("PlateState", (int)PlateState.Activate);
            SoundManager.Instance.Play3DSound(activateSound, transform.position);
            onActivate?.Invoke();
        }
        else
        {   
            anim.SetInteger("PlateState", (int)PlateState.Deactivate);
            SoundManager.Instance.Play3DSound(deactivateSound, transform.position);
            onDeactivate?.Invoke();
        }
    }
}
