using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    
    public AudioMixer mixer;

    public GameObject dialogueBox;

    public void SetVolume (float volume)
    {
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Master Volume", volume);
    }


    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
        Debug.Log("pauseme");
        if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Pause");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
                if (Input.GetKeyDown("p"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (GameIsPaused && pauseMenuUI != null || dialogueBox.activeInHierarchy == true)
        {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        } else
        {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }

    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

    }

    public void Hub()
    {
        SceneManager.LoadScene("MainHub");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
