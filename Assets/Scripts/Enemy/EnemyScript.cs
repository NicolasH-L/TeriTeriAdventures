using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : MonoBehaviour
    {
        public delegate int GetPlayerDamage();

        public event GetPlayerDamage OnPlayerWeaponDamageLoaded;

        public delegate void DamagePlayer(int damage);

        public event DamagePlayer OnPlayerHit;

        private const string PlayerTag = "Player";
        private const string JudahWeaponTag = "JudahWeapon";
        private const string EnemyTag = "Enemy";
        private const string DefaultLayerMask = "Default";
        private const string PlayerLayerMask = "Player";
        private const float WalkSpeed = 1f;
        private const float RunSpeed = 2.5f;
        private const float ChargeAttackSpeed = 5f;
        private const float MeleeAttackDelay = 5f;
        private const float RangeAttackDelay = 1f;
        private const float CollisionAttackDelay = 1f;
        private const float GroundDetectionDistance = 0.6f;
        private const float ObstacleDetectionDistance = 0f;
        private const float ObstacleDetectionDistance2 = 1f;
        private const float PlayerDetectionDistance = 4f;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform obstacleDetection;
        [SerializeField] private Transform obstacleDetection02;
        [SerializeField] private Transform groundDetection;
        [SerializeField] private Transform playerDetection;
        [SerializeField] private bool hasRangedAttack;
        [SerializeField] private int healthPoint;
        [SerializeField] private int damagePoint;
        private AudioSource _audioSource;
        private Rigidbody2D _rigidbody2D;
        private List<Collider2D> _colliders;
        private Vector2 _npcMovement;
        private Vector2 _npcDirection;
        private bool _hasAttacked;
        private bool _isMovingLeft;
        private bool _isCollidedWithPlayer;
        private bool _isHit;
        private bool _isgameObjectNull;
        private float _movementSpeed;

        private void Start()
        {
            _isgameObjectNull = gameObject == null;
            if (GameManager.GameManagerInstance != null && PlayerScript.GetPlayerInstance != null)
            {
                OnPlayerWeaponDamageLoaded += GameManager.GameManagerInstance.GetPlayerDamage;
                OnPlayerHit += PlayerScript.GetPlayerInstance.TakeDamage;
            }
            _audioSource = GetComponent<AudioSource>();
            _colliders = new List<Collider2D>();
            _colliders.AddRange(GetComponents<Collider2D>());
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _npcDirection = Vector2.left;
            _movementSpeed = WalkSpeed;
            _isMovingLeft = true;
        }

        private void Update()
        {
            _npcMovement = Vector2.left * _movementSpeed;
            transform.Translate(_npcMovement * Time.deltaTime);
            var groundInfo = Physics2D.Raycast(groundDetection.position,
                Vector2.down, GroundDetectionDistance, 1 << LayerMask.NameToLayer(DefaultLayerMask));
            
            var obstacleInfo = Physics2D.Raycast(obstacleDetection.position, _npcDirection, ObstacleDetectionDistance,
                1 << LayerMask.NameToLayer(DefaultLayerMask));
            
            var obstacleInfo02 = Physics2D.Raycast(obstacleDetection02.position, _npcDirection, ObstacleDetectionDistance2,
                1 << LayerMask.NameToLayer(DefaultLayerMask));
            
            var playerInfo = Physics2D.Raycast(playerDetection.position, _npcDirection, PlayerDetectionDistance,
                1 << LayerMask.NameToLayer(PlayerLayerMask));

            if (groundInfo.collider != false && obstacleInfo.collider == false && obstacleInfo02.collider == false
                && playerInfo.collider == false) return;
            if (playerInfo)
            {
                Attack();
                return;
            }
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
            if (_hasAttacked || _isgameObjectNull)
                return;
            _hasAttacked = true;
            if (hasRangedAttack)
                Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            else
                _rigidbody2D.velocity = _npcDirection * ChargeAttackSpeed;

            _audioSource.Play();
            StartCoroutine(DelayAttack());
        }

        private IEnumerator DelayAttack()
        {
            var waitSecond = hasRangedAttack ? RangeAttackDelay : MeleeAttackDelay;
            yield return new WaitForSeconds(waitSecond);
            _hasAttacked = false;
        }

        private void TakeDamage(int damage)
        {
            if (gameObject == null || _isHit)
                return;
            _isHit = true;
            if (healthPoint - damage <= 0)
            {
                foreach (var enemyCollider in _colliders)
                {
                    Destroy(enemyCollider);
                }

                Destroy(GetComponent<Rigidbody2D>());
                Destroy(gameObject);
                return;
            }
            healthPoint -= damage;
            StartCoroutine(ResetIsHit());
        }

        private IEnumerator ResetIsHit()
        {
            yield return new WaitForSeconds(0.5f);
            _isHit = false;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            _movementSpeed = WalkSpeed;
            if (other.gameObject.CompareTag(EnemyTag))
                ChangeDirection();
            if (other.gameObject.CompareTag(PlayerTag) && !_isCollidedWithPlayer)
            {
                _isCollidedWithPlayer = true;
                OnPlayerHit?.Invoke(damagePoint);
                StartCoroutine(DelayCollisionDamage());
            }
        }

        private IEnumerator DelayCollisionDamage()
        {
            yield return new WaitForSeconds(CollisionAttackDelay);
            _isCollidedWithPlayer = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(JudahWeaponTag) && OnPlayerWeaponDamageLoaded != null)
                TakeDamage(OnPlayerWeaponDamageLoaded());
        }


        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(PlayerTag) || _hasAttacked) return;
            _movementSpeed = RunSpeed;
            Attack();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(PlayerTag))
                _movementSpeed = WalkSpeed;
        }
    }
}