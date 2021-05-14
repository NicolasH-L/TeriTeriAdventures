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
        // _child = gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        // if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene) return;
        // Debug.Log(gameObject.GetComponentInChildren<CinemachineVirtualCamera>().name);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene &&
            SceneManager.GetActiveScene().buildIndex > 0) return;
        Debug.Log("hel;lo");
        Destroy(GameObject.Find("MainCamera"));
        
        // Destroy(GetComponentInChildren<CinemachineVirtualCamera>());
        // Destroy(GetComponent<CinemachineBrain>());
        // StartCoroutine(Delay());
        
    }

    // private IEnumerator Delay()
    // {
    //     Debug.Log("waiting");
    //     yield return new WaitForSeconds(0.5f);
    //     Debug.Log("done");
    //     
    //     
    // }
    
}