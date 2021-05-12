using UnityEngine;

public class IslandNativeSavageScript : MonoBehaviour
{
    private const float SpeedMovement = 1f;
    private const int MaxHitPoint = 3;
    private const int axisAngle = 180;
    [SerializeField] private Transform _groundDetection;
    [SerializeField] private Transform _spawnBullet;
    private Vector2 _npcMovement;
    private int _compteurHit;
    private bool _isMovingLeft;
    private int axisTemp;
    private Bullet _bullet;

    void Start()
    {
        _isMovingLeft = true;
        _npcMovement = Vector2.left * SpeedMovement;
    }

    void Update()
    {
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

        RaycastHit2D playerDetected = Physics2D.Raycast(transform.position, Vector2.left, 3f);
        if (playerDetected.collider)
        {
            _bullet.FiringBullet(_spawnBullet);
        }
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
        if (other.gameObject.tag.Equals("Judah"))
            _compteurHit++;

        if (_compteurHit == MaxHitPoint)
            Destroy(transform.gameObject);
    }
}