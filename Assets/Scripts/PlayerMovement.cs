using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private const string KeyMoveRight = "d";
    private const string KeyMoveLeft = "a";
    private const string KeyJump = "space";
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
    private const int TeriPlayerRightIndex = 1;
    private const int TeriPlayerLeftIndex = 0;
    [SerializeField] private Rigidbody2D playerRigidBody2D;
    [SerializeField] private GameObject judahCross;

    //TODO get rid of it
    [SerializeField] private List<Sprite> listSprite;
    private SpriteRenderer _spriteRenderer;

    private Animator _animatorPlayer;
    private PolygonCollider2D _polygonCollider2D;
    private AudioSource[] _audioSource;
    private HingeJoint2D _hingeJoint2D;
    private JointMotor2D _jointMotor2D;
    private Collider2D _judahCollider;
    private bool _hasAttacked;
    private bool _isPlayerFacingTheRight;
    private int _jumpCounter;
    private int _currentHealth;

    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        //TODO get rid of it
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _spriteRenderer.sprite =  listSprite[TeriPlayerLeftIndex];
        var movementPlayerX = Input.GetAxis("Horizontal") * Time.deltaTime * SpeedPlayer;

        if (movementPlayerX != 0)
        {
            transform.Translate(movementPlayerX, 0f, 0f);
            if (Input.GetKey(KeyMoveRight))
                _animatorPlayer.SetBool("isMovingToTheRight", true);
            else if (Input.GetKey(KeyMoveLeft))
                _animatorPlayer.SetBool("isMovingToTheLeft", true);
        }

        if (Input.GetKeyUp(KeyMoveRight))
        {
            _animatorPlayer.SetBool("isMovingToTheRight", false);
            _isPlayerFacingTheRight = true;
        }
        else if (Input.GetKeyUp(KeyMoveLeft))
        {
            _animatorPlayer.SetBool("isMovingToTheLeft", false);
            _isPlayerFacingTheRight = false;
        }

        if (Input.GetKeyDown(KeyJump) && _jumpCounter < MaxJump)
        {
            playerRigidBody2D.velocity = new Vector2(0f, JumpHeight);
            _jumpCounter++;
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
    }

    private void ChangeIdleSprite()
    {
        _spriteRenderer.sprite =  listSprite[TeriPlayerLeftIndex];
        print("hello");
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