using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        private Vector2 dirction = Vector2.right;

        private List<Transform> snakeTail;
        public Transform snakeTailPrefab;

        public BoxCollider2D SpawnArea;

        private void Start()
        {
            snakeTail = new List<Transform>();  
            snakeTail.Add(transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) { dirction = Vector2.up; }
            else if (Input.GetKeyDown(KeyCode.S)) { dirction = Vector2.down; }
            else if (Input.GetKeyDown(KeyCode.A)) { dirction = Vector2.left; }
            else if (Input.GetKeyDown(KeyCode.D)) { dirction = Vector2.right; }
        }

        private void FixedUpdate()
        { 
            for(int i = snakeTail.Count - 1; i > 0; i--)
            {
                snakeTail[i].position = snakeTail[i - 1].position;
            }

            this.transform.position = new Vector3
            (
                Mathf.Round(this.transform.position.x + dirction.x),
                Mathf.Round(this.transform.position.y + dirction.y),
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
        }

         private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "MassGainer")
            {
                Grow();
            }
        }
    }
}
