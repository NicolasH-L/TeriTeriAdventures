using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKianaBoss : MonoBehaviour
{
    [SerializeField] private KianaBoss kianaBoss;
    [SerializeField] private GameObject blockingWall;

    void Start()
    {
        blockingWall.SetActive(false);
        kianaBoss.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            kianaBoss.enabled = true;
            blockingWall.SetActive(true);
        }
    }
}