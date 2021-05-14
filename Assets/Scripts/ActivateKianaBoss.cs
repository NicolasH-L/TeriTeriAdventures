using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKianaBoss : MonoBehaviour
{
    [SerializeField] private KianaBoss kianaBoss;
    [SerializeField] private GameObject blockingWall;
    private const float SelfDestructDelay = 1f;
    public delegate void BossFight();

    public event BossFight OnPlayerReachedBossLevel;

    private void Start()
    {
        if (GameManager.GameManagerInstance != null)
            OnPlayerReachedBossLevel += GameManager.GameManagerInstance.EnableBossFight;
        blockingWall.SetActive(false);
        kianaBoss.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player") && OnPlayerReachedBossLevel == null) return;
        OnPlayerReachedBossLevel?.Invoke();
        kianaBoss.enabled = true;
        blockingWall.SetActive(true);
        StartCoroutine(DelaySelfDestruction());
    }

    private IEnumerator DelaySelfDestruction()
    {
        yield return new WaitForSeconds(SelfDestructDelay);
        OnPlayerReachedBossLevel -= GameManager.GameManagerInstance.EnableBossFight;
        Destroy(gameObject);
    }
}