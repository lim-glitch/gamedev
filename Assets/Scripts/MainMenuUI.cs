using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    public void PlaySingle()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void PlayMultiplayer()
    {
        GameResult.isMultiplayer = true;
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void OpenStoryline()
    {
        SceneManager.LoadScene("Storyline");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
