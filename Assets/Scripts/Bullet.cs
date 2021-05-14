using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string JudahWeaponTag = "JudahWeapon";
    private const float BulletSpeed = 5f;
    private const float BulletDestructionDelay = 2f;
    [SerializeField] private float bulletDamage;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidbody2D.velocity = -transform.right * BulletSpeed;
    }

  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag) || other.gameObject.CompareTag(JudahWeaponTag))
        {
            
            DestroyBullet();
            return;
        }

        Invoke(nameof(DestroyBullet), BulletDestructionDelay);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}