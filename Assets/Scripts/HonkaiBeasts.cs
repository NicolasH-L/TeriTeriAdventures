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
    private int _direction;
    private Transform playerCollider;
    private int _compteurHit;
    private bool _isMovingLeft;
    private int axisTemp;

    void Start()
    {
        _isMovingLeft = true;
        _npcMovement = Vector2.left * SpeedMovement;
        _direction = -axisAngle;
    }

    void Update()
    {
        transform.Translate(_npcMovement * Time.deltaTime);
        // transform.eulerAngles = new Vector3(0, _direction, 0);
        RaycastHit2D playerDetected = Physics2D.Raycast(lineOfSight.position, Vector2.left, 1f);
        if (playerDetected != null)
        {
            print("touched player");
            Debug.DrawLine(lineOfSight.position, playerDetected.point, Color.magenta);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                break;

            case "Obstacle":
            case "Plateform":
            case "Enemy":
                // _npcMovement = -_npcMovement;
                _direction = -_direction;
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