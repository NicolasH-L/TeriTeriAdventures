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

    private const string PlayerSpawnLocationTag = "PlayerSpawn";
    private const string PlayerUiTag = "PlayerUI";
    private const string DialogueManage = "DialogueManager";
    private const int BaseInvincibilityDuration = 8;
    private const int IndexAudioSourceLevelBgm = 0;
    private const int IndexAudioSourceSpecialBgm = 1;
    private const int GameEndSceneIndex = 4;
    private const int GameOverSceneIndex = 5;
    private const int FinalLevelScene = 3;
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private List<AudioClip> listLevelBgm;
    [SerializeField] private AudioClip invincibleBgm;
    [SerializeField] private AudioClip bossMusic;
    private Canvas _canvas;
    private Camera _playerCamera;
    private GameObject _dialogueManager;
    private int _invincibilityDuration;
    private int _currentLevel;
    private bool _isBossFight;
    private List<AudioSource> _listAudioSources;

    // private AudioSource _audioSource;
    private PlayerScript _player;
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
        // StopCoroutine(PlayAnotherAudioClip(listWelcomeBgm));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += GetPlayer;
        ++_currentLevel;
        QueueSong(listLevelBgm);
    }

    public void QuitGame()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        Application.Quit();
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void QueueSong(List<AudioClip> musicList)
    {
        if (musicList == null)
            return;
        AudioClip clip;
        if (!_isBossFight)
        {
            clip = _currentLevel > 0 ? musicList[_currentLevel - 1] : musicList[Random.Range(0, musicList.Count)];
        }
        else
        {
            clip = bossMusic;
        }
        _listAudioSources[IndexAudioSourceLevelBgm].clip = clip;
        _listAudioSources[IndexAudioSourceLevelBgm].Play();
        StartCoroutine(PlayAnotherAudioClip(musicList));
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
        ++_currentLevel;
        DontDestroyOnLoad(_player);
        DontDestroyOnLoad(_playerSpawnLocation);
        DontDestroyOnLoad(_playerCamera);
        DontDestroyOnLoad(_canvas);
        DontDestroyOnLoad(_dialogueManager);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _player.transform.position = _playerSpawnLocation.transform.position;
        StartCoroutine(DelayEndReachedReset());
        OnLevelEndReached -= NextLevel;
        RequeueMusic();
    }

    private void GetPlayer(Scene scene, LoadSceneMode mode)
    {
        _player = PlayerScript.GetPlayerInstance();
        _playerSpawnLocation = GameObject.FindGameObjectWithTag(PlayerSpawnLocationTag);
        _playerCamera = Camera.main;
        _canvas = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        _dialogueManager = GameObject.FindGameObjectWithTag(DialogueManage);
        // print(_player.tag);
    }

    public int GetPlayerDamage()
    {
        return _player.GetWeaponDamage();
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

            default:
                Debug.Log("Erreur cette option de chanson n'existe pas! " + option.ToString());
                break;
        }

        if (!_listAudioSources[IndexAudioSourceSpecialBgm].isPlaying)
            _listAudioSources[IndexAudioSourceSpecialBgm].Play();
        Invoke(nameof(CountdownChangeMusic), 1f);
    }

    private void RequeueMusic()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        QueueSong(listLevelBgm);
    }

    private void CountdownChangeMusic()
    {
        --_invincibilityDuration;
        if (_invincibilityDuration <= 0)
        {
            _listAudioSources[IndexAudioSourceSpecialBgm].Stop();
            _listAudioSources[IndexAudioSourceLevelBgm].UnPause();
            return;
        }

        Invoke(nameof(CountdownChangeMusic), 1f);
    }

    private IEnumerator DelayEndReachedReset()
    {
        yield return new WaitForSeconds(5);
        _isEndReached = false;
    }

    public void EnableBossFight()
    {
        _isBossFight = true;
        RequeueMusic();
    }

    public void GameOver(bool isDead)
    {
        var index = GameEndSceneIndex;
        if (isDead)
        {
            index = GameOverSceneIndex;

        }
        SceneManager.LoadScene(index);
        // Destroy(gameObject);
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
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        _listAudioSources[IndexAudioSourceSpecialBgm].Stop();
        Destroy(GameObject.FindGameObjectWithTag(PlayerUiTag));
        Destroy(_playerCamera);
        Destroy(_playerSpawnLocation);
        Destroy(_player);
        Destroy(_canvas);
        Destroy(_dialogueManager);
        Destroy(gameObject);
        Destroy(this);
    }
}