using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerTag = "Player";
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private GameObject essexSwitchScene;
    private AudioSource _audioSource;
    private GameObject _player;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        QueueSong();
    }

    void Update()
    {
        if (!_audioSource.isPlaying)
        {
            QueueSong();
        }
    }

    public void StartGame()
    {
        _audioSource.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        print(SceneManager.GetActiveScene().buildIndex);
        _player = GameObject.FindGameObjectWithTag(PlayerTag);
        print(_player.tag);
    }

    public void QuitGame()
    {
        _audioSource.Stop();
        Application.Quit();
    }

    private void QueueSong()
    {
        _audioSource.clip = listWelcomeBgm[Random.Range(0, listWelcomeBgm.Count)];
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}