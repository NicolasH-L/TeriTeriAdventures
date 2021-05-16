using UnityEngine;

public class ItemScript : MonoBehaviour
{
    private const string PlayerTag = "Player";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
            Destroy(gameObject);
    }
}