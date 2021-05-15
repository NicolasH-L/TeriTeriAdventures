using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChests : MonoBehaviour
{
    private const float timeRepeat = 5f;
    private const int MaxNbChest = 7;
    [SerializeField] private List<GameObject> chestList;
    private int nbChest;
    private bool _isgameObjectNull;

    void Start()
    {
        _isgameObjectNull = gameObject == null;
        if (gameObject == null)
            return;
        InvokeRepeating("ChestSpawning", 0f, timeRepeat);
    }

    private void ChestSpawning()
    {
        if (nbChest >= MaxNbChest || _isgameObjectNull)
            return;

        nbChest++;
        var index = Random.Range(0, chestList.Count);
        var randomChest = chestList[index];
        var spawnChestTransform = transform;
        Instantiate(randomChest, spawnChestTransform.position, spawnChestTransform.rotation);
    }
}