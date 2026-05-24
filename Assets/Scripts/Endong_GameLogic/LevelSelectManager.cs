using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Start()
    {
        UpdateLevelButtons();
    }

    void UpdateLevelButtons()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Level 1 always unlocked
        level1Button.interactable = true;

        // Level 2 unlocks after Level 1 complete
        level2Button.interactable = unlockedLevel >= 2;

        // Level 3 unlocks after Level 2 complete
        level3Button.interactable = unlockedLevel >= 3;
    }

    public void OpenLevel(int levelNumber)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (levelNumber <= unlockedLevel)
        {
            SceneManager.LoadScene("Level" + levelNumber);
        }
        else
        {
            Debug.Log("Level " + levelNumber + " is locked.");
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.SetInt("UnlockedLevel", 1);
        PlayerPrefs.Save();

        UpdateLevelButtons();

        Debug.Log("Progress reset. Only Level 1 is unlocked.");
    }
}