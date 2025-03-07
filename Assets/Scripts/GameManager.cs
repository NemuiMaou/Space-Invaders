using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;
    public Invaders invaders;

    void Start()
    {
        highScore = PlayerPrefs.GetInt(("HighScore"), 0);
        UpdateScores();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetScore();
            invaders.ResetInvaders();
        }
    }

    public void ScoreIncrease(int points)
    {
        score += points;
        highScore = Mathf.Max(highScore, score);

        if (highScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
            
        UpdateScores();
    }

    void UpdateScores()
    {
        String scoreFormat = score.ToString("0000");
        String hScoreFormat = highScore.ToString("0000");
        
        Score.text = $"Current Score <{scoreFormat}>";
        HighScore.text = $"High Score <{hScoreFormat}>";
        
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScores();
    }
}