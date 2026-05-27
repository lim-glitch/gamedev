using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ResultUI : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject singleCanvas;
    public GameObject multiplayerCanvas;

    [Header("Single UI")]
    public Text scoreText;
    public Text timeText;
    public Text levelHeader;
    public Text levelName;
  
    [Header("Stars UI")]
    public Image star1;
    public Image star2;
    public Image star3;


    /*[Header("Multiplayer UI")]
    public Text p1Text;
    public Text p2Text;
    public Text winnerText;*/

    void Start()
    {
        levelHeader.text = "Level " + GameResult.levelNumber;

        switch (GameResult.levelNumber)
        {
            case 1:
                levelName.text = "The Quarantine Lobby";
                break;

            case 2:
                levelName.text = "The Corrupted Research Wing";
                break;

            case 3:
                levelName.text = "The Robot Factory";
                break;
        }

        // TIME FORMAT
        int minutes =
            Mathf.FloorToInt(
                GameResult.timeTaken / 60
            );

        int seconds =
            Mathf.FloorToInt(
                GameResult.timeTaken % 60
            );

        timeText.text =
            "Time: " +
            minutes.ToString("00") +
            ":" +
            seconds.ToString("00");

        // SHOW STARS
        SetStars(GameResult.stars);

    }

    void SetStars(int stars)
    {
        star1.color = (stars >= 1) ? Color.white : Color.gray;
        star2.color = (stars >= 2) ? Color.white : Color.gray;
        star3.color = (stars >= 3) ? Color.white : Color.gray;

    }

    //if use sprite
    /*void SetStars(int stars)
{
    star1.sprite = (stars >= 1) ? filledStar : emptyStar;
    star2.sprite = (stars >= 2) ? filledStar : emptyStar;
    star3.sprite = (stars >= 3) ? filledStar : emptyStar;
}*/

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Replay()
    {
        int level = PlayerPrefs.GetInt("SelectedLevel", 1);
        SceneManager.LoadScene("Level" + level);
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("SelectedLevel", 1);
        level++;

        if (level > 3) level = 1;

        PlayerPrefs.SetInt("SelectedLevel", level);
        SceneManager.LoadScene("Level" + level);
    }
}