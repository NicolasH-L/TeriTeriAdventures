﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private const string KeyMoveRight = "d";
    private const string KeyMoveLeft = "a";
    private const string KeyJump = "space";
    private const string BooleanDirectionRight = "isMovingToTheRight";
    private const string BooleanDirectionLeft = "isMovingToTheLeft";
    private const float SpeedPlayer = 7f;
    private const float JumpHeight = 8f;
    private const float ForceAppliedAttacking = -1000f;
    private const float ForceAppliedRetracting = 950f;
    private const float DelayTime = 0.4f;
    private const int MaxJump = 2;
    private const int SoundEffect1 = 0;
    private const int SoundEffect2 = 1;
    private const int SoundEffect3 = 2;
    private const int MaxHealth = 100;
    private const int Damage = 10;
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    [SerializeField] private GameObject judahCross;
    private Animator _animatorPlayer;
    private PolygonCollider2D _polygonCollider2D;
    private AudioSource[] _audioSource;
    private HingeJoint2D _hingeJoint2D;
    private JointMotor2D _jointMotor2D;
    private Collider2D _judahCollider;
    private bool _hasAttacked;
    private int _jumpCounter;
    private int _currentHealth;
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        _animatorPlayer = GetComponent<Animator>();
        _hingeJoint2D = GetComponent<HingeJoint2D>();
        _audioSource = GetComponents<AudioSource>();
        _judahCollider = judahCross.GetComponent<Collider2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _jointMotor2D = _hingeJoint2D.motor;
        _judahCollider.enabled = false;
        _jumpCounter = 0;
        _currentHealth = MaxHealth;
        healthBar.SetMaxLife(MaxHealth);
    }

    void Update()
    {
        // var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        // if (movementPlayerX != 0)
        // {
        //     print("entered");
        //     transform.Translate(movementPlayerX, 0f, 0f);
        //     if (Input.GetKey(KeyMoveRight))
        //     {
        //         SetMovingAnimationBooleans(true, false);
        //     }
        //     else if (Input.GetKey(KeyMoveLeft))
        //     {
        //         SetMovingAnimationBooleans(false, true);
        //     }
        // }
        //
        // if (Input.GetKeyUp(KeyMoveRight))
        // {
        //     SetIdleAnimationBooleans(BooleanDirectionRight, true);
        // }
        // else if (Input.GetKeyUp(KeyMoveLeft))
        // {
        //     SetIdleAnimationBooleans(BooleanDirectionLeft, false);
        // }
        //
        // if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        // {
        //     playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
        //     _jumpCounter++;
        //     _audioSource[SoundEffect1].Play();
        // }
        if (Input.GetKeyUp(KeyMoveRight))
        {
            SetIdleAnimationBooleans(BooleanDirectionRight, true);
        }
        else if (Input.GetKeyUp(KeyMoveLeft))
        {
            SetIdleAnimationBooleans(BooleanDirectionLeft, false);
        }
        
        if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        {
            playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
            _jumpCounter++;
            print(_jumpCounter.ToString());
            _audioSource[SoundEffect1].Play();
        }

        //TODO : Fix attacking
        if (Input.GetKey("j") && !_hasAttacked)
        {
            _audioSource[SoundEffect2].Play();
            _jointMotor2D.motorSpeed = ForceAppliedAttacking;
            _hingeJoint2D.motor = _jointMotor2D;
            _judahCollider.enabled = true;
            _hasAttacked = true;
            StartCoroutine(Delay());
        }
    }

    private void FixedUpdate()
    {
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;
        if (movementPlayerX != 0)
        {
            print("entered");
            transform.Translate(movementPlayerX, 0f, 0f);
            if (Input.GetKey(KeyMoveRight))
            {
                SetMovingAnimationBooleans(true, false);
            }
            else if (Input.GetKey(KeyMoveLeft))
            {
                SetMovingAnimationBooleans(false, true);
            }
        }

        

        
    }

    private void SetMovingAnimationBooleans(bool isMoveRight, bool isMoveLeft)
    {
        _animatorPlayer.SetBool("isMovingToTheRight", isMoveRight);
        _animatorPlayer.SetBool("isMovingToTheLeft", isMoveLeft);
        _animatorPlayer.SetBool("isIdle", false);
    }

    private void SetIdleAnimationBooleans(string booleanDirection, bool isIdleRight)
    {
        _animatorPlayer.SetBool(booleanDirection, false);
        _animatorPlayer.SetBool("isIdleRight", isIdleRight);
        _animatorPlayer.SetBool("isIdle", true);
    }
    private void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        healthBar.SetHealth(_currentHealth);
    }

    //TODO : Fix the coroutine when the player is attacking
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(DelayTime);
        _jointMotor2D.motorSpeed = ForceAppliedRetracting;
        _hingeJoint2D.motor = _jointMotor2D;
        _judahCollider.enabled = false;
        _hasAttacked = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Ground":
            case "Obstacle":
                _jumpCounter = 0;
                break;

            case "Enemy":
                // _audioSource[SoundEffect3].Play();
                TakeDamage(Damage);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Plateform":
            case "Obstacle":
                break;
        }
    }

    //TODO : Callback
    public void ResetJump()
    {
        _jumpCounter = 0;
    }
}