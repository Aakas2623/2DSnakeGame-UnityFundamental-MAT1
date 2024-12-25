using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextAoda;
    [SerializeField] private TextMeshProUGUI scoreTextManda;
    [SerializeField] private TextMeshProUGUI scoreTextWin;

    int scoreAoda = 0;
    int scoreManda = 0;

    private void Awake()
    {
        //scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //RefreshUI();
    }

    private void Update()
    {
        RefreshUI();
    }

    //compare()
    public int GetScore(SnakeID snakeID)
    {
        if (snakeID == SnakeID.AODA)
        {
            return scoreAoda; 
        }
        else
        {
            return scoreManda; 
        }
    }

    public void IncreaseScore(SnakeID snakeID, int increment)
    {
        //if stat here
        if (snakeID == SnakeID.AODA) 
        {
            scoreAoda += increment;
        }
        else 
        {
            scoreManda += increment;
        }
        
        RefreshUI();
    }

    public void DecreaseScore(SnakeID snakeID, int decrement)
    {
        if (snakeID == SnakeID.AODA)
        {
            scoreAoda -= decrement;
        }
        else
        {
            scoreManda -= decrement;
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        scoreTextAoda.text = "Aoda : " + scoreAoda;
        scoreTextManda.text = "Manda : " + scoreManda;
    }

    public void Win()
    {
        if (scoreAoda > scoreManda)
        {
            scoreTextWin.text = "Aoda Wins : ";
        }
        else if (scoreAoda < scoreManda)
        {
            scoreTextWin.text = "Manda Wins : ";
        }
        else
        {
            scoreTextWin.text = "Draw";
        }
    }
}

