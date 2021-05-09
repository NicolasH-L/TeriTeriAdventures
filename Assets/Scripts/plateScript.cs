using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plateScript : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _rigidbody2D.gravityScale = 1;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}