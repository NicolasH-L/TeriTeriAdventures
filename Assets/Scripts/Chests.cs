using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chests : MonoBehaviour
{
    private const float OffSetY = 1.5f;
    private const float OffSetX = 0.1f;
    [SerializeField] private Sprite chests;
    [SerializeField] private List<GameObject> _listItems;
    [SerializeField] private int pinkGourdDropRate;
    [SerializeField] private int greenGourdDropRate;
    [SerializeField] private int invincibleGourdDropRate;
    [SerializeField] private int bandAidDropRate;
    [SerializeField] private int teriTicketDropRate;
    [SerializeField] private int potionDropRate;
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
            var index = Random.Range(0, _listItems.Count);
            var spawnItemPosition = transform.position;
            spawnItemPosition.y += OffSetY;
            spawnItemPosition.x -= OffSetX;
            var randomItems = _listItems[index];
            Instantiate(randomItems, spawnItemPosition, transform.rotation);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}