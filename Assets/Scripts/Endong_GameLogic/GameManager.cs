using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool gameStarted;
    public bool gameEnded;

    [Header("Score and Progress")]
    public int score = 0;
    public float facilityProgress = 0f;

    /*[Header("UI Text")]
    public Text scoreText;
    public Text progressText;
    public Text resultText;

    [Header("UI Panels")]
    public GameObject gameOverPanel;
    public GameObject missionCompletePanel;*/

    [Header("Level Settings")]
    public int currentLevelNumber = 1;
    public string levelSelectSceneName = "LevelSelect";
    public string nextLevelName;

    private float levelStartTime;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        //ResetProgress(); //temporary
        gameStarted = true;
        gameEnded = false;

        levelStartTime = Time.time;

        // UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        
    }

    public void IncreaseFacilityProgress(float amount)
    {
        facilityProgress = Mathf.Clamp(facilityProgress + amount, 0f, 100f);
        
    }

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        gameStarted = false;

        Time.timeScale = 0f;

        StartCoroutine(RestartLevel());

    }
    IEnumerator RestartLevel()
    {
        //Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MissionComplete()
    {
        if (gameEnded) return;

        gameEnded = true;
        gameStarted = false;

        float timeTaken = Time.time - levelStartTime;

        GameResult.levelNumber = currentLevelNumber;

        // Save results
        GameResult.levelNumber = currentLevelNumber;
        GameResult.score = score;
        GameResult.timeTaken = timeTaken;

        int threeStarScore = 0;
        int twoStarScore = 0;

        switch (currentLevelNumber)
        {
            case 1:
                threeStarScore = 150;
                twoStarScore = 80;
                break;

            case 2:
                threeStarScore = 450;
                twoStarScore = 250;
                break;

            case 3:
                threeStarScore = 850;
                twoStarScore = 500;
                break;
        }

        if (score >= threeStarScore)
        {
            GameResult.stars = 3;
        }
        else if (score >= twoStarScore)
        {
            GameResult.stars = 2;
        }
        else
        {
            GameResult.stars = 1;
        }

        // SAVE CURRENT LEVEL
        PlayerPrefs.SetInt(
            "SelectedLevel",
            currentLevelNumber
        );

        // SAVE COMPLETION
        PlayerPrefs.SetInt(
            "Level" + currentLevelNumber + "Completed",
            1
        );

        // SAVE STARS
        PlayerPrefs.SetInt(
            "Level" + currentLevelNumber + "Stars",
            GameResult.stars
        );

        PlayerPrefs.Save();

        UnlockNextLevel();

        SceneManager.LoadScene("Result");
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

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }

}