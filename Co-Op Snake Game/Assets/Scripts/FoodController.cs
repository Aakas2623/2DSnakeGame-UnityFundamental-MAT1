using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodController : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private SnakeController snakeController;

    public BoxCollider2D foodArea;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.foodArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            RandomizePosition();
        }
    }
}

