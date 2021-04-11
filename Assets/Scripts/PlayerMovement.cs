using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private const float ForceAppliedAttacking = -1000f;
    private const float ForceAppliedRetracting = 1000f;
    private const float DelayTime = 0.4f;
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    [SerializeField] private GameObject judahCross;
    private AudioSource _audioSource;
    private HingeJoint2D _hingeJoint2D;
    private JointMotor2D _jointMotor2D;
    private Collider2D _judahCollider;
    private bool _isPlayerGrounded;

    void Start()
    {
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _audioSource = GetComponent<AudioSource>();
        _jointMotor2D = _hingeJoint2D.motor;
        _judahCollider = judahCross.GetComponent<Collider2D>();
        _judahCollider.enabled = false;
    }

    void Update()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;

        if (movementPlayerX != 0)
            transform.Translate(movementPlayerX, 0f, 0f);

        if (Input.GetKeyDown("space") && _isPlayerGrounded)
        {
            playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
            _audioSource.Play();
        }

        if (Input.GetKey("j"))
        {
            _jointMotor2D.motorSpeed = ForceAppliedAttacking;
            _hingeJoint2D.motor = _jointMotor2D;
            _judahCollider.enabled = true;
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        _jointMotor2D.motorSpeed = ForceAppliedRetracting;
        _hingeJoint2D.motor = _jointMotor2D;
        _judahCollider.enabled = false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Obstacle":
                _isPlayerGrounded = true;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Obstacle":
                _isPlayerGrounded = false;
                break;
        }
    }
}