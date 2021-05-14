using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public delegate int GetPlayerDamage();

    public event GetPlayerDamage OnPlayerWeaponDamageLoaded;

    private const string PlayerTag = "Player";
    private const string JudahWeaponTag = "JudahWeapon";
    private const string EnemyTag = "Enemy";
    private const float WalkSpeed = 1f;
    private const float RunSpeed = 2.5f;
    private const float ChargeAttackSpeed = 5f;
    private const float MeleeAttackDelay = 5f;
    private const float RangeAttackDelay = 1f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform obstacleDetection;
    [SerializeField] private Transform obstacleDetection02;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private bool hasRangedAttack;
    [SerializeField] private int healthPoint;
    private Rigidbody2D _rigidbody2D;
    private const string DefaultLayerMask = "Default";
    private Vector2 _npcMovement;
    private Vector2 _npcDirection;
    private bool _hasAttacked;
    private bool _isMovingLeft;
    private float _movementSpeed;

    private void Start()
    {
        if (GameManager.GameManagerInstance != null)
            OnPlayerWeaponDamageLoaded += GameManager.GameManagerInstance.GetPlayerDamage;

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
            Vector2.down, 0.6f, 1 << LayerMask.NameToLayer(DefaultLayerMask));
        var obstacleInfo = Physics2D.Raycast(obstacleDetection.position, _npcDirection, 0f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));
        var obstacleInfo02 = Physics2D.Raycast(obstacleDetection02.position, _npcDirection, 1f,
            1 << LayerMask.NameToLayer(DefaultLayerMask));


        if (groundInfo.collider != false && obstacleInfo.collider == false
                                         && obstacleInfo02.collider == false ||
            IsRaycastCollidedWithSameTag(obstacleInfo) ||
            IsRaycastCollidedWithSameTag(obstacleInfo02)) return;
        ChangeDirection();
    }

    private bool IsRaycastCollidedWithSameTag(RaycastHit2D raycastHit2D)
    {
        if (!raycastHit2D.collider)
            return false;
        return raycastHit2D.collider.CompareTag(EnemyTag);
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
        if (hasRangedAttack)
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        else
        {
            _rigidbody2D.velocity = _npcDirection * ChargeAttackSpeed;
        }

        _hasAttacked = true;
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
        if (healthPoint - damage <= 0)
        {
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(transform.gameObject);
            return;
        }

        healthPoint -= damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _movementSpeed = WalkSpeed;
        if (other.gameObject.CompareTag(EnemyTag))
            ChangeDirection();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(JudahWeaponTag) && OnPlayerWeaponDamageLoaded != null)
        {
            TakeDamage(OnPlayerWeaponDamageLoaded());
        }
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