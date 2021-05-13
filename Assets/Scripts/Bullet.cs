﻿using System;
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
    private const float BulletSpeed = 5f;
    private const float BulletDestructionDelay = 2f;

    void Start()
    {
        _isDirectionLeft = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody2D.velocity = -transform.right * BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(PlayerTag))
        {
            Invoke(nameof(DestroyBullet), BulletDestructionDelay);
        }
        else
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}