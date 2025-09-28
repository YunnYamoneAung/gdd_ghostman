using UnityEngine;
using System.Collections;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;

    private void Start()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false); // hidden at start
    }

    public void ShowControls()
    {
        Debug.Log("ShowControls called");
        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        Debug.Log("HideControls called");
        if (controlsPanel != null)
            StartCoroutine(HideAfterSound());
    }

    private IEnumerator HideAfterSound()
    {
        // wait a tiny bit so UIAudio on BackButton can play its click sound
        yield return new WaitForSeconds(0.15f);
        controlsPanel.SetActive(false);
    }
}
