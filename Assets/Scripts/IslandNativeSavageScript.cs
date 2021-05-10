using UnityEngine;

public class IslandNativeSavageScript : MonoBehaviour
{
    private const float SpeedMovement = 1f;
    private const int MaxHitPoint = 3;
    [SerializeField] private Transform _groundDetection;
    private Vector2 _npcMovement;
    private int _compteurHit;
    private bool _isMovingLeft = true;

    void Start()
    {
        _npcMovement = Vector2.left * SpeedMovement;
    }

    void Update()
    {
        transform.Translate(_npcMovement * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(_groundDetection.position, Vector2.down, 0.1f);
        if (groundInfo.collider == false)
        {
            if (_isMovingLeft)
            {
                transform.Translate(-_npcMovement * Time.deltaTime);
                _groundDetection.eulerAngles = new Vector3(0, 180, 0);
                _isMovingLeft = false;
            }
            else
            {
                transform.Translate(_npcMovement * Time.deltaTime);
                _groundDetection.eulerAngles = new Vector3(0, -180, 0);
                _isMovingLeft = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                print("Player touched enemy");
                break;

            case "Obstacle":
            case "Enemy":
                _npcMovement = -_npcMovement;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Judah"))
        {
            _compteurHit++;
        }

        if (_compteurHit == MaxHitPoint)
        {
            Destroy(transform.gameObject);
        }
    }
}