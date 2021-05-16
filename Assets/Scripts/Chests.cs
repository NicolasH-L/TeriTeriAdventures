using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    private const string PlayerTag = "Player";
    private const float OffSetY = 1.5f;
    private const float OffSetX = 0.1f;
    [SerializeField] private List<GameObject> listItems;
    [SerializeField] private Sprite chests;
    private SpriteRenderer _chestsRenderer;
    private GameObject _itemToSpawn;

    private void Start()
    {
        _chestsRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(PlayerTag)) return;
        _chestsRenderer.sprite = chests;
        var spawnItemPosition = transform.position;
        spawnItemPosition.y += OffSetY;
        spawnItemPosition.x -= OffSetX;
        var index = Random.Range(0, listItems.Count);
        var randomItems = listItems[index];
        Instantiate(randomItems, spawnItemPosition, transform.rotation);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}