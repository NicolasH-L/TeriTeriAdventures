using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonkaiBeasts : MonoBehaviour
{
    private const float WalkSpeed = 1f;
    private const float RunSpeed = 3f;
    private const int MaxHealthPoint = 200;
    [SerializeField] private Transform _groundDetection;
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _isMovingLeft;
    private int _healthPoint;
    private float _movementSpeed;

    void Start()
    {
        _npcDirection = Vector2.left;
        _movementSpeed = WalkSpeed;
        _healthPoint = MaxHealthPoint;
        _isMovingLeft = true;
    }

    void Update()
    {
        _npcMovement = Vector2.left * _movementSpeed;
        transform.Translate(_npcMovement * Time.deltaTime);
        var groundInfo = Physics2D.Raycast(_groundDetection.position,
            Vector2.down, 0.2f);
        if (groundInfo.collider != false) return;
        ChangeDirection();
    }

    private void ChangeDirection()
    {
        if (_isMovingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isMovingLeft = false;
            _npcDirection = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isMovingLeft = true;
            _npcDirection = Vector2.left;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle":
            case "Plateform":
            case "Wall":
            case "Enemy":
                _npcDirection = -_npcDirection;
                // _direction = -_direction;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo taking damage


        if (_healthPoint <= 0)
            Destroy(transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _movementSpeed = WalkSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _movementSpeed = RunSpeed;
    }
}