using System.Collections.Generic;
using UnityEngine;

public class Chests : MonoBehaviour
{
    private const float OffSetY = 1.5f;
    private const float OffSetX = 0.1f;
    [SerializeField] private Sprite chests;
    [SerializeField] private List<GameObject> _listItems;
    private SpriteRenderer _chestsRenderer;
    private GameObject _itemToSpawn;

    void Start()
    {
        _chestsRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _chestsRenderer.sprite = chests;
        var spawnItemPosition = transform.position;
        spawnItemPosition.y += OffSetY;
        spawnItemPosition.x -= OffSetX;
        var index = Random.Range(0, _listItems.Count);
        var randomItems = _listItems[index];
        Instantiate(randomItems, spawnItemPosition, transform.rotation);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}