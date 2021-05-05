using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKianaBoss : MonoBehaviour
{
    [SerializeField] private KianaBoss kianaBoss;

    void Start()
    {
        kianaBoss.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            kianaBoss.enabled = true;
        }
    }
}