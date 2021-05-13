using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    private const float FireDelay = 2f;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    private bool _hasFired;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Attack()
    {
        if (_hasFired)
            return;

        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        _hasFired = true;
        StartCoroutine(DelayFiring(FireDelay));
    }

    private IEnumerator DelayFiring(float waitSecond)
    {
        yield return new WaitForSeconds(waitSecond);
        _hasFired = false;
    }
}