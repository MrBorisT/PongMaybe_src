using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    private int PlayerScore = 0;
    private int EnemyScore = 0;
    private int difficulty = 0;

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddToPlayerScore()
    {
        if (PlayerScore < 99)
            PlayerScore++;
        UpdateScoreText();
        difficulty++;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
    public void AddToEnemyScore()
    {
        if (EnemyScore < 99)
            EnemyScore++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = GetScoreText();

    }

    private string GetScoreText()
    {
        return PlayerScore.ToString() + " : " + EnemyScore.ToString();
    }
}
