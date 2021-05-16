using System.Collections.Generic;
using UnityEngine;

public class SpawnChests : MonoBehaviour
{
    private const float RepeatRate = 5f;
    private const int MaxNbChest = 7;
    [SerializeField] private List<GameObject> chestList;
    private int _nbChest;
    private bool _isgameObjectNull;

    private void Start()
    {
        _isgameObjectNull = gameObject == null;
        if (gameObject == null)
            return;
        InvokeRepeating(nameof(ChestSpawning), 0f, RepeatRate);
    }

    private void ChestSpawning()
    {
        if (_nbChest >= MaxNbChest || _isgameObjectNull)
            return;

        _nbChest++;
        var index = Random.Range(0, chestList.Count);
        var randomChest = chestList[index];
        var spawnChestTransform = transform;
        Instantiate(randomChest, spawnChestTransform.position, spawnChestTransform.rotation);
    }
}