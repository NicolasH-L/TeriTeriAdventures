using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerTag = "Player";
    [SerializeField] private List<AudioClip> listWelcomeBgm;
    [SerializeField] private List<AudioClip> listLevelBgm;
    [SerializeField] private GameObject essexSwitchScene;
    private AudioSource _audioSource;
    private GameObject _player;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        QueueSong(listWelcomeBgm);
    }

    void Update()
    {
       
    }

    public void StartGame()
    {
        _audioSource.Stop();
        StopCoroutine(PlayAnotherAudioClip(listWelcomeBgm));
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
}