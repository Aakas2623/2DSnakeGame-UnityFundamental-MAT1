using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private PowerupType powerupType;

    public PowerupType getPowerupType()
    {
        return powerupType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           gameObject.SetActive(false);
        }
    }
}
