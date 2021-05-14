using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraOnLoadScript : MonoBehaviour
{
    private const int FinalLevelScene = 3;
    private const string VcamTag = "Vcam";
    private CinemachineBrain _cinemachineBrain;
    private GameObject _vcam;
    private void Start()
    {
        if (GetComponent<CinemachineBrain>() == null || GameObject.FindGameObjectWithTag(VcamTag))
            return;
        _cinemachineBrain = GetComponent<CinemachineBrain>();
        _vcam = GameObject.FindGameObjectWithTag(VcamTag);
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
        if (SceneManager.GetActiveScene().buildIndex <= FinalLevelScene) return;
        _cinemachineBrain.gameObject.SetActive(false);
        _vcam.SetActive(false);
        Destroy(_cinemachineBrain);
        Destroy(_vcam);
        Destroy(gameObject);
        Destroy(this);
    }
}