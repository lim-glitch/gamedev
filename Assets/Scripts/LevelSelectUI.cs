using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//can add like highlighted when selected the level -- the ui
public class LevelSelectUI : MonoBehaviour
{
    [System.Serializable]
    public class LevelUI
    {
        public Button button;

        public GameObject lockOverlay;
        public GameObject lockbase;
        public GameObject completedBadge;

        //public Image highlightBorder;

        public Image[] stars;
    }

    [Header("Levels")]
    public LevelUI[] levels;

    [Header("Progress UI")]
    public Image progressBarFill;
    public Text progressText;

    private int selectedLevel = 1;

    void Start()
    {
        //PlayerPrefs.DeleteAll(); //temporraty

        int unlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levels.Length; i++)
        {
            int levelNumber = i + 1;

            bool unlockedLevel = levelNumber <= unlocked;

            // BUTTON INTERACTABLE
            levels[i].button.interactable = unlockedLevel;

            // LOCK OVERLAY
            if (levels[i].lockbase != null)
                levels[i].lockbase.SetActive(!unlockedLevel);

            if (levels[i].lockOverlay != null)
                levels[i].lockOverlay.SetActive(!unlockedLevel);

            // COMPLETED BADGE
            bool completed =
                PlayerPrefs.GetInt("Level" + levelNumber + "Completed", 0) == 1;

            if (levels[i].completedBadge != null)
                levels[i].completedBadge.SetActive(completed);

            // STARS
            int starsEarned =
                PlayerPrefs.GetInt("Level" + levelNumber + "Stars", 0);

            for (int s = 0; s < levels[i].stars.Length; s++)
            {
                levels[i].stars[s].color =
                    (s < starsEarned) ? Color.white : Color.gray;
            }
        }

        UpdateProgressBar();
        //HighlightLevel(1);
    }

    public void SelectLevel(int level)
    {
        selectedLevel = level;

        //HighlightLevel(level);

        PlayerPrefs.SetInt("SelectedLevel", level);

        Debug.Log("Selected Level: " + level);

        SceneManager.LoadScene("AvatarSelect");
    }

    /*void HighlightLevel(int level)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].highlightBorder != null)
            {
                levels[i].highlightBorder.color =
                    (i == level - 1) ? Color.green : Color.gray;
            }
        }
    }*/

    void UpdateProgressBar()
    {
        int completedLevels = 0;

        for (int i = 1; i <= levels.Length; i++)
        {
            completedLevels +=
                PlayerPrefs.GetInt("Level" + i + "Completed", 0);
        }

        float progress =
            (float)completedLevels / levels.Length;

        progressBarFill.fillAmount = progress;

        if(progressText != null)
        {
            progressText.text = Mathf.RoundToInt(progress * 100f) + "%";
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}