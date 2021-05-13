using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private bool _isDirectionLeft;

    void Start()
    {
        _isDirectionLeft = true;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_isDirectionLeft)
        {
            _rigidbody2D.velocity = new Vector2(-2, 0);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(2, 0);
        }
    }

    public void FiringBullet(Transform spawnBullet)
    {
        Instantiate(gameObject, spawnBullet.position, spawnBullet.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}