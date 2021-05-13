using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private const string EnemyTag = "Enemy";
    private const string PlayerTag = "Player";
    private Rigidbody2D _rigidbody2D;
    private bool _isDirectionLeft;
    private const float BulletSpeed = 3f;
    private const float BulletDestructionDelay = 2f;

    void Start()
    {
        _isDirectionLeft = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isDirectionLeft)
        {
            _rigidbody2D.velocity = new Vector2(-BulletSpeed, 0);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(BulletSpeed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            Invoke(nameof(DestroyBullet), BulletDestructionDelay);
        }
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void ChangeBulletDirection(bool isLeft)
    {
        _isDirectionLeft = isLeft;
    }
}