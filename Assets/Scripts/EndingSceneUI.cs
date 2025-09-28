using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneUI : MonoBehaviour
{
    [SerializeField] private Text winnerText;

    [Header("Ending Sounds")]               //  this creates a header in Inspector
    [SerializeField] private AudioClip ghostVictoryClip;  // field 1
    [SerializeField] private AudioClip pacmanDefeatClip;  // field 2

    private void Start()
    {
        string winner = PlayerPrefs.GetString("Winner", "PAC-MAN WINS!");
        if (winnerText != null)
            winnerText.text = winner;

        // Play the right sound
        if (AudioManager.Instance != null)
        {
            if (winner.Contains("GHOST") && ghostVictoryClip != null)
            {
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySound(ghostVictoryClip);
            }
            else if (winner.Contains("PAC-MAN") && pacmanDefeatClip != null)
            {
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySound(pacmanDefeatClip);
            }
        }
    }

    public void OnPlayAgain()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds();

        SceneManager.LoadScene("Gameplay_Ghostman"); // use your scene name
    }

    public void OnMainMenu()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds();

        SceneManager.LoadScene("TitleScene");
    }
}
