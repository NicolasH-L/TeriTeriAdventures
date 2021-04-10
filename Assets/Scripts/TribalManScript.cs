using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalManScript : MonoBehaviour
{
    private const float SpeedMovement = 0.4f;
    private Vector2 _npcMovement;

    void Start()
    {
        _npcMovement = Vector2.left * SpeedMovement * Time.deltaTime;
    }

    void Update()
    {
        transform.Translate(_npcMovement);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Destroy(transform.gameObject);
                break;

            case "Obstacle":
                _npcMovement = -_npcMovement;
                break;
        }
    }
}