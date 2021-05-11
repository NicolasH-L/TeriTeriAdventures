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
    private const int BaseInvincibilityDuration = 8;
    private const int IndexAudioSourceLevelBgm = 0;
    private const int IndexAudioSourceSpecialBgm = 1;
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private List<AudioClip> listLevelBgm;
    [SerializeField] private AudioClip invincibleBgm;
    private Canvas _canvas;
    private Camera _playerCamera;
    private int _invincibilityDuration;

    private List<AudioSource> _listAudioSources;

    // private AudioSource _audioSource;
    private GameObject _player;
    private GameObject _playerSpawnLocation;
    private bool _isEndReached;
    private bool _isInvincible;

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
        // _audioSource = GetComponent<AudioSource>();
        _listAudioSources = new List<AudioSource>();
        _listAudioSources.AddRange(GetComponents<AudioSource>());
        _invincibilityDuration = 0;
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
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        StopCoroutine(PlayAnotherAudioClip(listWelcomeBgm));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += GetPlayer;
        QueueSong(listLevelBgm);
    }

    public void QuitGame()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        Application.Quit();
    }

    private void QueueSong(List<AudioClip> musicList)
    {
        //TODO
        if (!_listAudioSources[IndexAudioSourceLevelBgm].isPlaying &&
            _listAudioSources[IndexAudioSourceSpecialBgm].isPlaying)
        {
            _listAudioSources[IndexAudioSourceLevelBgm].UnPause();
        }
        else
        {
            _listAudioSources[IndexAudioSourceLevelBgm].clip = musicList[Random.Range(0, musicList.Count)];
            _listAudioSources[IndexAudioSourceLevelBgm].Play();
            StartCoroutine(PlayAnotherAudioClip(musicList));
        }
    }

    private IEnumerator PlayAnotherAudioClip(List<AudioClip> musicList)
    {
        yield return new WaitForSeconds(_listAudioSources[IndexAudioSourceLevelBgm].clip.length);
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

    public void ChangeToSpecialBgm(int option)
    {
        switch (option)
        {
            case 1:
                _isInvincible = true;
                if (_listAudioSources[IndexAudioSourceLevelBgm].isPlaying &&
                    !_listAudioSources[IndexAudioSourceSpecialBgm].isPlaying)
                {
                    _listAudioSources[IndexAudioSourceLevelBgm].Pause();
                    _listAudioSources[IndexAudioSourceSpecialBgm].clip = invincibleBgm;
                }

                _invincibilityDuration = BaseInvincibilityDuration;
                break;
        }

        if (!_listAudioSources[IndexAudioSourceSpecialBgm].isPlaying)
            _listAudioSources[IndexAudioSourceSpecialBgm].Play();
        Invoke(nameof(CountdownChangeMusic), 1f);
        // StartCoroutine(QueueInvincibleBgm(_invincibilityDuration));
    }

    private IEnumerator QueueInvincibleBgm(int duration)
    {
        yield return new WaitForSeconds(duration);
        _listAudioSources[IndexAudioSourceSpecialBgm].Stop();
        QueueSong(listLevelBgm);
    }

    private void CountdownChangeMusic()
    {
        --_invincibilityDuration;
        if (_invincibilityDuration <= 0)
        {
            _listAudioSources[IndexAudioSourceSpecialBgm].Stop();
            QueueSong(listLevelBgm);
            return;
        }
        Invoke(nameof(CountdownChangeMusic), 1f);
    }
    
    private IEnumerator DelayEndReachedReset()
    {
        yield return new WaitForSeconds(5);
        _isEndReached = false;
    }
}