using System.Collections;
using UnityEngine;

public class PlateScript : MonoBehaviour
{
    private const string GroundTag = "Ground";
    private const string PlayerTag = "Player";
    private const float Delay = 1f;
    private const int GravityScale = 5;
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
            StartCoroutine(DelaySelfDestruct(Delay));

        if (other.gameObject.CompareTag(GroundTag))
            Destroy(gameObject);
    }

    private IEnumerator DelaySelfDestruct(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _rigidbody2D.gravityScale = GravityScale;
    }
}