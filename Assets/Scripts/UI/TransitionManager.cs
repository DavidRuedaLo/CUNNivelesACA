using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image wipeImage;
    public CameraController cameraController;

    public float transitionSpeed = 3f;
    public float maxRadius = 1.5f;

    public InputReader inputReader;
    private Material wipeMaterial;

    private void Awake()
    {
        if(wipeImage != null)
        {   
            //clone the material to prevent edits to saved asset
            wipeMaterial = new Material(wipeImage.material);
            wipeImage.material = wipeMaterial;

            wipeMaterial.SetFloat("_Radius", maxRadius);
        }
    }

    public void StartTransition(PlayerController player, Vector3 newPosition, Quaternion newRotation)
    {
        StartCoroutine(TransitionRoutine(player, newPosition, newRotation));
    }
    private IEnumerator TransitionRoutine(PlayerController player, Vector3 newPosition, quaternion newRotation)
    {
        if(inputReader != null)
        {
            inputReader.DisablePlayerInput();
        }

        //shrink to black
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            float currentRadius = Mathf.Lerp(maxRadius, 0f, t);
            wipeMaterial.SetFloat("_Radius", currentRadius);
            yield return null;
        }
        wipeMaterial.SetFloat("_Radius", 0f);

        //teleport the player
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
        player.transform.position = newPosition;
        player.transform.rotation = newRotation;

        yield return new WaitForSeconds(0.1f);

        if (cameraController != null)
        {
            cameraController.SnapCameraToRotation(newRotation);
        }

        //grow to clear
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            float currentRadius = Mathf.Lerp(0f, maxRadius, t);
            wipeMaterial.SetFloat("_Radius", currentRadius);
            yield return null;
        }
        wipeMaterial.SetFloat("_Radius", maxRadius);

        if (inputReader != null)
        {
            inputReader.EnablePlayerInput();
        }
    }
}
