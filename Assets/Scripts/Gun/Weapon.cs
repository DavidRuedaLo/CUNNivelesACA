using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioClip fireSound;

    public void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if(fireSound != null)
        {
            SoundManager.Instance.Play3DSound(fireSound, firePoint.position);
        }
    }
}
