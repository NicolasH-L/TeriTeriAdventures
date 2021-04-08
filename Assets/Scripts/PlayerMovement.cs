using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private const float SpeedPlayer = 7f;
    private const float SpeedPlayerJump = 2f;
    private bool _isPlayerGrounded;
    private Vector2 _jumping;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _jumping = new Vector2(0.0f, 2f);
        _isPlayerGrounded = true;
    }

    void Update()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;

        if (movementPlayerX != 0)
            transform.Translate(movementPlayerX, 0f, 0f);

        if (Input.GetKey("space") && _isPlayerGrounded)
        {
            _rigidbody2D.AddForce(_jumping * SpeedPlayerJump, ForceMode2D.Impulse);
            _audioSource.Play();
            _isPlayerGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plateform"))
        {
            _isPlayerGrounded = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
        }
    }
}