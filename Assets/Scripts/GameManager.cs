using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private PacmanAI pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text readyText;

    [SerializeField] private FruitSpawner fruitSpawner;
    [SerializeField] private Slider powerupBar;     // Fruit speed boost
    [SerializeField] private Slider frightenedBar;  // Frightened mode bar

    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private int ghostMultiplier = 1;
    private int pelletsEatenThisRound = 0;

    private float powerupTimer = 0f;
    private bool powerupActive = false;

    private float frightenedTimer = 0f;
    private bool frightenedActive = false;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        if (powerupBar != null)
            powerupBar.gameObject.SetActive(false);

        if (frightenedBar != null)
            frightenedBar.gameObject.SetActive(false);

        readyText.enabled = false; 
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();
        }

        // Power-up countdown
        if (powerupActive)
        {
            powerupTimer -= Time.deltaTime;
            if (powerupBar != null)
                powerupBar.value = Mathf.Max(0f, powerupTimer);

            if (powerupTimer <= 0f)
                EndPowerup();
        }

        // Frightened countdown
        if (frightenedActive)
        {
            frightenedTimer -= Time.deltaTime;
            if (frightenedBar != null)
                frightenedBar.value = Mathf.Max(0f, frightenedTimer);

            if (frightenedTimer <= 0f)
                EndFrightened();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;
        pelletsEatenThisRound = 0;
        readyText.enabled = false; 

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        StartCoroutine(ShowReadyAndStart());
    }

    private IEnumerator ShowReadyAndStart()
    {
        pacman.gameObject.SetActive(false);
        foreach (var g in ghosts) g.gameObject.SetActive(false);

        readyText.enabled = true;
        readyText.text = "READY!";

        yield return new WaitForSeconds(2f);

        readyText.enabled = false; 

        ResetState();

        pacman.gameObject.SetActive(true);
        foreach (var g in ghosts) g.gameObject.SetActive(true);
    }

    private void ResetState()
    {
        foreach (var g in ghosts)
            g.ResetState();

        pacman.ResetState();
    }

    private void GameOver(bool ghostWin)
    {
        // Save winner so EndingScene can show it
        string winner = ghostWin ? "GHOST WINS!" : "YOU LOST!";
        PlayerPrefs.SetString("Winner", winner);
        PlayerPrefs.Save();

        //  Stop all sounds
        AudioManager.Instance.StopAllSounds();

        // Switch to EndingScene
        SceneManager.LoadScene("EndingScene");
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();
        AudioManager.Instance.PlaySound(AudioManager.Instance.deathSound);

        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver(true); // Ghost wins
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
        ghost.gameObject.SetActive(false);

        Invoke(nameof(ResetState), 1.5f);

        AudioManager.Instance.PlaySound(AudioManager.Instance.eatGhostSound);
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(score + pellet.points);

        pelletsEatenThisRound++;
        CheckFruitSpawn();

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            GameOver(false); // Pac-Man wins
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        foreach (var g in ghosts)
            g.frightened.Enable(pellet.duration);

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);

        StartFrightened(pellet.duration); 
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private void CheckFruitSpawn()
    {
        if (pelletsEatenThisRound > 0 && pelletsEatenThisRound % 30 == 0)
            fruitSpawner.SpawnFruit();
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
            if (pellet.gameObject.activeSelf)
                return true;

        return false;
    }

    // Powerup (Fruit Boost)
    public void StartPowerup(float duration)
    {
        powerupTimer = duration;
        powerupActive = true;

        if (powerupBar != null)
        {
            powerupBar.gameObject.SetActive(true);
            powerupBar.maxValue = duration;
            powerupBar.value = duration;
        }
    }

    private void EndPowerup()
    {
        powerupActive = false;
        if (powerupBar != null)
            powerupBar.gameObject.SetActive(false);
    }

    // Frightened (Blue Mode)
    public void StartFrightened(float duration)
    {
        frightenedTimer = duration;
        frightenedActive = true;

        if (frightenedBar != null)
        {
            frightenedBar.gameObject.SetActive(true);
            frightenedBar.maxValue = duration;
            frightenedBar.value = duration;
        }
    }

    private void EndFrightened()
    {
        frightenedActive = false;
        if (frightenedBar != null)
            frightenedBar.gameObject.SetActive(false);
    }
}
