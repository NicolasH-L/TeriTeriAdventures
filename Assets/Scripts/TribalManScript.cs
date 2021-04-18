using UnityEngine;

public class TribalManScript : MonoBehaviour
{
    private const float SpeedMovement = 1f;
    private const int MaxHitPoint = 3;
    private Vector2 _npcMovement;
    private int _compteurHit;

    void Start()
    {
        _npcMovement = Vector2.left * SpeedMovement;
    }

    void Update()
    {
        transform.Translate(_npcMovement * Time.deltaTime);
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