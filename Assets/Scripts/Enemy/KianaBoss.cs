using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class KianaBoss : MonoBehaviour
    {
        private const string JudahWeapon = "JudahWeapon";
        private const float SelfDestructDelay = 2f;
        private const float ResetHitDelay = 0.5f;
        private const float SpawnCookieDelay = 1.5f;
        private const int StartingHealthPoint = 2500;
        [SerializeField] private Transform cookieBullets;
        [SerializeField] private List<GameObject> cookiePortals;
        private AudioSource _audioSource;
        private Animator _animator;
        private Random _random;
        private int _healthPoint;
        private bool _isAlive;
        private bool _isBald;

        private bool _isHit;

        //Suggestion made by Rider
        private static readonly int IsBald = Animator.StringToHash("IsBald");

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
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _isAlive = true;
            _healthPoint = StartingHealthPoint;
            SpawnBullets();
        }

        private void SpawnBullets()
        {
            if (!_isAlive)
                return;
            var index = Random.Range(0, cookiePortals.Count);
            var cookieSpawnPosition = new Vector2(cookiePortals[index].transform.position.x,
                cookiePortals[index].transform.position.y);
            Instantiate(cookieBullets, cookieSpawnPosition, new Quaternion());
            Invoke(nameof(SpawnBullets), SpawnCookieDelay);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag(JudahWeapon) || GameManager.GameManagerInstance == null || _isHit)
                return;
            TakeDamage(GameManager.GameManagerInstance.GetPlayerDamage());
        }

        private void TakeDamage(int damage)
        {
            _isHit = true;
            if (!_isBald && _healthPoint - damage <= StartingHealthPoint / 2)
            {
                _animator.SetBool(IsBald, true);
                _isBald = true;
            }

            if (_healthPoint - damage <= 0)
            {
                _isAlive = false;
                Destroy(GetComponent<Rigidbody2D>());
                Destroy(GetComponent<Collider2D>());
                Destroy(GetComponent<PolygonCollider2D>());
                Destroy(_animator);
                Destroy(GetComponent<SpriteRenderer>());
                StartCoroutine(DelayDeath());
                return;
            }

            _audioSource.Play();
            _healthPoint -= damage;
            StartCoroutine(ResetIsHit());
        }

        private IEnumerator ResetIsHit()
        {
            yield return new WaitForSeconds(ResetHitDelay);
            _isHit = false;
        }

        private IEnumerator DelayDeath()
        {
            yield return new WaitForSeconds(SelfDestructDelay);
            OnGameEnded?.Invoke(false);
            Destroy(gameObject);
        }
    }
}