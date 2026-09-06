using UnityEngine;

public class Deployer : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private Transform spawnPoint;
    
    [Header("Audio")]
    public AudioClip activateSound;

    private void Awake()
    {
        spawnPoint = transform.Find("SpawnPoint");
    }

    public void Deploy()
    {
        if(prefabToSpawn == null)
        {
            Debug.Log("No prefab to spawn found on deployer");
            return;
        }

        Vector3 position = spawnPoint.position;
        Quaternion rotation = spawnPoint.rotation;

        GameObject newInstance = Instantiate(prefabToSpawn, position, rotation);
        SoundManager.Instance.Play3DSound(activateSound, transform.position);
    }
}
