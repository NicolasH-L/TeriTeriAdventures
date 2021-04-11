using UnityEngine;

public class TribalManScript : MonoBehaviour
{
    private const float SpeedMovement = 0.4f;
    private Vector2 _npcMovement;

    void Start()
    {
    }

    void Update()
    {
        _npcMovement = Vector2.left * (SpeedMovement * Time.deltaTime);
        transform.Translate(_npcMovement);
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
        switch (other.gameObject.tag)
        {
            case "Judah":
                Destroy(transform.gameObject);
                break;
        }
    }
}