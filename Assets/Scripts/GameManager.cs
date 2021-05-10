using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;
    public static GameManager GameManagerInstance => _gameManager;

    private const string PlayerTag = "Player";
    private const string PlayerSpawnLocationTag = "PlayerSpawn";
    private const string PlayerUiTag = "PlayerUI";
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private List<AudioClip> listLevelBgm;
    private Canvas _canvas;
    private Camera _playerCamera;
    private AudioSource _audioSource;
    private GameObject _player;
    private GameObject _playerSpawnLocation;
    private bool _isEndReached;

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
        if (_isEndReached)
            return;
        _isEndReached = true;
        DontDestroyOnLoad(_player);
        DontDestroyOnLoad(_playerSpawnLocation);
        DontDestroyOnLoad(_playerCamera);
        DontDestroyOnLoad(_canvas);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _player.transform.position = _playerSpawnLocation.transform.position;
        StartCoroutine(DelayEndReachedReset());
        OnLevelEndReached -= NextLevel;
    }

    private void GetPlayer(Scene scene, LoadSceneMode mode)
    {
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        _playerSpawnLocation = GameObject.FindGameObjectWithTag(PlayerSpawnLocationTag);
        _playerCamera = Camera.main;
        _canvas = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        // print(_player.tag);
    }


    private IEnumerator DelayEndReachedReset()
    {
        yield return new WaitForSeconds(5);
        _isEndReached = false;
    }
}