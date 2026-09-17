using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public InputReader inputReader;
    public GameObject instructionsPanel;

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.panelEvent += HandleToggle;
        }
    }

    private void Osable()
    {
        if (inputReader != null)
        {
            inputReader.panelEvent -= HandleToggle;
        }
    }

    private void HandleToggle()
    {
        if(instructionsPanel != null)
        {
            instructionsPanel.SetActive(!instructionsPanel.activeSelf);
        }
    }
}
