using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BriefingUI : MonoBehaviour
{
    public Text levelText;
    public Text countdownText;
    public Image playerImage;
    public Text playerText;

    [Header("Audio")]
    public AudioClip countdownSFX;
    public AudioSource audioSource;

    private int selectedLevel;

    void Start()
    {
        
        playerImage.sprite = GameData.agentSprite;
        playerText.text = GameData.agentName;
        selectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        levelText.text = "Level " + selectedLevel;
    }

    public void StartGame()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            // PLAY SOUND
            if (audioSource != null && countdownSFX != null)
            {
                audioSource.PlayOneShot(countdownSFX);
            }
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene("Level" + selectedLevel);
    }
}
