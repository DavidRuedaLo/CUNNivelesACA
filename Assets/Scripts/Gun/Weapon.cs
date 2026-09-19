using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioClip fireSound;

    public LineRenderer laserLine;
    public float laserRange = 50f;

    private void Awake()
    {
        if(laserLine != null)
        {
            laserLine.enabled = false;
        }
    }

    public void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if(fireSound != null)
        {
            SoundManager.Instance.Play3DSound(fireSound, firePoint.position);
        }
    }

    public void ToggleLaser(bool isActive)
    {
        if(laserLine != null)
        {
            laserLine.enabled = isActive;
        }
    }

    public void UpdateLaser()
    {
        if(laserLine != null && laserLine.enabled)
        {
            laserLine.SetPosition(0, firePoint.position);
            laserLine.SetPosition(1, firePoint.position + (firePoint.forward * laserRange));
        }
    }
}
