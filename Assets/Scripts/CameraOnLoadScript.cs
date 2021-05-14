using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraOnLoadScript : MonoBehaviour
{
    private const int FinalLevelScene = 3;
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
        if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene) return;
        Destroy(GetComponent<CinemachineBrain>());
        Destroy(gameObject);
        Destroy(this);
    }
}