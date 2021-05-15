using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;
    public static GameManager GameManagerInstance => _gameManager;

    private const string PlayerSpawnLocationTag = "PlayerSpawn";
    private const string PlayerUiTag = "PlayerUI";
    private const string DialogManagerTag = "DialogueManager";
    private const string PauseMenuTag = "PauseMenu";
    private const string MainCamera = "MainCamera";
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
    private Canvas _pauseMenu;
    private AudioSource _levelAudioSource;
    private AudioSource _specialAudioSource;
    private GameObject _playerSpawnLocation;
    private GameObject _dialogueManager;
    private Camera _playerCamera;
    private List<AudioSource> _listAudioSources;
    private PlayerScript _player;
    private int _invincibilityDuration;
    private int _currentLevel;
    private int _playingClipIndex;
    private bool _isEndReached;
    private bool _isMusicPaused;

    public delegate void LoadNextLevel();

    public event LoadNextLevel OnLevelEndReached;

    private void Awake()
    {
        if (_gameManager != null && _gameManager != this)
            Destroy(gameObject);
        else
            _gameManager = this;
    }

    private void Start()
    {
        _listAudioSources = new List<AudioSource>();
        _listAudioSources.AddRange(GetComponents<AudioSource>());
        _levelAudioSource = _listAudioSources[IndexAudioSourceLevelBgm];
        _specialAudioSource = _listAudioSources[IndexAudioSourceSpecialBgm];
        _invincibilityDuration = 0;
        _playingClipIndex = 0;

        // QueueSong(listWelcomeBgm);
        QueueWelcomeSong();

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        OnLevelEndReached?.Invoke();
    }

    public void StartGame()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.sceneLoaded += GetPlayer;
        ++_currentLevel;
        PlayMusic(listLevelBgm);
    }

    public void QuitGame()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        Application.Quit();
    }

    private void QueueWelcomeSong()
    {
        if (_currentLevel != 0 || _levelAudioSource.isPlaying)
            return;
        var index = Random.Range(0, listWelcomeBgm.Count);
        _levelAudioSource.Stop();
        _levelAudioSource.clip = listWelcomeBgm[index];
        _levelAudioSource.Play();
        Invoke(nameof(QueueWelcomeSong), _levelAudioSource.clip.length);
    }

    private void PlayMusic(List<AudioClip> musicList)
    {
        if (musicList == null)
            return;

        var index = Random.Range(0, musicList.Count);
        if (_currentLevel > _playingClipIndex + 1 && !_isMusicPaused)
        {
            index = _currentLevel - 1;
            ++_playingClipIndex;
        }
        else if (_currentLevel == _playingClipIndex + 1)
            index = _currentLevel - 1;

        _levelAudioSource.clip = musicList[index];
        _levelAudioSource.loop = true;
        _levelAudioSource.Play();
    }

    public void ChangeToSpecialBgm()
    {
        if (_listAudioSources[IndexAudioSourceLevelBgm].isPlaying &&
            !_specialAudioSource.isPlaying)
        {
            _listAudioSources[IndexAudioSourceLevelBgm].Pause();
            _specialAudioSource.clip = invincibleBgm;
        }

        _isMusicPaused = true;
        _specialAudioSource.Play();
        _invincibilityDuration = BaseInvincibilityDuration;
        Invoke(nameof(CountdownChangeMusic), 1f);
    }

    private void CountdownChangeMusic()
    {
        --_invincibilityDuration;
        if (_invincibilityDuration <= 0)
        {
            _specialAudioSource.Stop();
            _isMusicPaused = false;
            if (_currentLevel - 1 > _playingClipIndex)
                PlayMusic(listLevelBgm);
            else
                _listAudioSources[IndexAudioSourceLevelBgm].UnPause();

            return;
        }

        Invoke(nameof(CountdownChangeMusic), 1f);
    }

    public void NextLevel()
    {
        OnLevelEndReached -= NextLevel;
        if (_isEndReached)
            return;
        _isEndReached = true;
        ++_currentLevel;
        DontDestroyOnLoad(_player);
        DontDestroyOnLoad(_playerSpawnLocation);
        DontDestroyOnLoad(_playerCamera);
        DontDestroyOnLoad(_canvas);
        DontDestroyOnLoad(_pauseMenu);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _player.transform.position = _playerSpawnLocation.transform.position;
        StartCoroutine(DelayEndReachedReset());
        if (!_isMusicPaused)
            PlayMusic(listLevelBgm);
    }

    private void GetPlayer(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex > FinalLevelScene ||
            SceneManager.GetActiveScene().buildIndex == 0) return;
        _player = PlayerScript.GetPlayerInstance;
        _playerSpawnLocation = GameObject.FindGameObjectWithTag(PlayerSpawnLocationTag);
        _playerCamera = Camera.main;
        _canvas = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        _pauseMenu = GameObject.FindGameObjectWithTag(PauseMenuTag).GetComponent<Canvas>();
        _dialogueManager = GameObject.FindGameObjectWithTag(DialogManagerTag);
    }

    public int GetPlayerDamage()
    {
        return _player.GetWeaponDamage();
    }


    private IEnumerator DelayEndReachedReset()
    {
        yield return new WaitForSeconds(5f);
        _isEndReached = false;
    }

    public void EnableBossFight()
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        _listAudioSources[IndexAudioSourceLevelBgm].clip = bossMusic;
        _listAudioSources[IndexAudioSourceLevelBgm].loop = true;
        _listAudioSources[IndexAudioSourceLevelBgm].Play();
    }

    public void GameOver(bool isDead)
    {
        _listAudioSources[IndexAudioSourceLevelBgm].Stop();
        var index = GameEndSceneIndex;
        if (isDead)
            index = GameOverSceneIndex;

        _listAudioSources[IndexAudioSourceSpecialBgm].Stop();
        Destroy(_playerCamera.GetComponentInChildren<CinemachineVirtualCamera>());
        Destroy(_playerCamera.GetComponent<CinemachineBrain>());
        Destroy(GameObject.FindGameObjectWithTag(MainCamera));
        Destroy(GameObject.FindGameObjectWithTag(PlayerUiTag));
        Destroy(GameObject.FindGameObjectWithTag(PauseMenuTag));
        StartCoroutine(LoadSc(index));
    }

    private IEnumerator LoadSc(int index)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(index);
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
        Destroy(_playerSpawnLocation);
        Destroy(_dialogueManager);
        Destroy(gameObject);
        Destroy(this);
    }
}