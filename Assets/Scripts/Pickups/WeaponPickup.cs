using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weaponPrefabToGrant;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponentInParent<PlayerController>();

            if(player != null)
            {
                player.EquipWeapon(weaponPrefabToGrant);

                SoundManager.Instance.Play3DSound(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}
