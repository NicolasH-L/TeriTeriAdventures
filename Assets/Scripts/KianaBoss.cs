using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KianaBoss : MonoBehaviour
{

    [SerializeField] private Transform cookieBullets;
    private Transform _kianaBoss;
    
    // Start is called before the first frame update
    void Start()
    {
        _kianaBoss = GetComponent<Transform>();
        InvokeRepeating("SpawnBullets", 2f, 3f);
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    private void SpawnBullets()
    {
        var pos = new Vector2(_kianaBoss.position.x - 5, _kianaBoss.position.y);
        Instantiate(cookieBullets, pos, _kianaBoss.rotation);
    }
}
