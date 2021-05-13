using System;
using System.Collections;
using System.Diagnostics;
using UnityEditor.UIElements;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IslandNativeSavageScript : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const float WalkSpeed = 1f;
    private const float RunSpeed = 2.5f;
    private const float FireDelay = 2f;
    private const int MaxHealthPoint = 150;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform obstacleDetection;
    [SerializeField] private Transform obstacleDetection02;
    [SerializeField] private Transform groundDetection;
    private const string DefaultLayerMask = "Default";
    private const string PlayerLayerMask = "Player";
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _hasFired;
    private bool _isMovingLeft;
    private int _healthPoint;
    private float _movementSpeed;

    public delegate void ChangeDirectionBullet();

    public ChangeDirectionBullet OnDirectionChange;

    public delegate void ShootProjectile();

    public ShootProjectile OnEnemyDetected;

    void Start()
    {
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
            Vector2.down, 0.4f);
        var obstacleInfo = Physics2D.Raycast(obstacleDetection.position, _npcDirection, 0f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));
        var obstacleInfo02 = Physics2D.Raycast(obstacleDetection02.position, _npcDirection, 1f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));
        Debug.DrawRay(obstacleDetection02.position, _npcDirection, Color.magenta);
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
        if (_hasFired)
            return;

        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        _hasFired = true;
        StartCoroutine(DelayFiring(FireDelay));
    }

    private IEnumerator DelayFiring(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        _hasFired = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
            return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //todo taking damage


        if (_healthPoint <= 0)
            Destroy(transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            _movementSpeed = WalkSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _movementSpeed = RunSpeed;
            Attack();
        }
    }
}