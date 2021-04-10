using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private bool _isPlayerGrounded;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;

        if (movementPlayerX != 0)
            transform.Translate(movementPlayerX, 0f, 0f);

        if (Input.GetKeyDown("space") && _isPlayerGrounded)
        {
            _rigidbody2D.velocity = new Vector2(0f, JumpHeight);
            _audioSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plateform"))
        {
            _isPlayerGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Plateform"))
        {
            _isPlayerGrounded = false;
        }
    }
}