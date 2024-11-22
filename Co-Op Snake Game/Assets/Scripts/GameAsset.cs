using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAsset : MonoBehaviour
{
    public static GameAsset instance;

    public Sprite snakeHeadSprite;

    private void Awake()
    {
        instance = this;

    }


}
