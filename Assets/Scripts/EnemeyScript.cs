using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor.UIElements;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemeyScript : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const float WalkSpeed = 1f;
    private const float RunSpeed = 2.5f;
    private const float ChargeAttackSpeed = 5f;
    private const float MeleeAttackDelay = 5f;
    private const float RangeAttackDelay = 1f;
    private const int MaxHealthPoint = 150;
    private const string AnimatorMoveLeftBoolean = "isMoveLeft";
    private const string AnimatorMeleeAttackBoolean = "isMeleeAttacking";
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform obstacleDetection;
    [SerializeField] private Transform obstacleDetection02;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private bool _hasRangedAttack;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private const string DefaultLayerMask = "Default";
    private const string PlayerLayerMask = "Player";
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _hasAttacked;
    private bool _isMovingLeft;
    private int _healthPoint;
    private float _movementSpeed;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _npcDirection = Vector2.left;
        _movementSpeed = WalkSpeed;
        _healthPoint = MaxHealthPoint;
        _isMovingLeft = true;
    }

    void Update()
    {
        _npcMovement = Vector2.left * _movementSpeed;
        transform.Translate(_npcMovement * Time.deltaTime);
        var groundInfo = Physics2D.Raycast(groundDetection.position,
            Vector2.down, 0.6f, 1 << LayerMask.NameToLayer(DefaultLayerMask));
        var obstacleInfo = Physics2D.Raycast(obstacleDetection.position, _npcDirection, 0f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));
        var obstacleInfo02 = Physics2D.Raycast(obstacleDetection02.position, _npcDirection, 1f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));

        Debug.DrawRay(groundDetection.position, Vector2.down, Color.magenta);
        Debug.DrawRay(obstacleDetection02.position, _npcDirection, Color.magenta);
        Debug.DrawRay(obstacleDetection.position, _npcDirection, Color.magenta);
        
        if(obstacleInfo.collider)
            Debug.Log("01 " + obstacleInfo.collider.name);
        if(obstacleInfo02.collider)
            Debug.Log("02 " + obstacleInfo02.collider.name);

        if (groundInfo.collider != false && obstacleInfo.collider == false && obstacleInfo02.collider == false) return;
        ChangeDirection();
    }


    private void ChangeDirection()
    {
        transform.Rotate(0, 180, 0);
        if (_isMovingLeft)
        {
            _isMovingLeft = false;
            _npcDirection = Vector2.right;
        }
        else
        {
            _isMovingLeft = true;
            _npcDirection = Vector2.left;
        }
    }

    private void Attack()
    {
        if (_hasAttacked)
            return;
        if (_hasRangedAttack)
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        else
        {
            // Debug.Log((_npcDirection * ChargeAttackSpeed).ToString());
            _rigidbody2D.velocity = _npcDirection * ChargeAttackSpeed;
        }

        _hasAttacked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        float waitSecond = _hasRangedAttack ? RangeAttackDelay : MeleeAttackDelay;
        yield return new WaitForSeconds(waitSecond);
        _hasAttacked = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _movementSpeed = WalkSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo taking damage


        if (_healthPoint <= 0)
            Destroy(transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
            _movementSpeed = WalkSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(PlayerTag) || _hasAttacked) return;
        _movementSpeed = RunSpeed;
        Attack();
    }
}