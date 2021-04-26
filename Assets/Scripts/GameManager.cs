using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    private AudioSource _audioSource;

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