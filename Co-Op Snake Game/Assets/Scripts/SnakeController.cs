using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SnakeController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameOverController gameOverController;

    [SerializeField] SnakeID snakeID;

    private Vector2 direction = Vector2.right;

    private List<Transform> snakeTail;
    public Transform snakeTailPrefab;

    public BoxCollider2D SpawnArea;

    private float moveTimer;
    private float moveTimerMax;

    [SerializeField] private bool isShieldActive;

    [SerializeField] private bool isScoreMultiplierActive;

    [SerializeField] private bool isSpeedBoostActive;

    [SerializeField] private float powerupCooldownTimer;

    [SerializeField] private float SpeedMultiplier;

    [SerializeField] private int ScoreMultiplier;


    private void Start()
    {
        snakeTail = new List<Transform>();  
        snakeTail.Add(transform);
    }

    private void Update()
    {
        if (snakeID == SnakeID.AODA)
        {
            if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down) { direction = Vector2.up; }
            if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up) { direction = Vector2.down; }
            if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right) { direction = Vector2.left; }
            if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left) { direction = Vector2.right; }
        }
        if (snakeID == SnakeID.MANDA)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down) { direction = Vector2.up; }
            if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up) { direction = Vector2.down; }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right) { direction = Vector2.left; }
            if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left) { direction = Vector2.right; }
        }


    }

    private void FixedUpdate()
    {
        for (int i = snakeTail.Count - 1; i > 0; i--)
        {
            snakeTail[i].position = snakeTail[i - 1].position;
        }

        this.transform.position = new Vector3
        (
            Mathf.Round(this.transform.position.x + direction.x),
            Mathf.Round(this.transform.position.y + direction.y),
            0.0f
        );

        WrapSnakeInBounds();

    }

    protected void WrapSnakeInBounds()
    {
        Bounds bounds = SpawnArea.bounds;

        Vector3 snakeHeadPosition = this.transform.position;

        if (snakeHeadPosition.x > bounds.max.x)
        {
            snakeHeadPosition.x = bounds.min.x;
        }
        else if (snakeHeadPosition.x < bounds.min.x)
        {
            snakeHeadPosition.x = bounds.max.x;
        }

        if (snakeHeadPosition.y > bounds.max.y)
        {
            snakeHeadPosition.y = bounds.min.y;
        }
        else if (snakeHeadPosition.y < bounds.min.y)
        {
            snakeHeadPosition.y = bounds.max.y;
        }

        this.transform.position = snakeHeadPosition;
    }

    private void Grow()
    {
        Transform tail = Instantiate(this.snakeTailPrefab);
        tail.position = snakeTail[snakeTail.Count - 1].position; 

        snakeTail.Add(tail);

        //scoreController.IncreaseScore(10);
    }

    private void Shrink(int length)
    {
        if (snakeTail.Count > 1)
        {
            
                for (int i = 0; i < length; i++)
                {
                    Transform lastBodyPart = snakeTail[snakeTail.Count - 1];
                    snakeTail.RemoveAt(snakeTail.Count - 1);
                    Destroy(lastBodyPart.gameObject);
                }
            
        }

        //scoreController.DecreaseScore(-5);
    }

    private void SnakeDead()
    {
        for (int i = 1; i < snakeTail.Count; i++)
        {
            this.gameObject.SetActive(false);
        }
        Debug.Log("Dead");
        //snakeTail.Clear();
        //snakeTail.Add(this.transform);

        gameOverController.PlayerDied();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MassGainer")
        {
            Grow();
        }
        //else if (collision.tag == "MassBurner")
        //{
        //    Shrink(-1);
        //}
        else if (collision.tag == "Powerup")
        {
            HandleCollisionWithPowerup(collision);
        }
        else if (collision.tag == "Tail")
        {
            SnakeDead();
        }
    }

    public void PickUpFood()
    {
        Debug.Log("Food Ate");
        scoreController.IncreaseScore(10);
        //scoreController.DecreaseScore(-5);
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
        
        Debug.Log("Score Multiplier Activated");
        yield return new WaitForSeconds(powerupCooldownTimer);
        
        isScoreMultiplierActive = false;
        Debug.Log("Score Multiplier Dectivated");
    }

    private IEnumerator ActivateSpeedBoost()
    {
        isSpeedBoostActive = true;
        moveTimerMax /= SpeedMultiplier;
        
        Debug.Log("Speed Boost Activated");
        yield return new WaitForSeconds(powerupCooldownTimer);
        
        isSpeedBoostActive = false;
        moveTimerMax *= SpeedMultiplier;
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
}

