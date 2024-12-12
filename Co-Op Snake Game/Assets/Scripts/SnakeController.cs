using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameOverController gameOverController;

    [SerializeField] private SnakeID snakeID;

    private Vector2 direction = Vector2.right;

    private Vector3 spawnPosition;

    private List<Transform> snakeTail;
    public Transform snakeTailPrefab;

    public BoxCollider2D SpawnArea;

    private float moveTimer;
    private float moveTimerMax;

    [SerializeField] private bool isShieldActive;

    [SerializeField] private bool isScoreMultiplierActive;

    [SerializeField] private bool isSpeedBoostActive;

    [SerializeField] private float powerupCooldownTimer;

    [SerializeField] private int SpeedMultiplier;

    [SerializeField] private int ScoreMultiplier = 1;

    //[SerializeField] private GameOverController gameOverObject;

    //[SerializeField] private PauseController gamePauseObject;

    //[SerializeField] private GameManager gameManagerObject;

    //[SerializeField] private GameObject gameOverPanel;

    public int moveSpeed = 10;

    bool isWrapping;
    Vector3 prevPos;


    private void Start()
    {
        snakeTail = new List<Transform>();  
        snakeTail.Add(transform);
        spawnPosition = new Vector3(SpawnArea.bounds.max.x + 1, SpawnArea.bounds.max.y + 1, 0f);
    }

    private void Update()
    {
        if (snakeID == SnakeID.AODA)
        {
            if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down) { direction = Vector2.up; return; }
            else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up) { direction = Vector2.down; return; }
            else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right) { direction = Vector2.left; return; }
            else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left) { direction = Vector2.right; return; }
        }
        if (snakeID == SnakeID.MANDA)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down) { direction = Vector2.up; return; }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up) { direction = Vector2.down; return; }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right) { direction = Vector2.left; return; }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left) { direction = Vector2.right; return; }
        }
    }

    //private void FixedUpdate()
    //{
    //    for (int i = snakeTail.Count - 1; i > 0; i--)
    //    {
    //        snakeTail[i].position = snakeTail[i - 1].position;
    //    }

    //    Vector3 previous = transform.position;


    //    this.transform.position = new Vector3
    //    (
    //        Mathf.Round(this.transform.position.x + direction.x),
    //        Mathf.Round(this.transform.position.y + direction.y),
    //        0.0f
    //    );

    //    WrapSnakeInBounds();
    //}

    private void FixedUpdate()
    {
        WrapSnakeInBounds();

        for (int i = snakeTail.Count - 1; i > 0; i--)
        {
            if (i == 1)
            {
                snakeTail[i].position = prevPos;
            }
            else
            {
                snakeTail[i].position = snakeTail[i - 1].position;
            }
        }

        transform.position = new Vector3
        (
            Mathf.Round(this.transform.position.x + direction.x * moveSpeed),
            Mathf.Round(this.transform.position.y + direction.y * moveSpeed),
            0.0f
        );
    }

    private void LateUpdate()
    {
        prevPos = transform.position;
    }

    protected void WrapSnakeInBounds()
    {
        Bounds bounds = SpawnArea.bounds;

        Vector3 snakeHeadPosition = this.transform.position;

        if (snakeHeadPosition.x > bounds.max.x)
        {
            snakeHeadPosition.x = bounds.min.x;
            isWrapping = true;
        }
        else if (snakeHeadPosition.x < bounds.min.x)
        {
            snakeHeadPosition.x = bounds.max.x;
            isWrapping = true;
        }

        if (snakeHeadPosition.y > bounds.max.y)
        {
            snakeHeadPosition.y = bounds.min.y;
            isWrapping = true;
        }
        else if (snakeHeadPosition.y < bounds.min.y)
        {
            snakeHeadPosition.y = bounds.max.y;
            isWrapping = true;
        }

        this.transform.position = snakeHeadPosition;
        StartCoroutine(ResetWrappingStatus());
    }



    private void Grow()
    {
        Transform tail = Instantiate(this.snakeTailPrefab);
        tail.position = snakeTail[snakeTail.Count - 1].position; 

        snakeTail.Add(tail);
        isWrapping = true;
        StartCoroutine (ResetWrappingStatus(0.5f));
        //Debug.Log("")
    }

    private void Shrink()
    {
        
        if (snakeTail.Count > 1)
        {
            Transform lastSegment = snakeTail[snakeTail.Count - 1];
            snakeTail.RemoveAt(snakeTail.Count - 1);
            Destroy(lastSegment.gameObject);
        }
    }

    private void SnakeDead()
    {
        
        for (int i = 1; i < snakeTail.Count; i++)
        {
            this.gameObject.SetActive(false);
        }
        Debug.Log("Dead");

        gameOverController.PlayerDied();
    }

    private IEnumerator ResetWrappingStatus(float duration = 1)
    {
        yield return new WaitForSeconds(duration); // Adjust duration as needed
        isWrapping = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MassGainer")
        {
            Grow();
            MassGainer();
        }
        else if (collision.tag == "MassBurner")
        {
            Shrink();
            MassBurner();
        }
        else if (collision.tag == "Powerup")
        {
            HandleCollisionWithPowerup(collision);
        }
        else if (collision.tag == "Tail" && isWrapping == false && isShieldActive == false)
        {
            SnakeDead();
            scoreController.Win();
            Debug.Log(collision.gameObject.name);
        }
        else if (collision.tag == "Player" && isShieldActive == false)
        {
            SnakeDead();
            scoreController.Win();
            Debug.Log(collision.gameObject.name);
        }

    }

    public void MassGainer()
    {
        Debug.Log("Gainer");
        scoreController.IncreaseScore(snakeID, 10 * ScoreMultiplier);
    }

    public void MassBurner()
    {
        Debug.Log("Drainer");
        scoreController.DecreaseScore(snakeID, 5 * ScoreMultiplier);
    }

    private void HandleCollisionWithPowerup(Collider2D colliderPowerupObject)
    {
        if (colliderPowerupObject.gameObject.GetComponent<PowerupController>() != null)
        {
            switch (colliderPowerupObject.gameObject.GetComponent<PowerupController>().getPowerupType())
            {
                case PowerupType.ScoreMultiplierPowerup:
                    StartCoroutine(ActivateScoreMultiplier());
                    break;
                case PowerupType.ShieldPowerup:
                    StartCoroutine(ActivateShield());
                    break;
                case PowerupType.SpeedBoostPowerup:
                    StartCoroutine(ActivateSpeedBoost());
                    break;
            }
        }
    }

    private IEnumerator ActivateShield()
    {
        isShieldActive = true;
        Debug.Log("ShieldActive");
        yield return new WaitForSeconds(powerupCooldownTimer);
        isShieldActive = false;
        Debug.Log("ShieldDeactive");
    }

    private IEnumerator ActivateScoreMultiplier()
    {
        isScoreMultiplierActive = true;
        ScoreMultiplier = 2;
        
        Debug.Log("Score Multiplier Activated");
        yield return new WaitForSeconds(powerupCooldownTimer);
        
        isScoreMultiplierActive = false;
        ScoreMultiplier = 1;    
        Debug.Log("Score Multiplier Dectivated");
    }

    private IEnumerator ActivateSpeedBoost()
    {
        isSpeedBoostActive = true;
        moveSpeed *= SpeedMultiplier;
        
        Debug.Log("Speed Boost Activated");
        yield return new WaitForSeconds(powerupCooldownTimer);
        
        isSpeedBoostActive = false;
        moveSpeed = 1;
        Debug.Log("Speed Boost Dectivated");
    }

    public bool checkShieldStatus()
    {
        return isShieldActive;
    }

    public bool checkScoreMultiplierStatus()
    {
        return isScoreMultiplierActive;
    }

    public bool checkSpeedBoostStatus()
    {
        return isSpeedBoostActive;
    }

    //private void OnPlayerDeath(SnakeID snakeID, bool headToHeadCollision)
    //{
       
    //    gameOverPanel.SetActive(true);
    //    scoreController.SetWinnerScore(snakeID, headToHeadCollision);
    //    Time.timeScale = 0f;
    //    gameManagerObject.KillAllPlayers();
    //}
}

