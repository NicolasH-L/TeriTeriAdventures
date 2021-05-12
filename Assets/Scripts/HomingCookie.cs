using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingCookie : MonoBehaviour
{
    private const float Speed = 15f;
    private const float RotateSpeed = 200f;
    private GameObject _target;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2) _target.transform.position - _rigidbody2D.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        _rigidbody2D.angularVelocity = rotateAmount * RotateSpeed;
        _rigidbody2D.velocity = -transform.right * Speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Cookie"))
            return;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cookie") || other.gameObject.CompareTag("Boss"))
            return;
        Destroy(gameObject);
    }
}