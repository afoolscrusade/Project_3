using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    
    public AudioMixer audioMixer;

    public GameObject dialogueBox;

    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
        
        
    void Update()
    {
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

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}