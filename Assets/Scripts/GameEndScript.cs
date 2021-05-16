using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScript : MonoBehaviour
{
    private const int MainMenuSceneIndex = 0;
    [SerializeField] private AudioClip music;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (music == null || _audioSource == null)
            return;
        _audioSource.clip = music;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void MainMenu()
    {
        _audioSource.Stop();
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void QuitGame()
    {
        _audioSource.Stop();
        Application.Quit();
    }
}