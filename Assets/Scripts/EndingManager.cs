using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float fadeDuration = 1.0f;
    
    private bool hasTriggered = false;

    private void Start()
    {
        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f;
            panelCanvasGroup.interactable = false;
            panelCanvasGroup.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(FadeInPanel());
            inputReader.DisablePlayerInput();
        }
    }

    private IEnumerator FadeInPanel()
    {
        float elapsedTime = 0f;
        float startAlpha = panelCanvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        panelCanvasGroup.alpha = 1f;
        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;
    }
}