using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeController : MonoBehaviour
    {
        private Vector2 dirction = Vector2.right;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) { dirction = Vector2.up; }
            else if (Input.GetKeyDown(KeyCode.S)) { dirction = Vector2.down; }
            else if (Input.GetKeyDown(KeyCode.A)) { dirction = Vector2.left; }
            else if (Input.GetKeyDown(KeyCode.D)) { dirction = Vector2.right; }
        }

        private void FixedUpdate()
        { 
            this.transform.position = new Vector3
            (
                Mathf.Round(this.transform.position.x + dirction.x),
                Mathf.Round(this.transform.position.y + dirction.y),
                0.0f
            );
            
        }

    }
}
