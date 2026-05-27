using UnityEngine;
using UnityEngine.SceneManagement;

public class StorylineManager : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;

     void Start()
    {
        ShowPage(currentPage);
    }

    void ShowPage(int index)
    {
        //hide all pages
        for(int i =0; i< pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        //show current pages
        pages[index].SetActive(true);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
