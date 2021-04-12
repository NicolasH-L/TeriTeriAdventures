using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    [SerializeField] private Sprite chests;
    private SpriteRenderer _chestsRenderer;

    void Start()
    {
        _chestsRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _chestsRenderer.sprite = chests;
        }
    }
}