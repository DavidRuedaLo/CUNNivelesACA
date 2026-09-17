using UnityEngine;

public class LevelTransitionTrigger : MonoBehaviour
{
    public Transform destinationPoint;
    public TransitionManager transitionManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            Debug.Log("Player entered transition point");
            PlayerController player = other.GetComponent<PlayerController>();
            if(player != null)
            {
                transitionManager.StartTransition(player, destinationPoint.position, destinationPoint.rotation);
            }
        }
    }

}
