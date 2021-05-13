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
    [SerializeField] private Transform _spawnBullet;
    [SerializeField] private GameObject bullet;
    private Transform _groundDetection;
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _hasFired;
    private bool _isMovingLeft;
    private int _healthPoint;
    private float _movementSpeed;

    void Start()
    {
        _groundDetection = FindGameObjectInChildrenWithTag("GroundDetection");
        _npcDirection = Vector2.left;
        _movementSpeed = WalkSpeed;
        _healthPoint = MaxHealthPoint;
        _isMovingLeft = true;
    }

    void Update()
    {
        _npcMovement = _npcDirection * _movementSpeed;
        transform.Translate(_npcMovement* Time.deltaTime);
        var groundInfo = Physics2D.Raycast(_groundDetection.position,
            Vector2.down, 0f);

        if (groundInfo.collider != false) return;
        if (_isMovingLeft)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isMovingLeft = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isMovingLeft = true;
        }
    }

    private Transform FindGameObjectInChildrenWithTag(string targetTag)
    {
        var parentTransform = transform;
        foreach(Transform transformChild in parentTransform)
        {
            if (!transformChild.CompareTag(targetTag)) continue;
            Debug.Log("found it");
            return transformChild.GetComponent<Transform>();
        }
        return null;
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
            case "WoodStake":
            case "Wall":
                _npcDirection = -_npcDirection;
                // var temp = transform.rotation;
                // temp.z = -temp.z;
                // transform.rotation = temp;
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