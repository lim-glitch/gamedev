using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage the score
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score;
    public float timeTaken;
    public int stars;

    private float startTime;

    void Awake()
    {
        Instance = this;
    }

    //temporary - update func to test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore(10);
        }

    }
    void Start()
    {
        startTime = Time.time;
    }


    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void EndLevel()
    {
        timeTaken = Time.time - startTime;
        CalculateStars();
    }

    void CalculateStars()
    {
        if (score >= 600)
            stars = 3;

        else if (score >= 300)
            stars = 2;

        else
            stars = 1;
    }
}