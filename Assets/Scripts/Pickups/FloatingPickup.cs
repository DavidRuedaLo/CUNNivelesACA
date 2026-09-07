using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float spinSpeed = 100f;
    public float bobbleAmplitude = 0.5f;
    public float bobbleSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        float newY = startPosition.y + (Mathf.Sin(Time.time * bobbleSpeed) * bobbleAmplitude);
        
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
