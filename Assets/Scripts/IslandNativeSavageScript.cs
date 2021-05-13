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
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform _obstacleDetection;
    [SerializeField] private Transform _obstacleDetection02;
    [SerializeField]private Transform _groundDetection;
    private const int DefaultLayerMask = 0;
    private const int PlayerLayerMask = 9;
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _hasFired;
    private bool _isMovingLeft;
    private int _healthPoint;
    private float _movementSpeed;

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
        var groundInfo = Physics2D.Raycast(_groundDetection.position,
            Vector2.down, 0.4f);
        var obstacleInfo = Physics2D.Raycast(_obstacleDetection.position, _npcDirection, 0f, 1 << LayerMask.NameToLayer("Default"));
        var obstacleInfo02 = Physics2D.Raycast(_obstacleDetection02.position, _npcDirection, 1f, 1 << LayerMask.NameToLayer("Default"));
        Debug.DrawRay(_obstacleDetection02.position, _npcDirection,Color.magenta);
        if (groundInfo.collider != false && obstacleInfo.collider == false && obstacleInfo02.collider == false) return;
        ChangeDirection();
    }
    

    private void ChangeDirection()
    {
        if (_isMovingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isMovingLeft = false;
            _npcDirection = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isMovingLeft = true;
            _npcDirection = Vector2.left;
        }
    }

    private void Attack()
    {
        if (_hasFired)
            return;

        Instantiate(bullet, _spawnBullet.position, _spawnBullet.rotation);
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