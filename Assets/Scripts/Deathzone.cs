using UnityEngine;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {   
        //handle player death
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if(player != null)
            {
                player.Respawn();
            }
        }

        //handle prop death
        //props are detected through posession of an interface

        IRespawnableProp respawnableProp = other.GetComponentInParent<IRespawnableProp>();
        if (respawnableProp != null)
        {
            respawnableProp.Respawn();
        }

    }
}
