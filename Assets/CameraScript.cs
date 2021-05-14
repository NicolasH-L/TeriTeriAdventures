using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    private const int FinalLevelScene = 3;
    // Start is called before the first frame update
    private CinemachineVirtualCamera _child;

    private void Start()
    {
        _child = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        
        // Debug.Log(gameObject.GetComponentInChildren<CinemachineVirtualCamera>().name);
    }

    private void OnEnable()
    {
        Debug.Log("hel;lo");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene) return;
        
        Destroy(_child);
        Destroy(gameObject.GetComponent<CinemachineBrain>());
        // Destroy(gameObject);
    }
}