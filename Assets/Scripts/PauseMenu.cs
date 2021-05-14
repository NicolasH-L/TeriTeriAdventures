using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private const string MenuTag = "PauseMenu";
    [SerializeField] private GameObject pauseMenuUI;
    private bool _gameIsPaused;
    private Canvas _pauseMenu;

    private void Start()
    {
        _gameIsPaused = false;
        pauseMenuUI.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        _gameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}