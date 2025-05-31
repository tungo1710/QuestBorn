using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // --- UI References ---
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject gameOverPanel;

    // --- Game State ---
    private int score = 0;
    private bool isPaused = false;
    public static int levelCount = 0;

    // --- Scene Names ---
    private const string LevelSceneName = "Level1";
    private const string MenuSceneName = "Menu";

    private void Start()
    {
        score = PlayerData.coin;
        UpdateScoreUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscapeKey();
        }
    }

    private void HandleEscapeKey()
    {
        if (optionPanel != null && optionPanel.activeSelf)
        {
            ClosePanel(optionPanel);
        }
        else if (settingsPanel != null && settingsPanel.activeSelf)
        {
            ClosePanel(settingsPanel);
        }
        else
        {
            ToggleSettings();
        }
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ResumeTime();
    }

    // --- Score Management ---

    public void AddScore(int points)
    {
        score += points;
        PlayerData.coin = score;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // --- Settings and Pause ---

    public void ToggleSettings()
    {
        isPaused = !isPaused;
        if (settingsPanel != null)
            settingsPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        ResumeTime();
    }

    public void SettingGame()
    {
        PauseTime();
        if (optionPanel != null && setting != null)
        {
            optionPanel.SetActive(true);
            setting.SetActive(false); 
        }
    }

    public void ExitSetting()
    {
        PauseTime();

        if (optionPanel != null && setting != null)
        {
            optionPanel.SetActive(false);
            setting.SetActive(true); 
        }
    }

    private void PauseTime() => Time.timeScale = 0f;
    private void ResumeTime()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    // --- Scene Control ---

    public void RestartGame()
    {
        ResetPlayerData();
        ResumeTime();
        SceneManager.LoadScene(LevelSceneName);
    }

    public void GoToMenu()
    {
        ResetPlayerData();
        ResumeTime();
        SceneManager.LoadScene(MenuSceneName);
    }

    private void ResetPlayerData()
    {
        PlayerData.coin = 0;
        PlayerData.health = 100;
    }

    // --- Game Over ---

    public void ShowGameOver()
    {
        PauseTime();

        if (coinText != null)
            coinText.text = "Coins: " + PlayerData.coin;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
