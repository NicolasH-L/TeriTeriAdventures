using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class IslandNativeSavageScript : MonoBehaviour
{
    private const float WalkSpeed = 1f;
    private const float RunSpeed = 2.5f;
    private const float FireDelay = 2f;
    private const int MaxHealthPoint = 150;
    private const int axisAngle = 180;
    [SerializeField] private Transform _groundDetection;
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject bullet;
    private Vector2 _npcMovement;
    private bool _hasFired;
    private bool _isMovingLeft;
    private int axisTemp;
    private int _healthPoint;
    private float _movementSpeed;

    void Start()
    {
        _movementSpeed = WalkSpeed;
        _healthPoint = MaxHealthPoint;
        _isMovingLeft = true;
    }

    void Update()
    {
        _npcMovement = Vector2.left * _movementSpeed;
        transform.Translate(_npcMovement * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(_groundDetection.position, Vector2.down, 0.5f);
        if (groundInfo.collider == false)
        {
            if (_isMovingLeft)
            {
                transform.eulerAngles = new Vector3(0, axisAngle, 0);
                _isMovingLeft = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -axisAngle, 0);
                _isMovingLeft = true;
            }
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
        switch (other.gameObject.tag)
        {
            case "Obstacle":
            case "Enemy":
                // _npcMovement = -_npcMovement;
                transform.eulerAngles = new Vector3(0, -axisTemp, 0);
                break;
        }
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
            Debug.Log(_movementSpeed);
            Attack();
        }
    }
}