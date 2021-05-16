using UnityEngine;

namespace Enemy
{
    public class HomingCookie : MonoBehaviour
    {
        private const string JudahWeaponTag = "JudahWeapon";
        private const string PlayerTag = "Player";
        private const string ChestSuperRare = "ChestSuperRare";
        private const string ChestRare = "ChestRare";
        private const string ChestCommon = "ChestCommon";
        private const string Cookie = "Cookie";
        private const string Boss = "Boss";
        private const float CookieDestructionDelay = 2f;
        private const float RotateSpeed = 200f;
        private const float Speed = 15f;
        private Rigidbody2D _rigidbody2D;
        private GameObject _target;

        private void Start()
        {
            _target = GameObject.FindGameObjectWithTag(PlayerTag);
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!GameObject.FindGameObjectWithTag(PlayerTag))
                return;
            var direction = (Vector2) _target.transform.position - _rigidbody2D.position;
            direction.Normalize();

            var right = transform.right;
            var rotateAmount = Vector3.Cross(direction, right).z;
            _rigidbody2D.angularVelocity = rotateAmount * RotateSpeed;
            _rigidbody2D.velocity = -right * Speed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(PlayerTag) || other.gameObject.CompareTag(JudahWeaponTag))
            {
                DestroyCookie();
                return;
            }

            Invoke(nameof(DestroyCookie), CookieDestructionDelay);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(Cookie) || other.gameObject.CompareTag(Boss) ||
                other.gameObject.CompareTag(ChestCommon) || other.gameObject.CompareTag(ChestRare) ||
                other.gameObject.CompareTag(ChestSuperRare))
                return;
            Destroy(gameObject);
        }

        private void DestroyCookie()
        {
            Destroy(gameObject);
        }
    }
}