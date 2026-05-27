using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [Header("Agent UI")]
    public Image agentSprite;
    public Text agentName;
    public Text agentType;

    [Header("Game UI")]
    public Text scoreText;
    public Text timeText;
    public Text objectiveText;

    [Header("Timer")]
    public float levelTime = 120f;
    private float currentTime;

    void Start()
    {
        // SET AGENT INFO
        agentSprite.sprite = GameData.agentSprite;
        agentName.text = GameData.agentName;
        agentType.text = "Power: " + GameData.agentType;

        // SET OBJECTIVE BASED ON LEVEL
        int level = PlayerPrefs.GetInt("SelectedLevel", 1);
        objectiveText.text = LevelObjective.GetObjective(level);

        currentTime = levelTime;
    }

    void Update()
    {
        // SCORE (OK to update if small project)
        scoreText.text = "Score: " + GameManager.Instance.score;

        // TIMER (countdown)
        currentTime -= Time.deltaTime;
        timeText.text = "Time: " + Mathf.Ceil(currentTime);

        if (currentTime <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }
}