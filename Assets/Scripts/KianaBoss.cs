using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KianaBoss : MonoBehaviour
{
    [SerializeField] private Transform cookieBullets;
    [SerializeField] private List<GameObject> cookiePortals;
    private Transform _kianaBoss;
    private Random _random;

    // Start is called before the first frame update
    void Start()
    {
        _kianaBoss = GetComponent<Transform>();
        InvokeRepeating("SpawnBullets", 2f, 3f);
    }

    private void SpawnBullets()
    {
        var index = Random.Range(0, cookiePortals.Count);
        var pos = new Vector2(cookiePortals[index].transform.position.x, cookiePortals[index].transform.position.y);
        Instantiate(cookieBullets, pos, new Quaternion());
    }
}