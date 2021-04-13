using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGourd : MonoBehaviour, ItemInterface
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UseItem()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            print("sdfsdf");
        }
    }
}