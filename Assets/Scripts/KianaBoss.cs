using System.Collections.Generic;
using UnityEngine;

public class KianaBoss : MonoBehaviour
{
    private const string JudahWeapon = "JudahWeapon";
    private const int StartingHealthPoint = 1500;
    private const float timeDamage = 2f;
    private const float timeBullet = 1.5f;
    private static readonly int IsBald = Animator.StringToHash("IsBald");
    [SerializeField] private Transform cookieBullets;
    [SerializeField] private List<GameObject> cookiePortals;
    private Animator _animator;
    private Random _random;
    private int _healthPoint;
    private bool _isAlive;

    public delegate void GameFinished(bool isDead);

    public event GameFinished OnGameEnded;

    private void Awake()
    {
        if (GameManager.GameManagerInstance == null)
            return;
        OnGameEnded += GameManager.GameManagerInstance.GameOver;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isAlive = true;
        _healthPoint = StartingHealthPoint;
        Invoke(nameof(SpawnBullets), 0f);
    }

    private void SpawnBullets()
    {
        if (!_isAlive)
            return;
        var index = Random.Range(0, cookiePortals.Count);
        var pos = new Vector2(cookiePortals[index].transform.position.x, cookiePortals[index].transform.position.y);
        Instantiate(cookieBullets, pos, new Quaternion());
        Invoke(nameof(SpawnBullets), timeBullet);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(JudahWeapon) || GameManager.GameManagerInstance == null)
            return;
        Debug.Log(other.gameObject.tag);
        TakeDamage(GameManager.GameManagerInstance.GetPlayerDamage());
    }

    private void TakeDamage(int damage)
    {
        if (_healthPoint - damage <= _healthPoint / 2)
            _animator.SetBool(IsBald, true);

        if (_healthPoint - damage <= 0)
        {
            _isAlive = false;
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<PolygonCollider2D>());
            Invoke(nameof(DelayDeath), timeDamage);
            return;
        }

        _healthPoint -= damage;
    }

    private void DelayDeath()
    {
        OnGameEnded?.Invoke(false);
        Destroy(gameObject);
    }
}