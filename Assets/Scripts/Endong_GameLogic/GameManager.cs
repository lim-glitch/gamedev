using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool gameStarted = false;
    public bool gameEnded = false;

    [Header("Level Unlock")]
    public int currentLevelNumber = 1;
    public string levelSelectSceneName = "LevelSelect";

    [Header("Score and Progress")]
    public int score = 0;
    public float facilityProgress = 0f;

    [Header("UI Text")]
    public Text scoreText;
    public Text progressText;
    public Text countdownText;
    public Text resultText;

    [Header("UI Panels")]
    public GameObject briefingPanel;
    public GameObject gameOverPanel;
    public GameObject missionCompletePanel;

    [Header("Level Settings")]
    public string nextLevelName;

    private float levelStartTime;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        gameStarted = false;
        gameEnded = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(false);

        if (briefingPanel != null)
            briefingPanel.SetActive(true);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        UpdateUI();
    }

    public void StartMissionButton()
    {
        StartCoroutine(MissionCountdown());
    }

    IEnumerator MissionCountdown()
    {
        if (briefingPanel != null)
            briefingPanel.SetActive(false);

        if (countdownText != null)
            countdownText.gameObject.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSeconds(1f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
            countdownText.gameObject.SetActive(false);

        gameStarted = true;
        levelStartTime = Time.time;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void SetFacilityProgress(float progress)
    {
        facilityProgress = Mathf.Clamp(progress, 0f, 100f);
        UpdateUI();
    }

    public void IncreaseFacilityProgress(float amount)
    {
        facilityProgress = Mathf.Clamp(facilityProgress + amount, 0f, 100f);
        UpdateUI();
    }

    public void GameOver()
{
    if (gameEnded) return;

    gameEnded = true;
    gameStarted = false;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    if (resultText != null)
        resultText.text = "GAME OVER\nRestarting Level...";

    Time.timeScale = 0f;

    StartCoroutine(RestartAfterGameOver());
}

IEnumerator RestartAfterGameOver()
{
    yield return new WaitForSecondsRealtime(2f);

    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}

    public void MissionComplete()
    {
        if (gameEnded) return;

        gameEnded = true;
        gameStarted = false;
        facilityProgress = 100f;

        UnlockNextLevel();

        float timeTaken = Time.time - levelStartTime;

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(true);

        if (resultText != null)
        {
            resultText.text =
                "CONGRATULATIONS!\n" +
                "Level Finished\n" +
                "Time: " + timeTaken.ToString("F1") + " seconds";
        }

        UpdateUI();
        Time.timeScale = 0f;
    }

    void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int nextLevel = currentLevelNumber + 1;

        if (nextLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();

            Debug.Log("Unlocked Level " + nextLevel);
        }
    }

    public void BackToLevelSelect()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelectSceneName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        if (string.IsNullOrEmpty(nextLevelName)) return;

        Time.timeScale = 1f;
        SceneManager.LoadScene(nextLevelName);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "";

        if (progressText != null)
            progressText.text = "Facility Progress: " + facilityProgress.ToString("F0") + "%";
    }
}