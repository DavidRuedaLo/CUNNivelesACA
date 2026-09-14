using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    [SerializeField] private float lookSensitivity = 150f;
    public InputReader inputReader;
    public CinemachineOrbitalFollow orbitalFollow;
    private Vector2 currentLookInput;

    private void OnEnable()
    {
        if(inputReader != null)
        {
            inputReader.lookEvent += OnLook;
        }
    }

    private void OnDisable()
    {
        if(inputReader != null)
        {
            inputReader.lookEvent -= OnLook;
        }
    }

    private void Update()
    {
        if (orbitalFollow != null && currentLookInput.sqrMagnitude > 0.01f)
        {
            orbitalFollow.HorizontalAxis.Value += currentLookInput.x * lookSensitivity * Time.deltaTime;
        }
    }

    private void OnLook(Vector2 lookDelta)
    {
        currentLookInput = lookDelta;
    }
}
