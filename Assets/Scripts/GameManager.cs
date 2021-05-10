using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    public static GameManager GameManagerInstance
    {
        get { return _gameManager; }
    }

    private const string PlayerTag = "Player";
    private const string NextLevelTag = "NextLevel";
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private List<AudioClip> listLevelBgm;
    private AudioSource _audioSource;
    private GameObject _player;

    public delegate void LoadNextLevel();

    public event LoadNextLevel OnLevelEndReached;

    private void Awake()
    {
        if (_gameManager != null && _gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _gameManager = this;
        }
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        QueueSong(listWelcomeBgm);
    }

    void Update()
    {
        if (OnLevelEndReached != null)
        {
            OnLevelEndReached();
        }
    }

    public void StartGame()
    {
        _audioSource.Stop();
        StopCoroutine(PlayAnotherAudioClip(listWelcomeBgm));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        print(SceneManager.GetActiveScene().buildIndex);
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += GetPlayer;
        QueueSong(listLevelBgm);
    }

    public void QuitGame()
    {
        _audioSource.Stop();
        Application.Quit();
    }

    private void QueueSong(List<AudioClip> musicList)
    {
        _audioSource.clip = musicList[Random.Range(0, musicList.Count)];
        _audioSource.Play();
        StartCoroutine(PlayAnotherAudioClip(musicList));
    }

    private IEnumerator PlayAnotherAudioClip(List<AudioClip> musicList)
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        QueueSong(musicList);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        OnLevelEndReached -= NextLevel;
    }

    private void GetPlayer(Scene scene, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        print(_player.tag);
    }
}