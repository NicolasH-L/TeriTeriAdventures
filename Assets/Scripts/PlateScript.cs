using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlateScript : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const int GravityScale = 5;
    private Rigidbody2D _rigidbody2D;
    private AudioSource _audioSource;
    private bool _isTouched;
    [SerializeField] private List<AudioClip> listAudioClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject == null || !other.gameObject.CompareTag(PlayerTag) || _isTouched)
            return;
        var index = Random.Range(0, listAudioClip.Count);
        _isTouched = true;
        _audioSource.clip = listAudioClip[index];
        _audioSource.Play();
        StartCoroutine(DelaySelfDestruct());
    }

    private IEnumerator DelaySelfDestruct()
    {
        _rigidbody2D.gravityScale = GravityScale;
        yield return new WaitForSeconds(_audioSource.clip.length);
        Destroy(gameObject);
    }
}