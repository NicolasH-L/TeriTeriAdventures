using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkaiBeasts : MonoBehaviour
{
    private const float SpeedMovement = 1f;
    private const int MaxHitPoint = 5;
    private const int axisAngle = 180;
    [SerializeField] private Transform lineOfSight;
    private Vector2 _npcMovement;
    private Transform playerCollider;
    private int _compteurHit;
    private bool _isMovingLeft;
    private int axisTemp;

    void Start()
    {
        _isMovingLeft = true;
        _npcMovement = Vector2.left * SpeedMovement;
    }

    void Update()
    {
        transform.Translate(_npcMovement * Time.deltaTime);
        RaycastHit2D playerDetected = Physics2D.Raycast(lineOfSight.position, Vector2.left, 1f);
        if (playerDetected != null)
        {
        }

        Debug.DrawLine(transform.position, Vector3.left, Color.magenta);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                break;

            case "Obstacle":
            case "Plateform":
            case "Wall":
            case "Enemy":
                _npcMovement = -_npcMovement;
                // _direction = -_direction;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Judah"))
        {
            _compteurHit++;
        }

        if (_compteurHit == MaxHitPoint)
        {
            Destroy(transform.gameObject);
        }
    }
}