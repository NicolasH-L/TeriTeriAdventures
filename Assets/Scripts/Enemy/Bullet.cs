using Player;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const string JudahWeaponTag = "JudahWeapon";
    private const float BulletSpeed = 5f;
    private const float BulletDestructionDelay = 2f;
    [SerializeField] private int bulletDamage;
    private Rigidbody2D _rigidbody2D;

    public delegate void DamagePlayer(int damage);

    public event DamagePlayer OnPlayerHit;

    void Start()
    {
        if (PlayerScript.GetPlayerInstance != null)
            OnPlayerHit += PlayerScript.GetPlayerInstance.TakeDamage;
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
            if (other.gameObject.CompareTag(PlayerTag))
                OnPlayerHit?.Invoke(bulletDamage);
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