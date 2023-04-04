using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    public GameObject mainMenuUI;

    public AudioMixer audioMixer;
    public void PlayGame ()
    {
        SceneManager.LoadScene("MainHub");
    }

    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();

    }

    public void Options ()
    {
        pauseMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
    }

    public void Back ()
    {
        pauseMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
