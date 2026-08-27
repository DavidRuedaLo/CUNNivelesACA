using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{   
    [Header("Camera rotation attributes")]
    [SerializeField] private float stepCooldown = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;
    private float timeSinceLastStep = 0f;
    private bool isRotatingRight;
    private bool isRotatingLeft;
    private float targetAngle;

    [Header("References")]
    public InputReader inputReader;
    public PlayerController player;
    public CinemachineOrbitalFollow orbitalFollow;

    private void Awake()
    {
        inputReader.rotateCamRightEvent += OnRotateCameraRight;
        inputReader.rotateCamLeftEvent += OnRotateCameraLeft;
    }

    private void Start()
    {
        if(orbitalFollow != null)
        {
            targetAngle = orbitalFollow.HorizontalAxis.Value;
        }
    }

    private void Update()
    {
        timeSinceLastStep += Time.deltaTime;

        if(timeSinceLastStep >= stepCooldown)
        {
            if(isRotatingRight)
            {
                targetAngle += 45f;
                timeSinceLastStep = 0f;
            }
            else if(isRotatingLeft)
            {
                targetAngle -= 45f;
                timeSinceLastStep = 0f;
            }
        }

        float currentAngle = orbitalFollow.HorizontalAxis.Value;
        float smoothedAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        orbitalFollow.HorizontalAxis.Value = smoothedAngle;
    }

    private void OnRotateCameraRight(bool isPressed)
    {
        isRotatingRight = isPressed;
    }

      private void OnRotateCameraLeft(bool isPressed)
    {
        isRotatingLeft = isPressed;
    }
}
