using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private Vector2Int moveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private void Awake()
    {
        gridPosition = new Vector2Int();
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        moveDirection = new Vector2Int(1, 0);
    }

    private void Update()
    {


        HandleInput();
        HandleMovement();


    }
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = +1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (moveDirection.y != +1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveDirection.x != +1)
            {
                moveDirection.x = -1;
                moveDirection.y = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = +1;
                moveDirection.y = 0;
            }
        }
    }

    private void HandleMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += moveDirection;
            gridMoveTimer -= gridMoveTimerMax;

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, AngleMovement(moveDirection) - 90);
        }
    }

    private float AngleMovement(Vector2Int direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}


