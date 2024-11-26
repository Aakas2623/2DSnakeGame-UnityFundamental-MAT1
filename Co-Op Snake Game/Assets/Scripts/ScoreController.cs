using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    int score = 0;

    private void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        RefreshUI();
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        RefreshUI();
    }

    public void DecreaseScore(int decrement)
    {
        score -= decrement;
        RefreshUI();
    }

    private void RefreshUI()
    {
        scoreText.text = "Score : " + score;
    }
}

