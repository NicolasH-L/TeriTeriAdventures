using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float forceApplied = -1000f;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject judahCross;
    private AudioSource _audioSource;
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private bool _isPlayerGrounded;
    private HingeJoint2D _hingeJoint2D;
    private JointMotor2D _jointMotor2D;
    private Collider2D judahCollider;

    void Start()
    {
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _audioSource = GetComponent<AudioSource>();
        _jointMotor2D = _hingeJoint2D.motor;
        judahCollider = judahCross.GetComponent<Collider2D>();
        judahCollider.enabled = false;
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

        if (Input.GetKeyDown("j"))
        {
            _jointMotor2D.motorSpeed = forceApplied;
            _hingeJoint2D.motor = _jointMotor2D;
            judahCollider.enabled = true;
        }

        if (Input.GetKeyUp("j"))
        {
            _jointMotor2D.motorSpeed = 200;
            _hingeJoint2D.motor = _jointMotor2D;
            StartCoroutine(delay());
        }
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(1f);
        judahCollider.enabled = false;
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