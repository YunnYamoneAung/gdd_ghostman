using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Optional Audio")]
    public AudioSource uiAudioSource;   // assign AudioSource in inspector
    public AudioClip startClickSound;   // assign your click sound here

    public void LoadGame()
    {
        // play click sound if available
        if (uiAudioSource != null && startClickSound != null)
            uiAudioSource.PlayOneShot(startClickSound);

        // load next scene after short delay
        StartCoroutine(LoadAfterDelay(0.2f));
    }

    private IEnumerator LoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
